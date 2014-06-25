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
using Admin;

namespace LineManagerApp
{
    public partial class MainForm : Form
    {
        private List<Form> childFormList = new List<Form>();
        private TestUtils testUtils1 = new TestUtils(1);
        private TestUtils testUtils2 = new TestUtils(2);

        private SecurityMgr securityMgr;
        private LogProvider logProvider;
        private LogProvider systemLog;

        public MainForm()
        {
            InitializeComponent();
            this.systemLog = new LogProvider(LogType.File, "lineManager_system_log.txt", true);
        }


        private void linesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // check if the same form is already opened ?
            bool isOpened = false;
            foreach(Form form in this.childFormList) 
            {
                if (form.GetType() == typeof(LinesForm))
                {
                    form.Activate();
                    isOpened = true;
                }
            }

            if (!isOpened)
            {
                LinesForm aLinesForm = new LinesForm(this.logProvider);
                aLinesForm.MdiParent = this;
                this.childFormList.Add(aLinesForm);
                aLinesForm.FormClosed += new FormClosedEventHandler(childFormClosed);
                aLinesForm.Show();
            }
        }

        private void childFormClosed(object sender, FormClosedEventArgs e)
        {
            //a child form was closed
            Form f = (Form)sender;
            this.childFormList.Remove(f);
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void startHostLineService1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.testUtils1.StartHostLineService();
        }

        private void stopHostLineService1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.testUtils2.StartHostLineService();
        }

        private void callMethod1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.testUtils1.testLineManager.myLine0.PushStationButton(1, "STOP");
            MessageBox.Show(this.testUtils1.testLineManager.myLine0.ReadRealTimeData(1)[0].Value);

        }

        private void testFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestForm aForm = new TestForm();
            aForm.MdiParent = this;
            this.childFormList.Add(aForm);
            aForm.FormClosed += new FormClosedEventHandler(childFormClosed);
            aForm.Show();
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

                Properties.Settings.ChangeConnectionString(builder.ConnectionString);

                this.systemLog.LogAlert(AlertType.Info, DateTime.Now.ToString() + " --- " + "Admin", "User = " + User.Name);
                this.systemLog.LogAlert(AlertType.Info, DateTime.Now.ToString() + " --- " + "Admin", "Password = " + User.Password);

                this.systemLog.LogAlert(AlertType.Info, DateTime.Now.ToString() + " --- " + "Admin", "builder.ConnectionString = " + builder.ConnectionString);
                this.systemLog.LogAlert(AlertType.Info, DateTime.Now.ToString() + " --- " + "Admin", "DetroitConnectionString = " + Properties.Settings.Default.DetroitConnectionString.ToString());


                this.logProvider = new LogProvider(LogType.SQL, Properties.Settings.Default.DetroitConnectionString.ToString(), false);
                this.logProvider.LogAlert(0, "NA", "LineManager", this.Name, "User login succesfull.", User.Name);
            }
            else
            {
                // revoke access
                // this.logProvider.LogAlert(0, "", "Application", this.Name, "User login unsuccesfull.", User.Name);  
                this.Close();
            };

        }

        private void chassisBufferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ProductBuffer aProductBuffer = new ProductBuffer(Properties.Settings.Default.DetroitConnectionString);
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

        private void uncopletedChassisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                UncompletedProducts aUncompletedProducts = new UncompletedProducts(Properties.Settings.Default.DetroitConnectionString);
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

        private void stockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Stock aStockForm = new Stock(Properties.Settings.Default.DetroitConnectionString);
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

        private void applicationLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Log aForm = new Log(Properties.Settings.Default.DetroitConnectionString);
                aForm.MdiParent = this;
                //aForm.Dock = DockStyle.Bottom;
                //aForm.FormBorderStyle = FormBorderStyle.SizableToolWindow;
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
                this.logProvider.LogAlert(0, "NA", "LineManager", this.Name, "User logged off.", User.Name);
            }
        }

        private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {

        }

        private void toolStripContainer1_BottomToolStripPanel_Click(object sender, EventArgs e)
        {

        }

        private void lineQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                LineQueueForm aLineQueueForm = new LineQueueForm(Properties.Settings.Default.DetroitConnectionString);
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









    }
}
