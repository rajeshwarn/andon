using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppLog;

namespace LineService
{
    public enum MoveType { Sync = 0, Async = 1 }

    /// <summary>
    /// Dispatcher is responsible for:
    /// - moving products on line
    /// - setting products on stations when restoring line state
    /// Works with Product, IStation, 
    /// </summary>
    class Dispatcher
    {
        private int id;
        private IEnumerable<IStation> lineStations;
        private MoveType moveType = MoveType.Async;
        private AppLog.LogProvider myLog;


        public Dispatcher()
        {
            List<IStation> lineStations = new List<IStation>();

            lineStations.Add(new TestStation() { Name = "Station A" });
            lineStations.Add(new TestStation() { Name = "Station B1" });
            lineStations.Add(new TestStation() { Name = "Station B2" });
            lineStations.Add(new TestStation() { Name = "Station C" });
            lineStations.Add(new TestStation() { Name = "Station D1" });
            lineStations.Add(new TestStation() { Name = "Station D2" });
            lineStations.Add(new TestStation() { Name = "Station D3" });
            lineStations.Add(new TestStation() { Name = "Station E" });
            lineStations.Add(new TestStation() { Name = "Station F" });

            this.lineStations = lineStations;
        }
        public Dispatcher(int id, IEnumerable<IStation> lineStations, AppLog.LogProvider log)
        {
            this.id = id;
            this.myLog = log;
            if (myLog == null) 
            {
                myLog = new AppLog.LogProvider(AppLog.LogType.File, "null_dispatcher_log.txt", true);
            }

            this.lineStations = lineStations;
            // test // this.lineStations.First().Name = "ABC";

            foreach (IStation station in this.lineStations) 
            { 
                station.OnFinished += new EventHandler<DispatcherStationArgs>(this.station_OnFinished);
                station.OnFree += new EventHandler<DispatcherStationArgs>(this.station_OnFree);
            }

        }

