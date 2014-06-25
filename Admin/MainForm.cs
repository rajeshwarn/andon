using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Security;
using AppLog;

namespace Admin
{
    public partial class MainForm : Form
    {
        private SecurityMgr securityMgr;
        private LogProvider logProvider;
        private LogProvider systemLog;

        public MainForm()
        {
            InitializeComponent();
            this.systemLog = new LogProvider(LogType.File, "admin_system_log.txt", true);
        }


        private void linesAndStationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                LinesForm aLinesForm = new LinesForm(this.logProvider);
                aLinesForm.MdiParent = this;
                aLinesForm.Show();
            }

            catch (System.UnauthorizedAccessException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    ex.Message
                    , "Action alert"
                    , System.Windows.Forms.MessageBoxButtons.OK
                    , System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void stationControlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                StationForm aStationForm = new StationForm();
                aStationForm.MdiParent = this;
                aStationForm.Show();
            }
            catch (System.UnauthorizedAccessException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    ex.Message
                    , "Action alert"
                    , System.Windows.Forms.MessageBoxButtons.OK
                    , System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(string.Format("{0} sent this event", sender.ToString()));
            Application.Exit();
        }


        private void batchTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                BatchTypeForm aBatchTypeForm = new BatchTypeForm();
                aBatchTypeForm.MdiParent = this;
                aBatchTypeForm.Show();
            }
            catch (System.UnauthorizedAccessException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    ex.Message
                    , "Action alert"
                    , System.Windows.Forms.MessageBoxButtons.OK
                    , System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        private void mapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                BatchTypeMap aBatchTypeMap = new BatchTypeMap();
                aBatchTypeMap.MdiParent = this;
                aBatchTypeMap.Show();
            }
            catch (System.UnauthorizedAccessException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    ex.Message
                    , "Action alert"
                    , System.Windows.Forms.MessageBoxButtons.OK
                    , System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                this.systemLog.LogAlert(AlertType.Error, ex.Message, User.Name);
            }
        }

        private void partsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Parts aParts = new Parts();
                aParts.MdiParent = this;
                aParts.Show();
            }
            catch (System.UnauthorizedAccessException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    ex.Message
                    , "Action alert"
                    , System.Windows.Forms.MessageBoxButtons.OK
                    , System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void batchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                BatchForm aBatchForm = new BatchForm();
                aBatchForm.MdiParent = this;
                aBatchForm.Show();
            }
            catch (System.UnauthorizedAccessException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    ex.Message
                    , "Action alert"
                    , System.Windows.Forms.MessageBoxButtons.OK
                    , System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void lineQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                LineQueueForm aLineQueueForm = new LineQueueForm();
                aLineQueueForm.MdiParent = this;
                aLineQueueForm.Show();
            }
            catch (System.UnauthorizedAccessException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    ex.Message
                    , "Action alert"
                    , System.Windows.Forms.MessageBoxButtons.OK
                    , System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void scheduleFramesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ScheduleFrames aScheduleFrameForm = new ScheduleFrames(this.systemLog);
                aScheduleFrameForm.MdiParent = this;
                aScheduleFrameForm.Show();
            }
            catch (System.UnauthorizedAccessException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    ex.Message
                    , "Action alert"
                    , System.Windows.Forms.MessageBoxButtons.OK
                    , System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void workScheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                WorkSchedule aWorkSchedForm = new WorkSchedule();
                aWorkSchedForm.MdiParent = this;
                aWorkSchedForm.Show();
            }
            catch (System.UnauthorizedAccessException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    ex.Message
                    , "Action alert"
                    , System.Windows.Forms.MessageBoxButtons.OK
                    , System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void uncompletedChassiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                UncompletedProducts aUncompletedProducts = new UncompletedProducts();
                aUncompletedProducts.MdiParent = this;
                aUncompletedProducts.Show();
            }
            catch (System.UnauthorizedAccessException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    ex.Message
                    , "Action alert"
                    , System.Windows.Forms.MessageBoxButtons.OK
                    , System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void chassisBufferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ProductBuffer aProductBuffer = new ProductBuffer();
                aProductBuffer.MdiParent = this;
                aProductBuffer.Show();
            }
            catch (System.UnauthorizedAccessException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    ex.Message
                    , "Action alert"
                    , System.Windows.Forms.MessageBoxButtons.OK
                    , System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void defaultSheduleFramesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ScheduleFramesDefault aScheduleFrameDefaultForm = new ScheduleFramesDefault();
                aScheduleFrameDefaultForm.MdiParent = this;
                aScheduleFrameDefaultForm.Show();
            }
            catch (System.UnauthorizedAccessException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    ex.Message
                    , "Action alert"
                    , System.Windows.Forms.MessageBoxButtons.OK
                    , System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void createNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.securityMgr.LanchNewUserDlg();
            }
            catch (System.UnauthorizedAccessException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    ex.Message
                    , "Action alert"
                    , System.Windows.Forms.MessageBoxButtons.OK
                    , System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void logonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.securityMgr.LanchLogonDlg())
                {
                    // get access
                }
                else
                {
                    // revoke access
                };
            }
            catch (System.UnauthorizedAccessException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    ex.Message
                    , "Action alert"
                    , System.Windows.Forms.MessageBoxButtons.OK
                    , System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void userPermissionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.securityMgr.LanchUserPermissionsDlg();
            }
            catch (System.UnauthorizedAccessException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    ex.Message
                    , "Action alert"
                    , System.Windows.Forms.MessageBoxButtons.OK
                    , System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.securityMgr = new SecurityMgr(this.systemLog);
            if (this.securityMgr.LanchLogonDlg())
            {
                // get access

                System.Data.SqlClient.SqlConnectionStringBuilder builder =
                    new System.Data.SqlClient.SqlConnectionStringBuilder(Properties.Settings.Default.DetroitConnectionString);
                builder.UserID = User.Name;
                builder.Password = User.Password;
                builder.IntegratedSecurity = false;

                Admin.Properties.Settings.ChangeConnectionString(builder.ConnectionString);

                this.systemLog.LogAlert(AlertType.Info, DateTime.Now.ToString() + " --- " + "Admin", "User = " + User.Name);
                this.systemLog.LogAlert(AlertType.Info, DateTime.Now.ToString() + " --- " + "Admin", "Password = " + User.Password);

                this.systemLog.LogAlert(AlertType.Info, DateTime.Now.ToString() + " --- " + "Admin", "builder.ConnectionString = " + builder.ConnectionString);
                this.systemLog.LogAlert(AlertType.Info, DateTime.Now.ToString() + " --- " + "Admin", "DetroitConnectionString = " + Properties.Settings.Default.DetroitConnectionString.ToString());

                this.logProvider = new LogProvider(LogType.SQL, Properties.Settings.Default.DetroitConnectionString.ToString(), false);
                this.logProvider.LogAlert(0, "NA", "Admin", this.Name, "User login succesfull.", User.Name);
            }
            else
            {
                // revoke access
                // this.logProvider.LogAlert(0, "", "Application", this.Name, "User login unsuccesfull.", User.Name);  
                this.Close();
            };
        }

        private void stockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Stock aStockForm = new Stock();
                aStockForm.MdiParent = this;
                aStockForm.Show();
            }
            catch (System.UnauthorizedAccessException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    ex.Message
                    , "Action alert"
                    , System.Windows.Forms.MessageBoxButtons.OK
                    , System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Log aForm = new Log();
                aForm.MdiParent = this;
                aForm.Show();
            }
            catch (System.UnauthorizedAccessException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    ex.Message
                    , "Action alert"
                    , System.Windows.Forms.MessageBoxButtons.OK
                    , System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.logProvider != null)
            {
                //this.logProvider.LogAlert(0, "NA", "Admin", this.Name, "User logged off.", User.Name);
            }
        }

        private void stationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Stations aStationsForm = new Stations();
                aStationsForm.MdiParent = this;
                aStationsForm.Show();
            }
            catch (System.UnauthorizedAccessException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    ex.Message
                    , "Action alert"
                    , System.Windows.Forms.MessageBoxButtons.OK
                    , System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void errorLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLog aErrorLogForm = new ErrorLog();
                aErrorLogForm.MdiParent = this;
                aErrorLogForm.Show();
            }
            catch (System.UnauthorizedAccessException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    ex.Message
                    , "Action alert"
                    , System.Windows.Forms.MessageBoxButtons.OK
                    , System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }




        //private void MainForm_Load(object sender, EventArgs e)
        //{
        //    List<TestData> myDataList = new List<TestData>();
        //    myDataList.Add(new TestData() { Id = 1, Message = "AAA" });
        //    myDataList.Add(new TestData() { Id = 2, Message = "BBB" });
        //}
    }

    //public class TestData 
    //{
    //    public int Id;
    //    public string Message;
    //}

}


