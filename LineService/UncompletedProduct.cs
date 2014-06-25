using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LineService
{
    class UncompletedProducts : Queue<Product>
    {
        private DetroitDataSet detroitDataSet;
        private DetroitDataSetTableAdapters.UncompletedProductTableAdapter uncompletedProductTableAdapter;

        public UncompletedProducts(DetroitDataSet detroit) 
        {
            this.detroitDataSet = detroit;
            this.uncompletedProductTableAdapter = new DetroitDataSetTableAdapters.UncompletedProductTableAdapter();
        } 

        // Check if the product needed to be placed in UncompletedTable
        // 1. Remove product from UncompletedProducts Queue
        // 2. Write into Detroit
        //
        public bool CheckFinishedProduct(Product enProduct) 
        {
            bool result = false;
            if (this.Contains(enProduct)) 
            {
                result = true;
                this.removeProductFromQueue(this, enProduct);


                int? nextLineId_value = null;
                if (enProduct.Router.NextLineId != 0)
                {
                    nextLineId_value = enProduct.Router.NextLineId;
                }

                try
                {
                    this.uncompletedProductTableAdapter.Insert(
                        enProduct.Id,
                        enProduct.Owner.Id,
                        enProduct.Owner.TypeId,
                        enProduct.Owner.Name,
                        enProduct.Name,
                        this.detroitDataSet.LineId,
                        nextLineId_value,
                        "failed",
                        0,
                        getProductFailedStations(enProduct),
                        DateTime.Now
                   );
                }
                catch (Exception ex) 
                {
                    string exx = ex.Message;
                }

            }
            return result;
        }


        // Utilities
        //
        //-----------------------------------------------
        private void removeProductFromQueue(Queue<Product> enQueue, Product enProduct)
        {
            int steps = enQueue.Count;
            for (int i = 0; i < steps; i++)
            {
                Product curProduct = (Product)enQueue.Dequeue();
                if (curProduct != enProduct)
                {
                    enQueue.Enqueue(curProduct);
                }
            }
        }
        private string getProductFailedStations(Product enProduct) 
        {
            string result = "";
            IEnumerable<string> stationNames = enProduct.FailedStations;
            result = stationNames.Aggregate((workingSentence, next) => workingSentence + ", " + next);
            
            return result;
        }
    }
}