        public bool LoadProduct(Product product)
        {
            return MoveProduct(product, null); ;
        }
        public IStation GetStationByName(string stationName)
        {
            IStation result = lineStations.FirstOrDefault(p => p.Name.Equals(stationName));
            return result;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public bool MoveProduct2(Product product, IStation station)
        {
            try
            {
                string proName = product != null ? product.Name : "product is null";
                string staName = station != null ? station.Name : "station is null";
                Console.WriteLine(String.Format("Call MoveProduct({0}, {1})", proName, staName));

                if (product == null)
                {
                    Console.WriteLine(String.Format("Product is null in dispatcher.moveProduct()"));
                    if (this.OnAlert != null) this.OnAlert(this, new DispatcherStationArgs() { Message = "Product is null in dispatcher.moveProduct()" });
                    return false;
                }

                bool result = false;
                string stationName = "NA";
                bool stationShouldRelease = true;
                bool faultResult = false;
                bool busyResult = false;
                List<string> nextStationNames;
                List<IStation> destinationList = new List<IStation>();
                List<string> destinationNames = new List<string>();
                List<IStation> tmpList = new List<IStation>();


                // Set State value instead of stationName if station param is null
                // ------------------------------------------------------------------------------------------
                if (station != null)
                {
                    stationName = station.Name;
                }
                else
                {
                    stationName = product.Router.State;
                    stationShouldRelease = false;
                }

                // product can be on several stations at the beginning
                List<string> oldStationNames = product.Router.FindStateStations();

                // Try to go forward with router
                // Router is a Finite-State-Machine that calculates next state using information about current station
                // ------------------------------------------------------------------------------------------
                string nextState = product.Router.FindNextState(stationName);

                //this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "moveProduct()",
                //          product.Name + " next state: " + nextState, "system");

                if (nextState != "" & !product.Router.IsNextStateFinal)
                {
                    // Get stations where product should be in the new state
                    // but only destionation stations there Product is not presented yet
                    // ------------------------------------------------------------------------------------------

                    nextStationNames = product.Router.NextStationNames(stationName);

                    foreach (IStation newStation in this.lineStations)
                    {
                        // and avoid to move product to the same station
                        // ------------------------------------------------------------------------------------------
                        //if (nextStationNames.Contains(newStation.Name) && !oldStationNames.Contains(newStation.Name))
                        if (nextStationNames.Contains(newStation.Name) && !oldStationNames.Contains(newStation.Name))
                        {
                            // It's a right station to Move, but check if it isn't busy ?! and sum result
                            // ------------------------------------------------------------------------------------------
                            destinationList.Add(newStation);
                            bool sameProduct = (newStation.CurrentProduct != null) ? (newStation.CurrentProduct.Name == product.Name) : false;
                            if (newStation.IsFull & !sameProduct)
                            {
                                busyResult = (busyResult | true);
                                this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "moveProduct()",
                                    product.Name + ", dispatcher trying to move, but station is busy (isFull): " + newStation.Name, "system");
                            }

                            // ?????????? never true
                            //if (stationShouldRelease && newStation.Name == station.Name)  
                            //    stationShouldRelease = false;
                        }
                    }

                    // It's impossible to Move if at least one station in destination list is busy
                    // ------------------------------------------------------------------------------------------
                    if (!busyResult)
                    {
                        // Count successfull additions
                        // ------------------------------------------------------------------------------------------
                        foreach (IStation newStation in destinationList)
                        {
                            this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "moveProduct()",
                                product.Name + ", dispatcher trying to move to station: " + newStation.Name, "system");

                            if (!this.sendProductToStation(product, newStation))
                            {
                                faultResult = (faultResult | true);
                                this.myLog.LogAlert(AlertType.Error, id.ToString(), this.GetType().ToString(), "moveProduct()",
                                    product.Name + ", dispatcher FAILED to move to station: " + newStation.Name, "system");
                            }
                            else
                            {
                                tmpList.Add(newStation);
                                destinationNames.Add(newStation.Name);
                            }
                        }

                        if (faultResult)
                        {
                            // rollback Add operation
                            foreach (IStation tmpSt in tmpList)
                            {
                                if (tmpSt.CurrentProduct != null && tmpSt.CurrentProduct.Name == product.Name && tmpSt.Name != stationName)
                                {
                                    this.myLog.LogAlert(AlertType.Error, "NA", this.GetType().ToString(), "moveProduct()", 
                                        product.Name + ", dispatcher trying to rollback from station: " + tmpSt.Name, "system");
                                    tmpSt.ReleaseProduct();
                                }
                            }

                            this.myLog.LogAlert(AlertType.Error, id.ToString(), this.GetType().ToString(), "moveProduct()",
                                product.Name + ", move FAULT result!", "system");

                            return false;
                        }

                        // If sent to destination then release from current station
                        // If all additions are successfull then release product from current station
                        // else rollback router and restore state
                        // ------------------------------------------------------------------------------------------
                        if (nextStationNames.Count > 0)
                        {
                            result = true;
                            product.Router.Go(stationName);
                        }
                        else
                        {
                            stationShouldRelease = false;
                        }
                    }
                    else
                    {
                        stationShouldRelease = false;
                    }
                }
                else
                {
                    stationShouldRelease = false;
                }


                if (product.Router.IsNextStateFinal) 
                {
                    stationShouldRelease = true;
                }

                //this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "moveProduct()",
                //          product.Name + " from " + stationName + " stationShouldRelease = " + stationShouldRelease.ToString(), "system");

                //string currentProductName = (station != null && station.CurrentProduct != null) ? station.CurrentProduct.Name : "";
                //if (stationShouldRelease & currentProductName == product.Name)

                
                if (stationShouldRelease)
                {
                    if (station != null && station.CurrentProduct != null && station.CurrentProduct.Name == product.Name) 
                    {
                        this.releaseProductFromStation(product, station);
                    }
                }

                if (result && this.OnProductMoved != null)
                {
                    if (destinationNames.Count == 0)
                    {
                        string msg = product.Name + ", dispatcher Result == true, but DestinationList is empty";
                        this.myLog.LogAlert(AlertType.Error, id.ToString(), this.GetType().ToString(), "moveProduct()", msg, "system");
                    }
                    this.OnProductMoved(this, new DispatcherMoveArgs() { Product = product, DestinationList = destinationNames });
                }


                //this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "moveProduct()",
                //          product.Name + " is finished?!", "system");

                if (product.Router.IsNextStateFinal)
                {
                    this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "moveProduct()",
                              product.Name + " finished the line!", "system");

                    if (this.OnProductFinishedLine != null)
                        this.OnProductFinishedLine(this, new DispatcherMoveArgs() { Product = product });
                }
                else 
                {
                    this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "moveProduct()",
                        product.Name + " is not finished, state = " + product.Router.State, "system");
                }

