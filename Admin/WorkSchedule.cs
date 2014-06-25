using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using Admin.DetroitDataSetTableAdapters;
//using LineService;
using PlannerLib;
using Security;

namespace Admin
{

    public partial class WorkSchedule : Form
    {
        private DetroitDataSet.WorkScheduleDataTable table;
        private DetroitDataSet.ProductDataTable productPlanTable;

        private int lineId;

        public WorkSchedule()
        {
            InitializeComponent();
            fsModule.CheckFormAccess(this.Name);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRecalc_Click(object sender, EventArgs e)
        {
            // recalculte work schedule from LineQueue and SchedulerFrames tables from Detroit
            this.recalcWorkSchedule(DateTime.Now, this.table);
            int sortColumnIdx = 3;
            this.renumerateGridRows(this.dgrWorkSched, sortColumnIdx);
        }

        private void recalcWorkSchedule(DateTime start_time, DetroitDataSet.WorkScheduleDataTable table)
        {
            table.Clear();

            DetroitDataSetTableAdapters.LineQueueTableAdapter lineQueueAdapter = new DetroitDataSetTableAdapters.LineQueueTableAdapter();
            DetroitDataSetTableAdapters.SchedulerFrameTableAdapter framesAdapter = new DetroitDataSetTableAdapters.SchedulerFrameTableAdapter();

            DetroitDataSet.LineQueueDataTable lineQueueTable = new DetroitDataSet.LineQueueDataTable();
            DetroitDataSet.SchedulerFrameDataTable framesTable = new DetroitDataSet.SchedulerFrameDataTable();
            lineQueueAdapter.FillBy(lineQueueTable, this.lineId);
            framesAdapter.FillByFutureFrames(framesTable, start_time);

            const int NON_WORKING_FRAME = 0;
            const int WORKING_FRAME = 1;

            DataRow frame = null;

            int current_batch = 0; // bath rows counter
            if (lineQueueTable.Rows.Count > 0)
            {
                DataRow batchInQueue = lineQueueTable.Rows[current_batch];
                int lineId = Convert.ToInt32(batchInQueue["LineId"]);

                //-------------------------------------------------------------------------------
                // Update start time for batches where it's empty
                //-------------------------------------------------------------------------------

                // get "BusyTill" time when the line will become free
                DateTime lineBusyTill = Convert.ToDateTime(lineQueueAdapter.GetLineBusyTill(lineId));
                DateTime prev_batchFinishTime = lineBusyTill;

                // set batches times from "BusyTill" and further
                for (int i = 0; i < lineQueueTable.Rows.Count; i++)
                {
                    batchInQueue = lineQueueTable.Rows[i];

                    if (batchInQueue["StartTime"].ToString() == ""
                        || (Convert.ToDateTime(batchInQueue["StartTime"])).CompareTo(prev_batchFinishTime) < 0)
                    {
                        batchInQueue["StartTime"] = prev_batchFinishTime;
                        batchInQueue["FinishTime"] = DBNull.Value;
                    }

                    if (batchInQueue["FinishTime"].ToString() == "")
                    {
                        batchInQueue["FinishTime"] = ((DateTime)batchInQueue["StartTime"]).AddMinutes(
                             Convert.ToInt32(batchInQueue["Length"])
                        );
                    }
                    prev_batchFinishTime = ((DateTime)batchInQueue["FinishTime"]);
                }

                //-------------------------------------------------------------------------------
                // Create table WorkScheduleDataTable
                //-------------------------------------------------------------------------------

                // initialize "batchFinishTime" 
                batchInQueue = lineQueueTable.Rows[current_batch];
                DateTime batchStartTime = Convert.ToDateTime(batchInQueue["StartTime"]);
                DateTime batchFinishTime = batchStartTime.AddMinutes(Convert.ToInt32(batchInQueue["Length"]));

                for (
                    int current_frame = 0;
                    (current_frame < framesTable.Rows.Count) & (current_batch < (lineQueueTable.Rows.Count));
                    current_frame++
                    )
                {
                    frame = framesTable.Rows[current_frame];
                    if (Convert.ToInt32(frame["Type"]) == WORKING_FRAME)
                    {
                        if (Convert.ToDateTime(frame["Start"]).CompareTo(batchStartTime) >= 0)
                        {
                            // shift batch
                            long shift = Convert.ToDateTime(frame["Start"]).Ticks - batchStartTime.Ticks;
                            batchStartTime = Convert.ToDateTime(frame["Start"]);
                            batchFinishTime = batchFinishTime.AddTicks(shift);
                            // save real batch start time 
                            // check if its full batch or a part of it
                            DateTime fullLengthFinishTime = batchStartTime.AddMinutes(Convert.ToInt32(batchInQueue["Length"]));
                            if (batchFinishTime.CompareTo(fullLengthFinishTime) == 0)
                            {
                                batchInQueue["StartTime"] = batchStartTime;
                            }

                            string ws_name = "Batch No." + batchInQueue["Nummer"].ToString();
                            int ws_BatchId = Convert.ToInt32(batchInQueue["BatchId"]);
                            DateTime ws_start = batchStartTime;
                            DateTime ws_finish = new DateTime(0);


                            if (Convert.ToDateTime(frame["Finish"]).CompareTo(batchFinishTime) >= 0)
                            {
                                // batch finish time is = real. It the last part of the batch.
                                ws_finish = batchFinishTime;
                                // save real batch finish time 
                                batchInQueue["FinishTime"] = ws_finish;

                                frame["Start"] = batchFinishTime;
                                current_frame--;
                                current_batch++;
                                if (current_batch < (lineQueueTable.Rows.Count))
                                {
                                    batchInQueue = lineQueueTable.Rows[current_batch];
                                    batchStartTime = Convert.ToDateTime(batchInQueue["StartTime"]);
                                    batchFinishTime = batchStartTime.AddMinutes(
                                        Convert.ToInt32(batchInQueue["Length"])
                                    );
                                }
                            }
                            else
                            {
                                ws_finish = Convert.ToDateTime(frame["Finish"]);
                                batchStartTime = ws_finish;
                                // frame dequeue automaticaly
                                // batchInQueue["StartTime"] = ws_finish;
                            }
                           
                            // add batch
                            table.AddWorkScheduleRow(ws_name, ws_start, ws_finish, ws_BatchId);

                        }
                        else
                        {
                            DateTime ws_start = Convert.ToDateTime(frame["Start"]);
                            string ws_name = frame["Name"].ToString() + " // свободно";
                            int ws_BatchId = 0;
                            DateTime ws_finish = new DateTime(0);

                            if (Convert.ToDateTime(frame["Finish"]).CompareTo(batchStartTime) > 0)
                            {
                                ws_finish = batchStartTime;
                                frame["Start"] = ws_finish;
                                current_frame--;
                            }
                            else
                            {
                                ws_finish = Convert.ToDateTime(frame["Finish"]);
                                // frame dequeue automaticaly
                            }

                            // add work frame "свободно"
                            table.AddWorkScheduleRow(ws_name, ws_start, ws_finish, ws_BatchId);
                        }
                    }
                    else
                    {
                        // next row
                    }
                }

                // fill NON-WORKING FRAMES into table
                for (int i = 0; i < framesTable.Rows.Count; i++)
                {
                    frame = framesTable.Rows[i];
                    int ws_BatchId = 0;
                    if (Convert.ToInt32(frame["Type"]) == NON_WORKING_FRAME & Convert.ToDateTime(frame["Finish"]) < batchFinishTime)
                    {
                        table.AddWorkScheduleRow(
                            frame["Name"].ToString(),
                            Convert.ToDateTime(frame["Start"]),
                            Convert.ToDateTime(frame["Finish"]),
                            ws_BatchId
                        );
                    }
                }

                // save start and finish times for LineQueue
                lineQueueAdapter.Update(lineQueueTable);
            }
        }

        private void renumerateGridRows(DataGridView dgrView, int colIndex) 
        { 
            //DataRow[] rows = dgrView.Rows;

            dgrView.Sort(dgrView.Columns[colIndex-1], ListSortDirection.Ascending);
            for (int i = 0; i < dgrView.Rows.Count; i++) 
            {
                dgrView.Rows[i].Cells["Id"].Value = (i + 1).ToString();    
            }
        }

        private void WorkSchedule_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'detroitDataSet.Product' table. You can move, or remove it, as needed.
            this.productTableAdapter.Fill(this.detroitDataSet.Product);
            // TODO: This line of code loads data into the 'detroitDataSet.AssembLine' table. You can move, or remove it, as needed.
            this.assembLineTableAdapter.Fill(this.detroitDataSet.AssembLine);

            //
            this.table = new DetroitDataSet.WorkScheduleDataTable();
            this.table.DefaultView.Sort = "StartTime ASC";
            //this.table.DefaultView.Sort = "StartTime ASC";
            this.productPlanTable = new DetroitDataSet.ProductDataTable();
            this.dgrWorkSched.DataSource = this.productPlanTable;

            //this.btnRecalc_Click(sender, e);
            this.lineId = Convert.ToInt32(this.cbLine.SelectedValue);


            if (fsModule.AccessMode == FormAssessMode.Write)
            {
                this.setFormWriteMode();
            }
            else if (fsModule.AccessMode == FormAssessMode.Read)
            {
                this.setFormReadMode();
            }

        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            WorkScheduleRepForm aWorkSchedReport = new WorkScheduleRepForm();
            aWorkSchedReport.MdiParent = this.Owner;

            //aWorkSchedReport.repWS.DataBindings.

            aWorkSchedReport.WorkScheduleBindingSource.DataSource = this.table;
            aWorkSchedReport.Show();            

        }

