using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LineService
{
    public class ProductBuffer : Queue<Product>
    {
        public event EventHandler OnRemoveProduct;

        public ProductBuffer() : base()
        {
            //...
            // connect to database
            // read Products from ... in routine
            // add every Product

        }

        new public Product Dequeue() 
        {
            Product result = base.Dequeue();
            if (this.OnRemoveProduct != null)
                this.OnRemoveProduct(this, new EventArgs());
            return result;
        }
    }
}
