using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Admin
{

    public enum LineQueueStatus
    { InQueue = 1, Started = 2, OnLine = 3 };

    class LineQueueItem
    {
        private int Id;
        private int BatchId;
        private int Length;
        private int ordinalNummer;
        public int OrdinalNummer { get { return ordinalNummer; } set { ordinalNummer = value; } }
        private LineQueueStatus Status;
        private DateTime StartTime;


        LineQueueItem(int id, int batchId, LineQueueStatus status, DateTime startTime)
        {
            this.Id = id;
            this.BatchId = batchId;
            this.Status = status;
            this.StartTime = startTime;
        }
        LineQueueItem(int id, int batchId, LineQueueStatus status)
        {
            this.Id = id;
            this.BatchId = batchId;
            this.Status = status;
        }
        LineQueueItem(int batchId, LineQueueStatus status)
        {
            this.BatchId = batchId;
            this.Status = status;
        }
        LineQueueItem(int batchId, LineQueueStatus status, DateTime startTime, int length)
        {
            this.BatchId = batchId;
            this.Status = status;
            this.StartTime = startTime;
            this.Length = length;
        }

    }


    class LineQueue : LinkedList<LinkedListNode<LineQueueItem>>
    {

        public void RenumQueue() {

            LinkedListNode<LineQueueItem> anItem = this.First();
            if (anItem != null) {
                int firstNum = this.First().Value.OrdinalNummer;

                while (anItem.Next != null) {
                    anItem.Next.Value.OrdinalNummer = anItem.Value.OrdinalNummer + 1;
                    anItem = anItem.Next;
                }
            }
        }
    
    
    
    };


}