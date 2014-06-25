namespace LineService {
   
 
   
    public partial class DetroitDataSet {

        private int lineId;
        public int LineId { get { return this.lineId; } }

        public DetroitDataSet(int lineId) : this() 
        {
            this.lineId = lineId;
        }

    }
}



namespace LineService.DetroitDataSetTableAdapters {
    partial class LineSnapshotReaderTableAdapter
    {
    }

    partial class ControlTableAdapter
    {
    }

    partial class ProductBufferTableAdapter
    {
    }

    partial class UncompletedProductTableAdapter
    {
    }

    partial class BatchTableAdapter
    {
    }

    partial class ProductTableAdapter
    {
    }

    partial class ProductOnLineTableAdapter
    {
    }

    partial class AssembLineTableAdapter
    {
    }

    partial class SchedulerFrameTableAdapter
    {
    }

    partial class BatchMapPointTableAdapter
    {
    }

    partial class LineQueueExtTableAdapter
    {
    }
    
    
    public partial class LineQueueTableAdapter {
    }
}