        private void cbLine_SelectedValueChanged(object sender, EventArgs e)
        {
            this.lineId = Convert.ToInt32(this.cbLine.SelectedValue);
        }

        private void button1_Click(object sender, EventArgs e)
        {
                        
            string planKey = "line " + this.lineId.ToString() + " day " + DateTime.Today.ToShortDateString().ToString();
            this.productTableAdapter.DeleteByKey(planKey);

            this.recalcProductPlan2(PlanMode.Day);
            this.productTableAdapter.FillByKey(this.productPlanTable, planKey);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime keyDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            string planKey = "line " + this.lineId.ToString() + " mon " + keyDate.ToShortDateString().ToString();
            this.productTableAdapter.DeleteByKey(planKey);

            this.recalcProductPlan2(PlanMode.Month);            
            this.productTableAdapter.FillByKey(this.productPlanTable, planKey);
        }

        private void recalcProductPlan2(PlanMode planMode) 
        {
            Planner planner = new Planner(this.lineId, Properties.Settings.Default.ConnectionString);
            planner.RunSimulation();
            planner.MakeProductPlanInTakts(planMode);
        }



        private FormSecurityModule fsModule = new FormSecurityModule();

        private void setFormReadMode()
        {
            this.dgrWorkSched.ReadOnly = true;
            this.btnOk.Enabled = false;
            this.btnRecalc.Enabled = false;
            this.button1.Enabled = false;
            this.button2.Enabled = false;
        }
        private void setFormWriteMode() { }


    }

