using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Security;
using System.IO;

namespace Admin
{
    public partial class Log : Form
    {
        private Timer timer;
        public Log()
        {
            InitializeComponent();
            fsModule.CheckFormAccess(this.Name);
            this.timer = new Timer();
            this.timer.Interval = 500;
            this.timer.Enabled = true;
            this.timer.Tick += new EventHandler(timer_Tick);
            this.dtPickerBegin.Value = DateTime.Today;
            this.dtPickerEnd.Value = DateTime.Now;
        }

        public Log(string databaseConnectionString)
        {
            Properties.Settings.ChangeConnectionString(databaseConnectionString);
            InitializeComponent();
            fsModule.CheckFormAccess(this.Name);
            this.timer = new Timer();
            this.timer.Interval = 500;
            this.timer.Enabled = true;
            this.timer.Tick += new EventHandler(timer_Tick);
            this.dtPickerBegin.Value = DateTime.Today;
            this.dtPickerEnd.Value = DateTime.Now;
        }

        private void Log_Load(object sender, EventArgs e)
        {
            this.logTableAdapter.Fill(this.detroitDataSet.Log, this.dtPickerBegin.Value, this.dtPickerEnd.Value);
            this.dataGridView1.Sort(this.dataGridView1.Columns[0], ListSortDirection.Descending);

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private FormSecurityModule fsModule = new FormSecurityModule();

        private void setFormReadMode()
        {
            this.dataGridView1.ReadOnly = true;
            //this.dataGridView2.ReadOnly = true;
            //this.dataGridView3.ReadOnly = true;
            this.btnOk.Enabled = false;
            //this.btnSave.Enabled = false;
        }
        private void setFormWriteMode() { }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.CurrentCell != null)
            {
                DataGridViewColumn currentViewColumn = this.dataGridView1.CurrentCell.OwningColumn;

                // the first column is a datetime column, it doesn't support "like %" comparision!!
                if (currentViewColumn.Index > 0)
                {
                    string columnName = currentViewColumn.DataPropertyName;
                    string filterSelect = columnName + " like '%" + this.tbxFilter.Text + "%'";
                    this.logBindingSource.Filter = filterSelect;
                    if (this.dataGridView1.Rows.Count > 0)
                    {
                        this.dataGridView1.ClearSelection();
                        this.dataGridView1.CurrentCell = this.dataGridView1[currentViewColumn.Index, 0];
                    }
                }
            }

        }

        private void tbxFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.btnFilter_Click(sender, new EventArgs());
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.logTableAdapter.Fill(this.detroitDataSet.Log, this.dtPickerBegin.Value, this.dtPickerEnd.Value);
        }


        private void timer_Tick(object sender, EventArgs e) 
        {
            if (this.chbAutoRefresh.Checked)
            {
                this.btnRefresh_Click(sender, e);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {          
            SaveFileDialog saveFileDlg = new SaveFileDialog();

            saveFileDlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDlg.FilterIndex = 2;
            saveFileDlg.RestoreDirectory = true;
            DateTime now = DateTime.Now;
            saveFileDlg.FileName = "syslog_" + DateTime.Now.ToString("yyyyMMyy-HHmmss") + ".txt";
            StreamWriter myStream = null;

            try
            {  
                if(saveFileDlg.ShowDialog() == DialogResult.OK)
                {
                    myStream = new StreamWriter(saveFileDlg.FileName);
                    myStream.WriteLine();
                    for (int row = 0; row < this.dataGridView1.Rows.Count; row++)
                    {
                        string line = "";
                        for (int col = 0; col < this.dataGridView1.ColumnCount; col++)
                        {
                            string value = dataGridView1.Rows[row].Cells[col].Value.ToString();
                            line += (string.IsNullOrEmpty(line) ? " " : ";;") + removeLineEndings(value) ;
                        }
                        myStream.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Ups, somthing goes wrong. Sorry for that." + "\n\n" + ex.Message, 
                    "The moment!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); 
            }
            finally {
                if (myStream != null) 
                    myStream.Close();
            }
        }

        private void logBindingSource_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
        }

        private void logBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            this.laRows.Text = "Rows: " + ((BindingSource)sender).Count;
        }

        private string removeLineEndings(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }
            string lineSeparator = ((char)0x2028).ToString();
            string paragraphSeparator = ((char)0x2029).ToString();

            return value.Replace("\r\n", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty).Replace(lineSeparator, string.Empty).Replace(paragraphSeparator, string.Empty);
        }

    }
}
