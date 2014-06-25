using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppLog;
using LineService.DetroitDataSetTableAdapters;
using System.Data;

namespace LineService
{

    /// <summary>
    /// Supplier is responsible for:
    /// - load products on line from Queue
    /// - load products from Buffer and Inbox
    /// - upload products from line to Stock or Outbox
    /// - restore itself and porducts on line
    /// </summary>
    class Supplier
    {
        private int id;
        private ProductBuffer productBuffer;
        private ProductStock productStock;
        private BatchStock batchStock;
        private LineQueue lineQueue;
        private LogProvider myLog;
        private BatchesOnLine batchesOnLine;
        private ProductsOnLine productsOnLine;
        private UncompletedProducts uncompletedProducts;

        private Dispatcher lineDispatcher;

        private DetroitDataSet detroitDataSet;
        private DetroitDataSet.LineSnapshotDataTable lineSnapshotTable; // = new DetroitDataSet.LineSnapshotDataTable();
        private LineSnapshotTableAdapter lineSnapshotTableAdapter;
        private ProductBufferTableAdapter productOutBufferTableAdapter;
        private ProductBufferTableAdapter productInboxBufferTableAdapter;

        public Supplier(int id, DetroitDataSet detroit, LogProvider logProvider, Dispatcher lineDispatcher)
        {
            this.id = id;
            this.detroitDataSet = detroit;
            this.myLog = logProvider;
            this.lineDispatcher = lineDispatcher;

            this.lineSnapshotTableAdapter = new LineSnapshotTableAdapter();
            this.lineSnapshotTable = this.detroitDataSet.LineSnapshot;
            this.lineSnapshotTableAdapter.Fill(this.lineSnapshotTable, this.detroitDataSet.LineId);
            
            this.productOutBufferTableAdapter = new ProductBufferTableAdapter();
            this.productInboxBufferTableAdapter = new ProductBufferTableAdapter();

            this.productBuffer = new ProductBuffer();
            this.lineQueue = new LineQueue(detroit, logProvider);

            this.batchesOnLine = new BatchesOnLine();
            //this.batchesOnLine.OnListChange += new EventHandler(this.batchesOnLine_OnListChange);
            this.productsOnLine = new ProductsOnLine();

            this.productStock = new ProductStock(detroit);
            this.batchStock = new BatchStock();

            this.uncompletedProducts = new UncompletedProducts(detroit);
            this.lineDispatcher.OnProductMoved += new EventHandler<DispatcherMoveArgs>(lineDispatcher_OnProductMoved);
            this.lineDispatcher.OnProductFinishedLine += new EventHandler<DispatcherMoveArgs>(lineDispatcher_OnProductFinishedLine);
            this.lineDispatcher.OnFreeStations += new EventHandler<DispatcherStationArgs>(lineDispatcher_OnFreeStations);
        }