    public class ProductFrame 
    {
        public int Frame = 0;
        public int Number = 0;
        
        public ProductFrame() { }

        public ProductFrame(int frame, int number) 
        {
            this.Frame = frame;
            this.Number = number;
        }
    }

}




//       private void recalcProductPlan(DetroitDataSet.WorkScheduleDataTable table, PlanMode planMode) 
//       {
//            string planKey = "NA";
//            int daysInPlan = 0;
//            if (planMode == PlanMode.Day)
//            {
//                planKey = "day " + DateTime.Today.AddDays(1).ToShortDateString().ToString();
//                daysInPlan = 2;
//            } 
//            else 
//            {
//                DateTime keyDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
//                planKey = "mon " + keyDate.ToShortDateString().ToString();
//                daysInPlan = 32;
//            }

//            DetroitDataSetTableAdapters.ProductTableAdapter productPlanAdapter = new DetroitDataSetTableAdapters.ProductTableAdapter();
//            DetroitDataSet.ProductDataTable productPlanTable = new DetroitDataSet.ProductDataTable();
//            productPlanAdapter.DeleteByKey(planKey);
//            productPlanAdapter.FillByKey(productPlanTable, planKey);

//            DetroitDataSetTableAdapters.BatchLinesTableAdapter batchAdapter = new DetroitDataSetTableAdapters.BatchLinesTableAdapter();
//            DetroitDataSet.BatchLinesDataTable batchTable = new DetroitDataSet.BatchLinesDataTable();
//            batchAdapter.Fill(batchTable);   

