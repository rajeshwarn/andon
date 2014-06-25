using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Security;

namespace Admin
{
    public partial class LineQueueForm : Form
    {
        public LineQueueForm()
        {
            InitializeComponent();
            fsModule.CheckFormAccess(this.Name);
        }

        public LineQueueForm(string databaseConnectionString)
        {
            Properties.Settings.ChangeConnectionString(databaseConnectionString);
            InitializeComponent();
            fsModule.CheckFormAccess(this.Name);
        }

        private void LineQueueForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'detroitDataSet.BatchLines' table. You can move, or remove it, as needed.
            this.batchLinesTableAdapter.Fill(this.detroitDataSet.BatchLines);
            this.batchTableAdapter.Fill(this.detroitDataSet.Batch);
            ////this.batchLinesTableAdapter.Fill(this.detroitDataSet.BatchLines);
            this.lineQueueTableAdapter.Fill(this.detroitDataSet.LineQueue);
            this.assembLineTableAdapter.Fill(this.detroitDataSet.AssembLine);

            DataGridViewColumn sortedColumn = this.dataGridView1.Columns[0];
            this.dataGridView1.Sort(sortedColumn , ListSortDirection.Ascending);


            int rowsCount = this.dataGridView2.Rows.Count;
            if (rowsCount > 0)
            {
                this.dataGridView2.CurrentCell = this.dataGridView2.Rows[rowsCount - 1].Cells[0];
            }


            if (fsModule.AccessMode == FormAssessMode.Write)
            {
                this.setFormWriteMode();
            }
            else if (fsModule.AccessMode == FormAssessMode.Read)
            {
                this.setFormReadMode();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //???? It should not be any update for "BatchLine"

            this.batchLinesTableAdapter.Update(this.detroitDataSet.BatchLines);
            this.lineQueueTableAdapter.Update(this.detroitDataSet.LineQueue);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            btnSave_Click(sender, e);
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
 
            //-----------------------------------------------------------------------------------
            //*** Get current batch from "BatchLines" and move it into "LineQueue"
            //-----------------------------------------------------------------------------------
            if (dataGridView2.CurrentCell != null)
            {
                //------------------------------------------------------------------------
                //*** Calculate takt lenght in minutes
                //------------------------------------------------------------------------
                DataRowView curLine = (DataRowView)this.assembLineBindingSource.Current;
                int taktLength = Convert.ToInt32(curLine.Row["Takt"]);

                //------------------------------------------------------------------------
                //*** Calculate next ordinal number, start time, takt lenght etc. in the queue
                //------------------------------------------------------------------------
                DataRowView curBatchViewData = (DataRowView)this.assembLineBatchLinesBindingSource.Current;
                fKLineQueueAssembLineBindingSource.MoveLast();
                DataRowView lastQueueRow = (DataRowView)this.fKLineQueueAssembLineBindingSource.Current;

                int lastNum = 0;
                string startTime = "";
                if (lastQueueRow != null) {
                    lastNum = Convert.ToInt32(lastQueueRow.Row["OrdinalNum"]);
                    startTime = ""; // null means start the batch ASAP
                }
                string newState = "In Queue";

                // get batch capacity from Batch!
                int batchCapacity = Convert.ToInt32(curBatchViewData.Row["Batch_Capacity"]);


                //------------------------------------------------------------------------
                //*** Add new row in the queue
                //------------------------------------------------------------------------
                fKLineQueueAssembLineBindingSource.AddNew();
                DataRowView newQueueRow = (DataRowView)this.fKLineQueueAssembLineBindingSource.Current;
                newQueueRow.Row["BatchId"] = Convert.ToInt32(curBatchViewData.Row["Batch_Id"].ToString());
                newQueueRow.Row["LineId"] = Convert.ToInt32(curBatchViewData.Row["Map_LineId"].ToString());
                newQueueRow.Row["Status"] = newState;
                newQueueRow.Row["OrdinalNum"] = lastNum + 1;
                if (startTime == "") 
                {
                    newQueueRow.Row["StartTime"] = DBNull.Value;
                } 
                else 
                {
                    newQueueRow.Row["StartTime"] = Convert.ToDateTime(startTime);
                }
                newQueueRow.Row["Length"] = taktLength * batchCapacity;
                newQueueRow.EndEdit();

                //------------------------------------------------------------------------
                //*** Update row in the batch list view
                //------------------------------------------------------------------------
                curBatchViewData.Row["Batch_State"] = "In Queue";
                DetroitDataSet.BatchRow curBatchTableData = this.detroitDataSet.Batch.FindById(Convert.ToInt32(curBatchViewData.Row["Batch_Id"].ToString()));
                curBatchTableData["State"] = curBatchViewData.Row["Batch_State"].ToString();

            } else {
                MessageBox.Show("Please select new batch at first.");
            }
   
            //-----------------------------------------------------------------------------------
            // end of block
            //-----------------------------------------------------------------------------------
        }

