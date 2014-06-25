using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LineService
{
    class TestStation : IStation
    {
        private int id = 0;
        private string name = "";
        private Product product;
        private bool isFull = false;

        public int Id { get { return this.id; } set { this.id = value; } }
        public string Name { get { return this.name; } set { this.name = value; } }
        public Product CurrentProduct { get { return this.product; } }
        public Int32 BitState { get; set; }

        public bool AddProduct(Product product)
        {
            Console.WriteLine("Trying to add product on station " + Name);
            this.product = product;
            this.isFull = true;
            return true;
        }
        public void ReleaseProduct()
        {
            Console.WriteLine("Trying to release product from station " + Name);
            this.product = null;
            this.isFull = false;
        }
        public void RollbackProductByName(string product)
        {
            //
        }
        public void Finish()
        {
            // do nothing
        }
        public void ResetFinish() { }
        public bool IsFull { get { return this.isFull; } }
        public bool IsFinished { get { return false; } }

        public ProductState ProductPosition(Product product) { return ProductState.Undefined; }
        public event EventHandler<DispatcherStationArgs> OnFinished;
        public event EventHandler<DispatcherStationArgs> OnFree;
        public event EventHandler<DispatcherStationArgs> OnGetNewProduct;
    }
}
