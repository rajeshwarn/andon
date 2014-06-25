using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LineService
{
    [Serializable]
    [DataContract]
    public class Batch
    {
        private int id; //
        private int typeId;
        private string name;
        private string type;
        private int capacity;
        private int length;
        private int takt;
        private string status = "New";
        private DateTime status_timestamp;

        private Line line;
        
        [NonSerialized]
        private BatchMap map;
        
        [NonSerialized]
        private Queue<Product> incompleteProducts;

        public Batch() { }

        public Batch(Line line, string name, string type, int capacity, int batchId, int typeId, int length, int takt)
        {
            this.id = batchId;
            this.typeId = typeId;
            this.name = name;
            this.type = type;
            this.capacity = capacity;
            this.line = line;
            this.takt = takt;
            this.length = length; 

            this.map = null; //x new BatchMap(this, this.line);
            this.incompleteProducts = new Queue<Product>();
        }

        public int Id { get { return this.id; } }
        public int TypeId { get { return this.typeId; } }
        public string Name { get { return this.name; } }
        public string Type { get { return this.type; } }
        public int Capacity { get { return this.capacity; } }
        public int Length { get { return this.length; } }
        public int Takt { get { return this.takt; } }
        public BatchMap Map { get { return this.map; } }


        public string Status { get { return this.status; } }
        public void PutInQueue() 
        {
            this.status = "InQueue";
            this.status_timestamp = DateTime.Now;
        }
        public void PutOnLine() 
        {
            this.status = "OnLine";
            this.status_timestamp = DateTime.Now;
        }
        public int IncompleteProducts { get {return this.incompleteProducts.Count ;} }
        public void AddProduct(Product enProduct) 
        {
            if (!this.incompleteProducts.Contains(enProduct)) 
            {
                this.incompleteProducts.Enqueue(enProduct);
            }
            
        }
        public void RemoveProduct(Product enProduct) 
        {
            int i = 0;
            int length = this.incompleteProducts.Count;
            while (i < length) 
            {
                Product product = this.incompleteProducts.Dequeue();
                if (product != enProduct) 
                {
                    this.incompleteProducts.Enqueue(product);
                }
                i++;
            }
        }

    }
}