                return result;
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, id.ToString(), this.GetType().ToString(), "moveProduct()",
                    ex.Message, "system");

                string productName = (product != null) ? product.Name : "product is null";
                string stationName = (station != null) ? station.Name : "station is null";

                this.myLog.LogAlert(AlertType.Error, id.ToString(), this.GetType().ToString(), "moveProduct()",
                    productName + " on " + stationName + ": " + ex.Message, "system");

                return false;
            }
        }





        public void MoveProductsOnLine() 
        {
            for(int i = lineStations.Count() -1; i >= 0; i--) 
            {
                IStation station = lineStations.ElementAt(i);
                //Product product = station.CurrentProduct;
                //this.MoveProduct(product, station);

                station.Finish();
            }
        }
        public void RestoreProductOnStation(Product product, string stationName) 
        {
            IStation station = this.lineStations.FirstOrDefault(p => p.Name.Equals(stationName));
            if (station != null)
            {
                station.AddProduct(product);
            }
        }

        public event EventHandler<DispatcherMoveArgs> OnProductMoved;
        public event EventHandler<DispatcherMoveArgs> OnProductFinishedLine;
        public event EventHandler<DispatcherStationArgs> OnFreeStations;
        public event EventHandler<DispatcherStationArgs> OnAlert;

        private bool sendProductToStation(Product product, IStation station)
        {
            bool result = false;
            if (station != null && product != null)
                result = station.AddProduct(product);
            return result;
        }
        private void releaseProductFromStation(Product product, IStation station)
        {
            if (station != null && product != null && station.CurrentProduct != null && station.CurrentProduct.Name == product.Name)
                station.ReleaseProduct();
        }


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        private void station_OnFinished(object sender, DispatcherStationArgs e) 
        {
            try
            {
                Console.WriteLine(String.Format("{0} {1} - OnFinised opened.", DateTime.Now.ToString(), e.Station.Name));
                this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "station_OnFinished()",
                                String.Format("{0} - OnFinised opened.", e.Station.Name), "system");

                if (e.Product != null)
                {
                    if (this.moveType == MoveType.Async)
                    {
                        bool isAnyNextState = (e.Product.Router.FindNextState(e.Station.Name) != "");
                        if (isAnyNextState)
                        {
                            this.MoveProduct(e.Product, e.Station);
                        }
                        else 
                        {
                            e.Station.ResetFinish();
                            this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "station_OnFinished()",
                                String.Format("{0} finish button reset! {1}", e.Station.Name, e.Product.Name), "system");
                        }
                    } 
                }
                else 
                {
                    Console.WriteLine(String.Format("Error! Station: {0} - product is null!", e.Station.Name));
                }

                //Console.WriteLine(String.Format("{0} {1} - OnFinised closed.", DateTime.Now.ToString(), e.Station.Name));
                //this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "station_OnFinished()",
                //                                String.Format("{0} - OnFinised closed.", e.Station.Name), "system");
            }
            catch (Exception ex) 
            {
                this.myLog.LogAlert(AlertType.Error, id.ToString(), this.GetType().ToString(), "station_OnFinished()",
                    String.Format("{0} - OnFinised exception: {1}", e.Station.Name, ex.ToString()), "system");
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        private void station_OnFree(object sender, DispatcherStationArgs e) 
        {
            if (this.moveType == MoveType.Async)
            {

                try
                {
                    this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "station_OnFree()",
                        String.Format("{0} - OnFree() opened.", e.Station.Name), "system");


                    Product waitingProduct = null;
                    List<IStation> waitingStations = this.findWatingProducts(e.Station);
                    if (waitingStations != null && waitingStations.Count > 0)
                    {
                        foreach (IStation waitingStation in waitingStations)
                        {
                            waitingProduct = waitingStation.CurrentProduct;
                            if (waitingProduct != null)
                            {
                                this.MoveProduct(waitingProduct, waitingStation);
                            }
                            else
                            {
                                //
                            }
                        }
                    }
                    else 
                    {
                        if (this.OnFreeStations != null)
                            this.OnFreeStations(this, new DispatcherStationArgs() { Product = null, Station = e.Station });
                    }

                    //this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "station_OnFree()",
                    //    String.Format("{0} - OnFree() closed.", e.Station.Name), "system");

                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    this.myLog.LogAlert(AlertType.Error, id.ToString(), this.GetType().ToString(), "station_OnFree()",
                        ex.Message, "system");

                    this.myLog.LogAlert(AlertType.Error, id.ToString(), this.GetType().ToString(), "station_OnFree()",
                        ex.StackTrace.ToString(), "system");
                }

            }
        }

        private IStation findWatingProduct(IStation station)
        {
            IStation result = null;
            try
            {
                for (int i = this.lineStations.Count() - 1; i >= 0; i--) 
                {
                    IStation cStation = this.lineStations.ElementAt(i);
                    Product product = cStation.CurrentProduct;

                    if (product != null && cStation.IsFinished) 
                    {
                        List<string> nextStationNames = product.Router.NextStationNames(cStation.Name);
                        foreach (string nextStName in nextStationNames) 
                        {
                            if (nextStName == station.Name) 
                            {
                                //this.MoveProduct(product, cStation);
                                result = cStation;
                                return result;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //this.myLog.LogAlert(AlertType.Error, this.name, this.GetType().ToString(), "findWatingProduct()", ex.ToString(), "system");
                Console.WriteLine(ex);
            }
            return result;
        }

        private List<IStation> findWatingProducts(IStation station)
        {
            List<IStation> result = new List<IStation>();
            try
            {
                foreach(IStation cStation in this.lineStations)
                {
                    Product product = cStation.CurrentProduct;
                    if (product != null && cStation.IsFinished)
                    {
                        List<string> nextStationNames = product.Router.NextStationNames(cStation.Name);
                        if(nextStationNames.Contains(station.Name))
                        {
                            result.Add(cStation);
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                //this.myLog.LogAlert(AlertType.Error, this.name, this.GetType().ToString(), "findWatingProduct()", ex.ToString(), "system");
                Console.WriteLine(ex);
            }
            return result;
        }


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public bool MoveProduct(Product product, IStation station)
        {
            bool result = false;
            try
            {
                #region initialisation ...
                string productName = product != null ? product.Name : "product is null";
                string stationName = station != null ? station.Name : "station is null";
                //Console.WriteLine(String.Format("Call MoveProduct({0}, {1})", productName, stationName));
                if(station != null)
                    this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "moveProduct()",
                          String.Format("Call MoveProduct({0}, {1})", productName, stationName), "system");

                if (product == null)
                {
                    Console.WriteLine("Product is null in dispatcher.moveProduct()");
                    this.myLog.LogAlert(AlertType.Error, id.ToString(), this.GetType().ToString(), "moveProduct()",
                        "Product is null in dispatcher.moveProduct()", "system");
                    return false;
                }

                // flag show if product leaved the station
                // if product de facto moded than station should release it
                bool stationShouldRelease = true;

                // flag used to be sure that all movements of the product completed successfully
                // is used to rollback the move
                bool faultResult = false;

                // flag used to know if one of the destination stations is busy
                // is used to rollback the move
                bool busyResult = false;


                List<string> nextStationNames;
                List<IStation> destinationList = new List<IStation>();
                List<string> destinationNames = new List<string>();
                List<IStation> tmpList = new List<IStation>();


                // Set State value instead of stationName if station param is null
                // ------------------------------------------------------------------------------------------
                if (station != null) //START_STATE ?
                {
                    stationName = station.Name;
                }
                else
                {
                    //START_STATE
                    stationName = product.Router.State;
                    stationShouldRelease = false;
                }
                #endregion

                List<string> oldStationNames = product.Router.FindStateStations();

                // Try to go forward with router
                // Router is a Finite-State-Machine that calculates next state using information about current station
                // ------------------------------------------------------------------------------------------
                string nextState = product.Router.FindNextState(stationName);
                //this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "moveProduct()",
                //          productName + " next state: " + nextState, "system");

                // if Router can't find next state for the symbol, than state == ""
                // (station is used as a transition symbol for FSM)
                // Exit if router have no way to go (any next state)
                if (nextState == "") return result;

                // exit if next state is final
                //this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "moveProduct()",
                //          product.Name + " is finished?!", "system");
                if (checkIfNextStateFinal(product, station)) return result;


                // Find stations where product should be in the new state
                // but only destionation stations there Product is not presented yet
                // make "destinationList"
                // ------------------------------------------------------------------------------------------
                nextStationNames = product.Router.NextStationNames(stationName);
                if (nextStationNames == null || nextStationNames.Count <= 0) return result;

                foreach (IStation newStation in this.lineStations)
                {
                    // and avoid to move product to the same station or to old station
                    if (nextStationNames.Contains(newStation.Name) && !oldStationNames.Contains(newStation.Name))
                    {
                        destinationList.Add(newStation);
                    }
                }


                // Check ALL destinations for avalability
                // So we have destinationList to Move, but check if they aren't busy ?! and sum result
                // ------------------------------------------------------------------------------------------
                foreach (IStation newStation in destinationList) 
                {
                    bool sameProduct = (newStation.CurrentProduct != null) ? (newStation.CurrentProduct.Name == product.Name) : false;
                    if (newStation.IsFull & !sameProduct)
                    {
                        busyResult = true;
                        //this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "moveProduct()",
                        //    product.Name + ", dispatcher trying to move, but station is busy (isFull): " + newStation.Name, "system");
                    }
                }
                // it's impossible to Move if at least one station in destination list is busy
                if (busyResult) 
                {
                    if (station != null) 
                    {
                        station.BitState = station.BitState | (int)BSFlag.Blocked;
                        this.myLog.LogAlert(AlertType.Info, id.ToString(), this.GetType().ToString(), "moveProduct()",
                            String.Format("Station {0} is blocked, chassis {1}", station.Name, productName), "system");

                        
                        this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "moveProduct()",
                            String.Format("Station {0} BitState = {1}", station.Name, station.BitState.ToString()), "system");
                    }
                    return result;
                } 

                
                // Try to send product to all destinations
                // count successfull attempts
                // ------------------------------------------------------------------------------------------
                foreach (IStation newStation in destinationList)
                {
                    this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "moveProduct()",
                        product.Name + ", dispatcher trying to move to station: " + newStation.Name, "system");

                    if (!this.sendProductToStation(product, newStation))
                    {
                        faultResult = true;
                        this.myLog.LogAlert(AlertType.Error, id.ToString(), this.GetType().ToString(), "moveProduct()",
                            product.Name + ", dispatcher FAILED to move to station: " + newStation.Name, "system");
                    }
                    else
                    {
                        tmpList.Add(newStation);
                        destinationNames.Add(newStation.Name);
                    }
                }

                if (faultResult)
                {
                    // rollback sending operations
                    foreach (IStation tmpSt in tmpList)
                    {
                        if (tmpSt.CurrentProduct != null && tmpSt.CurrentProduct.Name == productName && tmpSt.Name != stationName)
                        {
                            this.myLog.LogAlert(AlertType.Error, id.ToString(), this.GetType().ToString(), "moveProduct()",
                                product.Name + ", dispatcher trying to rollback from station: " + tmpSt.Name, "system");
                            tmpSt.RollbackProductByName(productName);
                        }
                    }
                    this.myLog.LogAlert(AlertType.Error, id.ToString(), this.GetType().ToString(), "moveProduct()",
                        product.Name + ", move FAULT result!", "system");

                    return false;
                }
                

                // If sent to destination or merge with oldstation then release from current station
                // If all additions are successfull then release product from current station
                // else rollback router and restore state
                // ------------------------------------------------------------------------------------------
                result = true;
                product.Router.Go(stationName);
                //this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "moveProduct()",
                //          productName + " from " + stationName + " stationShouldRelease = " + stationShouldRelease.ToString(), "system");

                if (result && this.OnProductMoved != null)
                    this.OnProductMoved(this, new DispatcherMoveArgs() { Product = product, DestinationList = destinationNames });
               

                if (stationShouldRelease)           
                    this.releaseProductFromStation(product, station);

                return result;
            }
            catch (Exception ex)
            {
                string productName = (product != null) ? product.Name : "product is null";
                string stationName = (station != null) ? station.Name : "station is null";

                this.myLog.LogAlert(AlertType.Error, id.ToString(), this.GetType().ToString(), "moveProduct()",
                    productName + " on " + stationName + ": " + ex.Message, "system");

                return false;
            }
        }

        private bool checkIfNextStateFinal(Product product, IStation station) 
        {
            bool result = false;

            string productName = product != null ? product.Name : "product is null";
            if (product.Router.IsNextStateFinal)
            {
                this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "moveProduct()",
                          productName + " finished the line!", "system");

                // tell station to remove the product
                this.releaseProductFromStation(product, station);

                // tell owner to remove the product from the line
                if (this.OnProductFinishedLine != null)
                    this.OnProductFinishedLine(this, new DispatcherMoveArgs() { Product = product });

                // exit, because moving is not needed
                result = true;
            } 
            return result;
        }


    }
}