        public void SetUncompletedProduct(Product product)
        {
            this.uncompletedProducts.Enqueue(product);
        }
        public void StartNewProduct() 
        {
            try
            {
                this.fillProductBufferFromInbox();
                this.fillProductBuffer();
                this.loadProductOnLine();
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }
        public void Backup() { }
        public void Restore() 
        {
            try
            {
                this.restoreProductsFormLineSnapshot();
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }
        public ProductBase[] GetProductStock() 
        {
            List<ProductBase> pbStock = new List<ProductBase>();

            try
            {
                foreach (Product product in this.productStock)
                {
                    ProductBase pb = new ProductBase() { Name = product.Name, Id = product.Id };
                    pbStock.Add(pb);
                }
                return pbStock.ToArray();

            }
            catch (Exception ex) 
            {
                //Console.WriteLine(ex);
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(), ex.Source, ex.ToString(), "system"); 
                return new ProductBase[0];
            }

            //ProductBase[] result = new ProductBase[productStock.Count];
            //for (int i = 0; i < productStock.Count; i++)
            //{
            //    result[i] = new ProductBase();
            //    result[i].Name = productStock.ToArray()[i].Name;
            //    result[i].Id = productStock.ToArray()[i].Id;
            //}
            //return result;        
        }

        public ProductBase[] GetProductBuffer() 
        {
            List<ProductBase> pbBuffer = new List<ProductBase>();

            try
            {
                foreach (Product product in this.productBuffer)
                {
                    ProductBase pb = new ProductBase() { Name = product.Name, Id = product.Id };
                    pbBuffer.Add(pb);
                }
                return pbBuffer.ToArray();

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex);
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(), ex.Source, ex.ToString(), "system");                 
                return new ProductBase[0];
            }
        }
        public IEnumerable<Batch> GetBatchesOnLine() 
        {
            IEnumerable<Batch> batchList = (IEnumerable<Batch>)this.batchesOnLine;
            return batchList;
        }
        public LogisticInfo GetNextBatchInfo() 
        {
            LogisticInfo result = new LogisticInfo();

            try
            {
                result.NextBatchName = "NA";
                result.TaktsTillNextBatch = 0;

                Batch nextBatch;
                if (this.lineQueue.Count > 0)
                {
                    nextBatch = this.lineQueue.Peek();
                    result.NextBatchName = (nextBatch != null) ? nextBatch.Name : "";
                    result.TaktsTillNextBatch = (nextBatch != null) ? this.productBuffer.Count : 0;
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }

            return result;           
        }
        public void RefreshLineQueue() 
        {
            try
            {
                this.lineQueue.Refresh();
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }

        /// <summary>
        /// restoreProductsFormLineSnapshot()
        /// Restores objects form Detroit database, table LineSnapshot:
        /// - products on line
        /// - product buffer
        /// - batches on line
        /// - ? uncomplete products
        /// </summary>
        private void restoreProductsFormLineSnapshot()
        {
            try
            {
                DetroitDataSetTableAdapters.LineSnapshotReaderTableAdapter readerTableAdapter = new LineSnapshotReaderTableAdapter();
                readerTableAdapter.Fill(this.detroitDataSet.LineSnapshotReader, this.detroitDataSet.LineId);

                int i = 0;
                while (i < this.detroitDataSet.LineSnapshotReader.Rows.Count)
                {
                    DataRow queueRow = this.detroitDataSet.LineSnapshotReader.Rows[i];

                    int lineId = Convert.ToInt32(queueRow["LineId"]);
                    if (lineId == this.detroitDataSet.LineId)
                    {

                        //----------------------------------------------------------------
                        // 2. create batch for product if necessary and put it into BatchesOnLine.
                        // Check if there is already batch with ID in "BatchesOnLine"
                        //----------------------------------------------------------------
                        int batchId = Convert.ToInt32(queueRow["BatchId"]);
                        Batch enBatch = batchesOnLine.FirstOrDefault(p => p.Id.Equals(batchId));
                        if (enBatch != null)
                        {
                            // batch already exists in "BatchesOnLine"       
                        }
                        else // create new batch
                        {
                            enBatch = new Batch(
                                null, queueRow["BatchNummer"].ToString(), queueRow["BatchType_Name"].ToString()
                                , (int)queueRow["Capacity"], (int)queueRow["BatchId"], (int)queueRow["BatchTypeId"], 0
                                , (int)queueRow["Takt"]
                            );
                            batchesOnLine.Enqueue(enBatch);
                            enBatch.PutOnLine();
                        }

                        //-------------------------------------------------------------------------------
                        // 3. create product, link it to batch and put into this.productBuffer queue 
                        //-------------------------------------------------------------------------------
                        int productId = Convert.ToInt32(queueRow["ProductNummer"]);
                        Product enProduct = this.productsOnLine.FirstOrDefault(p => p.Id.Equals(productId));
                        if (enProduct == null)
                        {
                            enProduct = new Product(enBatch, productId, this.myLog, this.detroitDataSet);
                        }
                        else
                        {
                            enBatch.AddProduct(enProduct);
                        }


                        int stationId = Convert.ToInt32(queueRow["StationId"]);
                        if (stationId < 0)
                        {
                            productBuffer.Enqueue(enProduct);
                        }
                        else if (stationId > 0)
                        {
                            string stationName = queueRow["StationName"].ToString();

                            this.lineDispatcher.RestoreProductOnStation(enProduct, stationName);
                            if (!this.productsOnLine.Contains(enProduct))
                            {
                                this.productsOnLine.Enqueue(enProduct);
                            }

                            string savedState = queueRow["RouterState"].ToString();
                            enProduct.Router.Restore(savedState);
                        }


                    }

                    //-------------------------------------------------------------------------------
                    // 4. next row from ProductBufferTable
                    //-------------------------------------------------------------------------------
                    i++;

                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }

        }
        private void updateProductInLineSnapshot(Product product) 
        {
            if (product != null)
            {
                //Console.WriteLine("Handler: On product moved. Product = " + product.Name);

                try
                {
                    //update products in buffer
                    string select = "BatchId = " + product.Owner.Id.ToString() + " AND " + "ProductNummer = " + product.Id.ToString();
                    DataRow[] lsRows = this.lineSnapshotTable.Select(select);
                    int fTaktSecondsLeft = 0;

                    if (lsRows.Count() > 0)
                    {
                        DataRow lsRow = lsRows[0];
                        fTaktSecondsLeft = (lsRow.RowState != DataRowState.Deleted) ? Convert.ToInt32(lsRow["TaktSecondsLeft"]) : 0;

                        foreach (DataRow oldRow in lsRows)
                        {
                            if (oldRow.RowState != DataRowState.Deleted)
                            {
                                oldRow.Delete();
                            }
                        }

                        //x this.lineSnapshotTableAdapter.Update(this.lineSnapshotTable);
                        //x this.lineSnapshotTable.AcceptChanges();

                        if (!product.Router.IsFinalState)
                        {

                            List<string> productStationNames = product.Router.FindStateStations();
                            foreach (string stationName in productStationNames)
                            {


                                IStation station = this.lineDispatcher.GetStationByName(stationName);
                                int pState = Convert.ToInt32(station.ProductPosition(product));

                                if (station != null && product != null)
                                {
                                    this.lineSnapshotTable.AddLineSnapshotRow(
                                        station.Id,
                                        product.Owner.Id,
                                        product.Id,
                                        pState,
                                        this.detroitDataSet.LineId,
                                        product.Name,
                                        fTaktSecondsLeft,
                                        DateTime.Now,
                                        product.Router.State
                                    );

                                }
                            }

                            //x this.lineSnapshotTableAdapter.Update(this.lineSnapshotTable);
                            //x this.lineSnapshotTable.AcceptChanges();
                        }
                    }
                }
                catch (DBConcurrencyException ex)
                {
                    //Console.WriteLine(ex);
                    this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(), ex.Source, ex.ToString(), "system"); 
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex);
                    this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(), ex.Source, ex.Message, "system"); 
                }

            }
        }
        public void UpdateLineSnaphot() 
        {
            try
            {
                this.lineSnapshotTable.AcceptChanges();
                this.lineSnapshotTable.Clear();

                foreach (Product product in this.productBuffer)
                {
                    this.lineSnapshotTable.AddLineSnapshotRow(
                        -1,
                        product.Owner.Id,
                        product.Id,
                        0,
                        this.detroitDataSet.LineId,
                        product.Name,
                        0,
                        DateTime.Now,
                        product.Router.State
                    );
                }

                foreach (Product product in this.productsOnLine)
                {
                    List<string> productStationNames = product.Router.FindStateStations();
                    foreach (string stationName in productStationNames)
                    {
                        IStation station = this.lineDispatcher.GetStationByName(stationName);
                        if (station != null && !product.Router.IsFinalState)
                        {
                            int pState = Convert.ToInt32(station.ProductPosition(product));

                            this.lineSnapshotTable.AddLineSnapshotRow(
                               station.Id,
                               product.Owner.Id,
                               product.Id,
                               pState,
                               this.detroitDataSet.LineId,
                               product.Name,
                               0,
                               DateTime.Now,
                               product.Router.State
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }
        private void lineDispatcher_OnProductMoved(object sender, DispatcherMoveArgs e) 
        {
            // This functionality is providing by SlowTask!
            // this.updateProductInLineSnapshot(e.Product);
         }
        private void lineDispatcher_OnProductFinishedLine(object sender, DispatcherMoveArgs e) 
        {
            try
            {
                this.uploadProductFromLine(e.Product);
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }
        private void lineDispatcher_OnFreeStations(object sender, DispatcherStationArgs e) 
        {
            try
            {

                if (productBuffer.Count == 0)
                {
                    this.fillProductBuffer();
                    this.fillProductBufferFromInbox();
                }

                if (this.productBuffer.Count > 0)
                {
                    Product productInBuffer = this.productBuffer.Peek();
                    List<string> stationNames = productInBuffer.Router.NextStationNames(productInBuffer.Router.State);
                    foreach (string stName in stationNames)
                    {
                        if (stName == e.Station.Name)
                        {
                            this.loadProductOnLine();
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                this.myLog.LogAlert(AlertType.Error, id.ToString(), this.GetType().ToString(), "lineDispatcher_OnFreeStations()",
                    ex.Message, "system");

                this.myLog.LogAlert(AlertType.Error, id.ToString(), this.GetType().ToString(), "lineDispatcher_OnFreeStations()",
                    ex.StackTrace.ToString(), "system");                
            }
        }
        
        private void fillProductBuffer()
        {
            try
            {
                // fill the buffer only if it is empty !
                if (this.productBuffer.Count == 0)
                {
                    Batch enBatch = null;

                    // check database for new batches in queue ?!
                    lineQueue.Refresh();

                    // check and get first batch from LineQueue
                    if (lineQueue.Count > 0)
                    {
                        enBatch = lineQueue.Peek();
                    }
                    if (enBatch == null)
                    {
                        //myLog.LogAlert(AlertType.System, detroitDataSet.LineId.ToString(), this.GetType().ToString(), "fillProductBuffer()",
                        //    String.Format("Line Queue is empty! There is no any batch."), "system");
                    }
                    else
                    {
                        lineQueue.Dequeue();  // remove the batch from LineQueue !!
                        enBatch.PutOnLine();  // change state of the Batch

                        myLog.LogAlert(AlertType.Info, detroitDataSet.LineId.ToString(), this.GetType().ToString(), "fillProductBuffer()",
                            String.Format("Putting batch {0} on line #{1}.", enBatch.Name, detroitDataSet.LineId.ToString()), "system");

                        int i = 0;
                        while (i < enBatch.Capacity)
                        {
                            i++;
                            Product newProduct = new Product(enBatch, i, this.myLog, this.detroitDataSet);
                            this.productBuffer.Enqueue(newProduct);
                            enBatch.AddProduct(newProduct);
                            myLog.LogAlert(AlertType.Info, detroitDataSet.LineId.ToString(), this.GetType().ToString(), "fillProductBuffer()",
                                String.Format("Putting new product {0} on line #{1}.", newProduct.Name, detroitDataSet.LineId.ToString()), "system");

                            // This functionality is providing by SlowTask!
                            // put product in table in Detroit LineSnapshot for plan calculation
                            //
                            //this.lineSnapshotTable.AddLineSnapshotRow(
                            //    -1,
                            //    enBatch.Id,
                            //    newProduct.Id,
                            //    (int)ProductState.InBuffer,
                            //    this.detroitDataSet.LineId,
                            //    newProduct.Name,
                            //    123456, //TODO: this.taktDuration - this.lineCounter,
                            //    DateTime.Now,
                            //    newProduct.Router.State
                            //);
                        }
                        //x this.lineSnapshotTableAdapter.Update(this.lineSnapshotTable);
                        batchesOnLine.Enqueue(enBatch);
                    }
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }

        }
        private void loadProductOnLine()
        {
            try
            {
                if (this.productBuffer.Count != 0)
                {
                    Product product = productBuffer.Peek();
                    if (this.lineDispatcher.LoadProduct(product))
                    {
                        this.productsOnLine.Enqueue(product);
                        this.productBuffer.Dequeue();
                    }
                }
                else
                {
                    //myLog.LogAlert(AlertType.System, this.detroitDataSet.LineId.ToString(), this.GetType().ToString(), "loadProductOnLine()",
                    //    String.Format("Couldn't load product from buffer cause it's empty."), "system");
                }
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }

        /// <summary>
        ///  It seem's that product is finished and going to:
        ///  a. uncompleted products
        ///  b. next line
        ///  c. the stock
        ///------------------------------------------------------------------
        /// </summary>
        /// <param name="enProduct"></param>
        /// <returns></returns>
        private bool uploadProductFromLine(Product enProduct)
        {
            try
            {
                string productName2 = enProduct != null ? enProduct.Name : "product is null";
                this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "uploadProductFromLine()",
                    productName2 + " is trying to upload from line ...", "system");



                bool result = true;

                bool porductIsUncompleted = this.uncompletedProducts.CheckFinishedProduct(enProduct);
                bool nextLineNeeded = this.checkProductForNextLine(enProduct);

                string productName = enProduct != null ? enProduct.Name : "product is null";
               
                this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "uploadProductFromLine()",
                    productName + ".productIsUncompleted == " + porductIsUncompleted.ToString(), "system");

                this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "uploadProductFromLine()",
                    productName + ".nextLineNeeded == " + nextLineNeeded.ToString(), "system");

                this.myLog.LogAlert(AlertType.System, id.ToString(), this.GetType().ToString(), "uploadProductFromLine()",
                    productName + ", NextLineId = " + enProduct.Router.NextLineId.ToString() + ", NextLineAuto = " + enProduct.Router.NextLineAuto.ToString(), "system");

                if (!porductIsUncompleted & !nextLineNeeded)
                    productStock.Enqueue(enProduct);

                // remove enProduct from productsOnLine
                //-----------------------------------------------------------------
                if (productsOnLine.Peek() == enProduct)
                    productsOnLine.Dequeue();
                else
                {
                    int queueCount = productsOnLine.Count;
                    for (int i = 0; i < queueCount; i++)
                    {
                        Product tmpProd = productsOnLine.Dequeue();
                        if (tmpProd != enProduct)
                            productsOnLine.Enqueue(tmpProd);
                    }
                }

                // remove Batch if it is last Product ("tail")
                Batch batch = enProduct.Owner;
                batch.RemoveProduct(enProduct);

                // remove batch from line if it was the last product in the batch
                if (batch.IncompleteProducts == 0 && this.batchesOnLine.Peek() == batch)
                {
                    this.batchesOnLine.Dequeue();
                }



                // TODO: calculate "PLAN" gap
                // this.dayGap = this.calcPlanGap(enProduct, PlanMode.Day);
                // this.monthGap = this.calcPlanGap(enProduct, PlanMode.Month);

                // TODO: fillLogisticTailTable
                // this.fillLogisticTailTable(currentStation, enProduct, StationProductAction.Remove);

                return result;
            }
            catch (Exception ex) 
            {
                string productName = enProduct != null ? enProduct.Name : "product is null";

                this.myLog.LogAlert(AlertType.Error, id.ToString(), this.GetType().ToString(), ex.TargetSite.ToString(),
                    productName + ": " + ex.Message, "system");

                this.myLog.LogAlert(AlertType.Error, id.ToString(), this.GetType().ToString(), ex.TargetSite.ToString(),
                    productName + ": " + ex.StackTrace.ToString(), "system");

                return false;
            }
        }

        /// <summary>
        /// If Product has NextLineId in BatchMap        
        /// then write new row into ProductBuffer for that Line
        /// </summary>
        /// <param name="enProduct"></param>
        /// <returns></returns>
        private bool checkProductForNextLine(Product enProduct)
        {
            bool result = false;

            try
            {

                if (enProduct.Router.NextLineId > 0 && enProduct.Router.NextLineAuto == 1)
                {
                    this.productOutBufferTableAdapter.Insert(
                       0,
                       enProduct.Id,
                       enProduct.Owner.Id,
                       enProduct.Owner.TypeId,
                       enProduct.Name,
                       this.detroitDataSet.LineId,
                       enProduct.Router.NextLineId,
                       "in buffer",
                       0,
                       enProduct.Owner.Name,
                       DateTime.Now
                    );
                    result = true;
                }
            }

            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }

            return result;

        }
        
        /// <summary>
        /// Load products from line's inbox from database          
        /// </summary>
        /// <param name="enProduct"></param>
        /// <returns></returns>
        private void fillProductBufferFromInbox()
        {
            /* 
             * 1. read filtered rows from ProductBufferTable
             * 2. in routine create batch for product if necessary and put it into BatchesOnLine
             * 3. create product, link it to batch and put into this.productBuffer queue 
             * 4. delete row from ProductBufferTable
             * 
             *    enjoy!
             */

            List<DataRow> rowsToRemove = new List<DataRow>();

            try
            {

                this.productInboxBufferTableAdapter.FillByLineId(this.detroitDataSet.ProductBuffer, this.detroitDataSet.LineId);

                int i = 0;
                while (i < this.detroitDataSet.ProductBuffer.Rows.Count)
                {
                    DataRow queueRow = this.detroitDataSet.ProductBuffer.Rows[i];

                    //----------------------------------------------------------------
                    // 2. create batch for product if necessary and put it into BatchesOnLine.
                    // Check if there is already batch with ID in "BatchesOnLine"
                    //----------------------------------------------------------------
                    int batchId = Convert.ToInt32(queueRow["BatchId"]);
                    Batch enBatch = this.batchesOnLine.FirstOrDefault(p => p.Id.Equals(batchId));
                    if (enBatch != null)
                    {    // batch already exists in "BatchesOnLine"       
                    }
                    else // create new batch
                    {
                        enBatch = new Batch(
                            null, queueRow["Nummer"].ToString(), queueRow["BatchType_Name"].ToString()
                            , (int)queueRow["Capacity"], (int)queueRow["BatchId"], (int)queueRow["BatchTypeId"], 0
                            , (int)queueRow["Takt"]
                        );
                        this.batchesOnLine.Enqueue(enBatch);
                        enBatch.PutOnLine();
                    }

                    //-------------------------------------------------------------------------------
                    // 3. create product, link it to batch and put into this.productBuffer queue 
                    //-------------------------------------------------------------------------------
                    int productId = Convert.ToInt32(queueRow["ProductNummer"]);
                    Product newProduct = new Product(enBatch, productId, this.myLog, this.detroitDataSet);
                    enBatch.AddProduct(newProduct);
                    this.productBuffer.Enqueue(newProduct);


                    // put product in table in Detroit LineSnapshot for plan calculation
                    //this.lineSnapshotTable.AddLineSnapshotRow(
                    //    -1,
                    //    enBatch.Id,
                    //    newProduct.Id,
                    //    (int)ProductState.InBuffer,
                    //    this.detroitDataSet.LineId,
                    //    newProduct.Name,
                    //    123456, //TODO: this.taktDuration - this.lineCounter,
                    //    DateTime.Now,
                    //    newProduct.Router.State
                    //);

                    //-------------------------------------------------------------------------------
                    // 4. delete row from ProductBufferTable
                    //-------------------------------------------------------------------------------
                    if (queueRow.RowState != DataRowState.Deleted)
                    {
                        queueRow.Delete();
                    }
                    i++;
                }


                this.productInboxBufferTableAdapter.Update(this.detroitDataSet.ProductBuffer);
                //x this.detroitDataSet.ProductBuffer.AcceptChanges();
            }
            catch (Exception ex)
            {
                this.myLog.LogAlert(AlertType.Error, this.detroitDataSet.LineId.ToString(), ex.TargetSite.ToString(),
                                                    ex.Source.ToString(), ex.Message.ToString(), "system");
            }
        }
    }
}
