using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LineService
{

    public class ProductStock : Queue<Product>
    {
        private DetroitDataSet detroit;
        private DetroitDataSetTableAdapters.ProductStockTableAdapter productStockTableAdapter;
        
        public ProductStock() 
            :base() 
        { 
        }

        public ProductStock(DetroitDataSet enDetroit) : base()
        {
            this.detroit = enDetroit;
            this.productStockTableAdapter = new DetroitDataSetTableAdapters.ProductStockTableAdapter();
        }

        public new void Enqueue(Product enProduct) 
        {
            base.Enqueue(enProduct);

            if (this.detroit.LineId != 0 && this.detroit != null) 
            {
                this.productStockTableAdapter.Insert(
                    0,
                    enProduct.Id,
                    enProduct.Owner.Id,
                    enProduct.Owner.TypeId,
                    enProduct.Name,
                    this.detroit.LineId, 
                    "in stock",
                    0,
                    enProduct.Owner.Name

                    );
            }
        }

    }
}
