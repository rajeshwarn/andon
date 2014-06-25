using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LineService
{
    public interface IStation
    {
        int Id { get; set; }
        string Name { get; set; }
        Product CurrentProduct { get; }
        int BitState { get; set; }
        // bit 0:   can't go forward because next station is occupited
        // bit 1:   is being late
        // bit 2:   is free, but previous station still holding product

        //bool RedState { get; set; }

        bool AddProduct(Product product);
        void ReleaseProduct();
        void RollbackProductByName(string product);
        void Finish();
        void ResetFinish();
        bool IsFull { get; }
        bool IsFinished { get; }
        ProductState ProductPosition(Product product);
        event EventHandler<DispatcherStationArgs> OnFinished;
        event EventHandler<DispatcherStationArgs> OnFree;
        event EventHandler<DispatcherStationArgs> OnGetNewProduct;
    }
}
