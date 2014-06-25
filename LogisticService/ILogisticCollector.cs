using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace LogisticService
{
    [ServiceContract]
    interface ILogisticCollector
    {

        [OperationContract]
        void RenewBatchesOnLine(LogistBatch[] lineBatches);

        [OperationContract]
        void RenewNextBatch(LogisticInfo info);

    }




    



}
