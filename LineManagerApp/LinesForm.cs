using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AppLog;

namespace LineManagerApp
{
    public partial class LinesForm : Form
    {
        private LogProvider myLog;
        private List<Form> childFormList = new List<Form>();

        public LinesForm(LogProvider logProvider)
        {
                InitializeComponent();
                this.myLog = logProvider;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LinesForm_Load(object sender, EventArgs e)
        {
            try { 
                this.assembLineTableAdapter.Fill(this.dataSet1.AssembLine);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(
                    "There is no connection to the database:"
                    + "\n" + Properties.Settings.Default.Properties["MyDetroitConnectionString"].DefaultValue.ToString(),
                    "Warning!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }


        private void btnOpenPanel_Click(object sender, EventArgs e)
        {
            this.openLineDashBoard(sender);
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            openLineDashBoard(sender);
        }

        private void openLineDashBoard(object sender)
        {
            if (this.dataGridView1.CurrentRow.Cells["Id"] != null)  // check'it to be sure_ that "line_id" wouldn't be null
            {
                int line_id = (int)this.dataGridView1.CurrentRow.Cells["Id"].Value;

                int line_hashcode = 0;
                bool form_showed = false;

                //-----------------------------------------------------------------
                // check if form already opened, find and open it
                if (this.dataGridView1.CurrentRow.Cells["formHashCode"].Value != null)
                {
                    line_hashcode = (int)this.dataGridView1.CurrentRow.Cells["formHashCode"].Value;
                    foreach (Form childForm in this.childFormList)
                    {
                        if (childForm.GetHashCode() == line_hashcode)
                        {
                            childForm.Activate();
                            form_showed = true;
                        }
                    }
                }
                //-----------------------------------------------------------------
                // check if form already opened, find and open it
                if (!form_showed)
                {
                    try
                    {
                        LineDashboard aDashBoard = new LineDashboard(line_id, this.myLog);
                        int form_hashcode = aDashBoard.GetHashCode();
                        this.dataGridView1.CurrentRow.Cells["formHashCode"].Value = form_hashcode;
                        this.childFormList.Add(aDashBoard);
                        aDashBoard.FormClosed += new FormClosedEventHandler(childFormClosed);
                        aDashBoard.MdiParent = this.ParentForm;
                        aDashBoard.Show();
                    }
                    catch (LineConnectionException e) 
                    {
                        MessageBox.Show("There is no connection. Please, try later.", "Connecting to line service ... (2)", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Asterisk);                        
                        //myLog.LogAlert(LogType.SQL, this.GetType().ToString(), e.Message.ToString());
                    }

                }

            }
        }

        private void childFormClosed(object sender, FormClosedEventArgs e)
        {
            //a child form was closed
            Form f = (Form)sender;
            this.childFormList.Remove(f);
        }

        private void LinesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.childFormList.Count > 0) 
            {
                MessageBox.Show("Please, close all child forms before.", "Hello!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
        }


    }
}