//            Queue<ProductFrame> quProducts = new Queue<ProductFrame>();

//            for (int i = 0; i < table.Rows.Count; i++) 
//            {
//                DataRow wsRow = table.Rows[i];
//                if ((wsRow != null) 
//                    && (Convert.ToInt32(wsRow["BatchId"]) > 0)
//                    && ((Convert.ToDateTime(wsRow["StartTime"]).CompareTo(DateTime.Today.AddDays(daysInPlan + 1))) < 0) // check only two days
//                    ) 
//                { 
//                    // get batch
//                    string batchType = "NA";
//                    string batchNum = "0";
//                    int batchTypeId = 0;
//                    string select = "Batch_Id = " + wsRow["BatchId"].ToString();
//                    DataRow[] batchRows = batchTable.Select(select);
//                    if ((batchRows != null && batchRows[0] != null)) 
//                    {
//                        batchType = batchRows[0]["BatchType_Name"].ToString();
//                        batchNum = batchRows[0]["Batch_Nummer"].ToString();
//                        batchTypeId = Convert.ToInt32(batchRows[0]["Batch_BatchTypeId"]);
//                    }

//                    int batchCapacity = (int)batchAdapter.GetBatchType_Capacity(batchTypeId);

//                    DateTime batchFrameFinishTime = Convert.ToDateTime(wsRow["FinishTime"]);
//                    DateTime batchFrameStartTime = Convert.ToDateTime(wsRow["StartTime"]);
//                    DateTime productStartTime = batchFrameStartTime;
//                    int freeSpace = Convert.ToInt32((batchFrameFinishTime.Ticks - batchFrameStartTime.Ticks) * 1E-7 / 60);
////!!!!!!!
//                    int lineTakt = 40; // minutes, tbd
//                    int productFrame = lineTakt;

//                    // fill products queue
//                    if (quProducts.Count == 0) 
//                    { 
//                        for (int productNumI = 1; productNumI <= batchCapacity; productNumI++) 
//                        {
//                            ProductFrame pFrame = new ProductFrame(productFrame, productNumI);
//                            quProducts.Enqueue(pFrame);
//                        }
//                    }

//                    // try to put products from queue to the table inside batch frame
//                    int productNum = 0;
//                    while(quProducts.Count > 0 && freeSpace > 0) 
//                    {
//                        ProductFrame pFrame = quProducts.Peek();
//                        productFrame = pFrame.Frame;
//                        productNum = pFrame.Number;    
//                        if (freeSpace - productFrame >= 0)
//                        {
//                            quProducts.Dequeue();
//                        }
//                        else
//                        {
//                            pFrame.Frame = productFrame - freeSpace;
//                            productFrame = freeSpace;
//                        }

//                        // add row (productNum, lineTakt)
//                        DateTime productFinishTime = productStartTime.AddMinutes(productFrame);
//                        productPlanTable.AddProductRow(
//                            Convert.ToInt32(wsRow["BatchId"]),
//                            productNum,
//                            "NA",
//                            productFinishTime,
//                            planKey,
//                            batchNum + "/" + productNum + "(" + batchType + ")",
//                            this.lineId,
//                            0
//                        );
//                        if (freeSpace >= productFrame) { freeSpace = freeSpace - productFrame; } else { freeSpace = 0; }
//                        productStartTime = productFinishTime;
//                    }
//                }
//            }
//            productPlanAdapter.Update(productPlanTable);
//        }

//private void recalcProductFinish(DetroitDataSet.WorkScheduleDataTable table, PlanMode planMode) 
//{
//    string planKey = "NA";
//    string lineKey = "line " + this.lineId.ToString();

//    int daysInPlan = 0;
//    if (planMode == PlanMode.Day)
//    {
//        planKey = "day " + DateTime.Today.AddDays(1).ToShortDateString().ToString();
//        daysInPlan = 2;
//    }
//    else
//    {
//        DateTime keyDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
//        planKey = "mon " + keyDate.ToShortDateString().ToString();
//        daysInPlan = 32;
//    }