        private void button2_Click(object sender, EventArgs e)
        {

            //------------------------------------------------------------------------
            //*** Remove current batch from Queue and from lower grid
            //------------------------------------------------------------------------
            DataRowView curQViewData = (DataRowView)this.fKLineQueueAssembLineBindingSource.Current;
            if (curQViewData != null)
            {
                int batchId = Convert.ToInt32(curQViewData.Row["BatchId"]);
                int lineId = Convert.ToInt32(curQViewData.Row["LineId"]);
                int deletedOrdinalNumber = Convert.ToInt32(curQViewData.Row["OrdinalNum"]);
                curQViewData.Delete();

                DataRowView curLine = (DataRowView)this.assembLineBindingSource.Current;
                int taktLength = Convert.ToInt32(curLine.Row["Takt"]);

                //------------------------------------------------------------------------
                //*** Move forward all "later" batches in the queue => change ordinal number and start time
                //------------------------------------------------------------------------
                string filterStrLQ = "LineId = " + lineId.ToString() + " AND OrdinalNum > " + deletedOrdinalNumber.ToString();
                DataRow[] queueItems = this.detroitDataSet.LineQueue.Select(filterStrLQ);
                for (int i = 0; i < queueItems.Count(); i++)
                {
                    queueItems[i]["OrdinalNum"] = Convert.ToInt32(queueItems[i]["OrdinalNum"]) - 1;
                    //queueItems[i]["StartTime"] = Convert.ToDateTime(queueItems[i]["StartTime"]).AddMinutes(-taktLength);
                }

                //------------------------------------------------------------------------
                //*** Set state "New" for batch and show it automaticaly in the upper grid
                //------------------------------------------------------------------------
                string filterStr = "Batch_Id = " + batchId.ToString() + " and Map_LineId = " + lineId.ToString();
                DataRow[] batches = this.detroitDataSet.BatchLines.Select(filterStr);
                batches[0]["Batch_State"] = "New";
            }
           
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            DataRowView curQViewData = (DataRowView)this.fKLineQueueAssembLineBindingSource.Current;
            if (curQViewData != null)
            {
                //
                this.fKLineQueueAssembLineBindingSource.MovePrevious();
                DataRowView prevQViewData = (DataRowView)this.fKLineQueueAssembLineBindingSource.Current;
                if (prevQViewData != curQViewData) 
                { 
                    //
                    int curNum = (int)curQViewData.Row["OrdinalNum"];
                    int prevNum = (int)prevQViewData.Row["OrdinalNum"];

                    curQViewData.Row["OrdinalNum"] = prevNum;
                    prevQViewData.Row["OrdinalNum"] = curNum;

                    this.fKLineQueueAssembLineBindingSource.MovePrevious();    
                }
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            DataRowView curQViewData = (DataRowView)this.fKLineQueueAssembLineBindingSource.Current;
            if (curQViewData != null)
            {
                //
                this.fKLineQueueAssembLineBindingSource.MoveNext();
                DataRowView prevQViewData = (DataRowView)this.fKLineQueueAssembLineBindingSource.Current;
                if (prevQViewData != curQViewData)
                {
                    //
                    int curNum = (int)curQViewData.Row["OrdinalNum"];
                    int prevNum = (int)prevQViewData.Row["OrdinalNum"];

                    curQViewData.Row["OrdinalNum"] = prevNum;
                    prevQViewData.Row["OrdinalNum"] = curNum;

                    this.fKLineQueueAssembLineBindingSource.MoveNext();
                }
            }

        }



        private FormSecurityModule fsModule = new FormSecurityModule();

        private void setFormReadMode()
        {
            this.dataGridView1.ReadOnly = true;
            this.dataGridView2.ReadOnly = true;
            //this.dataGridView3.ReadOnly = true;

            this.button1.Enabled = false;
            this.button2.Enabled = false;
            this.btnUp.Enabled = false;
            this.btnDown.Enabled = false;

            this.btnOk.Enabled = false;
            this.btnSave.Enabled = false;
        }
        private void setFormWriteMode() { }

        private void LineQueueForm_Shown(object sender, EventArgs e)
        {
            //this.dataGridView2.Sort(this.dataGridView2.Columns[0], ListSortDirection.Descending);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //save before refresh!
            this.btnSave_Click(sender, e);

            //reload data from database
            this.batchLinesTableAdapter.Fill(this.detroitDataSet.BatchLines);

        }

        private void dataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //MessageBox.Show("DataBindingComplete");
            int rowsCount = this.dataGridView2.Rows.Count;
            if (rowsCount > 0)
            {
                this.dataGridView2.CurrentCell = this.dataGridView2.Rows[rowsCount - 1].Cells[0];
            }
        }











    }
}
