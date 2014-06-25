using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppLog;

namespace LineService
{
    public class Product : ProductBase
    {
        private ProductMap map;
        public ProductMap Map; // { get { return this.map;  } }
        private bool operationFinished = false;
        private Batch batch;
        private List<string> failedStations;

        public IEnumerable<string> FailedStations { get { return this.failedStations; } }


        public Product(Batch batch, int id, LogProvider logProvider, DetroitDataSet detroitDataSet)
        {
            this.router = new DbProductRouter(1, detroitDataSet, batch.TypeId);
            this.batch = batch;
            this.id = id;
            this.name = batch.Name + "/" + id.ToString();
            this.map = null; // new ProductMap(this, batch.Map, logProvider);
            this.Map = map;
            this.failedStations = new List<string>();
            batch.AddProduct(this);

        }

        public void OperationFailed(string stationName) 
        {
            // TODO : Product.OperationFailed() 
            ////this.map.Marker.Value.Fail();

            this.failedStations.Add(stationName);
        }
        public List<LineStation> GetFailedStations() 
        { 
            List<LineStation> result = new List<LineStation>();
            // TODO : GetFailedStations() 


            //MapItem[] mapItems = this.map.Where(p => p.Result.Equals(MapStepResult.Fail)).ToArray<MapItem>();
            //for (int i = 0; i < mapItems.Count(); i++)
            //{
            //    result.Add(mapItems[i].LineStation);
            //}
            return result;
        }

        public Batch Owner { get { return this.batch; } }

        public bool OperationFinished
        {
            get { return this.operationFinished; }
            set { this.operationFinished = value; }
        }

        public void ResetOperation()
        {
            this.operationFinished = false;
        }




        //=====================================================
        private DbProductRouter router;
        public DbProductRouter Router { get { return router; } }

        private string manager = "";

        public string Manager { get { return this.manager; }  }
        public bool SetManager(string manager) 
        {
            bool result = false;
            if (this.manager == "") 
            {
                this.manager = manager;
            }
            return result;
        }
        public void KillManager() 
        {
            this.manager = "";
        }
    }
}