//    DetroitDataSetTableAdapters.ProductTableAdapter productPlanAdapter = new DetroitDataSetTableAdapters.ProductTableAdapter();
//    DetroitDataSet.ProductDataTable productPlanTable = new DetroitDataSet.ProductDataTable();
//    productPlanAdapter.DeleteByKey(planKey);
//    productPlanAdapter.FillByKey(productPlanTable, planKey);

//    DetroitDataSetTableAdapters.ProductOnLineTableAdapter productOnLineAdapter = new DetroitDataSetTableAdapters.ProductOnLineTableAdapter();
//    DetroitDataSet.ProductOnLineDataTable productOnLineTable = new DetroitDataSet.ProductOnLineDataTable();
//    productOnLineAdapter.Fill(productOnLineTable, lineKey);

//    DetroitDataSetTableAdapters.BatchLinesTableAdapter batchAdapter = new DetroitDataSetTableAdapters.BatchLinesTableAdapter();
//    DetroitDataSet.BatchLinesDataTable batchTable = new DetroitDataSet.BatchLinesDataTable();
//    batchAdapter.Fill(batchTable);

//    int lineTakt = 0;
//    object aTakt = productPlanAdapter.GetLineTaktLength(this.lineId);
//    if (aTakt != null) 
//    {
//        lineTakt = Convert.ToInt32(aTakt); 
//    } 
//    else 
//    {
//        throw new Exception("Coudn't read Line length from Database!");
//    }


//    // 1.   Для каждого продукта можем вычислить длину Рабочего времени до окончания (p.w). 
//    //      p.w = LineTakt * (mapStepsCount – currentMapStep) + TaktTime
//    // 2.   Для каждого продукта просматриваем рабочие кадры (фреймы) в графике Рабочего времени, 
//    //      начиная с текущего момента и сравниваем длину текущего рабочего кадра (f.t) с длиной p.w. 
//    // 3.   Если f.t. >= p.w, то искомая величина окончания сборки продукта равна сумме времени начала 
//    //      рабочего кадра (f.ft) и длины p.w., а именно: p.ft = f.st + p.w 
//    // 4.   Если f.t < p.w., то длину времени до окончания сборки продукта p.w. уменьшаем на f.t. 
//    //      Далее выбираем для сравнения следующий рабочий кадр (f.t).


//    DateTime productFinishTime;

//    // 1.
//    for (int k = 0; k < productOnLineTable.Count; k++) 
//    {
//        productFinishTime = new DateTime(DateTime.Today.Year + 1, DateTime.Today.Month, DateTime.Today.Day); // default date for exception
//        DataRow productRow = productOnLineTable.Rows[k];
//        int p_w = Convert.ToInt32(productRow["WorkSecondsTillFinish"]);

//        // 2.
//        for (int i = 0; i < table.Rows.Count; i++)
//        {
//            DataRow wsRow = table.Rows[i];
//            if ((wsRow != null)
//                && (Convert.ToInt32(wsRow["BatchId"]) > 0)
//                && ((Convert.ToDateTime(wsRow["StartTime"]).CompareTo(DateTime.Today.AddDays(daysInPlan + 1))) < 0) // check only two days
//                )
//            {
//                ///
//                int f_t = (int)((Convert.ToDateTime(wsRow["FinishTime"]).Ticks - Convert.ToDateTime(wsRow["StartTime"]).Ticks) * 1E-7);

//                // 3.
//                if (f_t >= p_w)
//                {
//                    productFinishTime = Convert.ToDateTime(wsRow["StartTime"]).AddSeconds(p_w);
//                    // add row (productNum, lineTakt)
//                    productPlanTable.AddProductRow(
//                        Convert.ToInt32(wsRow["BatchId"]),
//                        Convert.ToInt32(productRow["Nummer"]),
//                        "NA",
//                        productFinishTime,
//                        planKey,
//                        productRow["ProductName"].ToString(),
//                        this.lineId,
//                        0
//                    );

//                    break;
//                }
//                // 4.
//                else
//                {
//                    p_w = p_w - f_t;
//                }
//            }
//        }



//    }

//}
