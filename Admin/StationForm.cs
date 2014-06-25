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
    public partial class StationForm : Form
    {

        private BindingSource type_bindingSource = new BindingSource();
        public StationForm()
        {
            InitializeComponent();
            fsModule.CheckFormAccess(this.Name);
        }

        private void StationForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'detroitDataSet.MapParts' table. You can move, or remove it, as needed.
            this.mapPartsTableAdapter.Fill(this.detroitDataSet.MapParts);
            // TODO: This line of code loads data into the 'detroitDataSet.Parts' table. You can move, or remove it, as needed.
            this.partsTableAdapter.Fill(this.detroitDataSet.Parts);
            type_bindingSource.Add( new MyValue { id = 1, name = "Push-button" });
            type_bindingSource.Add( new MyValue { id = 2, name = "Lamp" });
            type_bindingSource.Add( new MyValue { id = 3, name = "Switch" });

            this.typeDataGridViewTextBoxColumn.DataSource = type_bindingSource;
            this.typeDataGridViewTextBoxColumn.DisplayMember = "name";
            this.typeDataGridViewTextBoxColumn.ValueMember = "id";

            this.controlTableAdapter.Fill(this.detroitDataSet.Control);
            this.stationTableAdapter.Fill(this.detroitDataSet.Station);

            if (fsModule.AccessMode == FormAssessMode.Write)
            {
                this.setFormWriteMode();
            }
            else if (fsModule.AccessMode == FormAssessMode.Read)
            {
                this.setFormReadMode();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.controlTableAdapter.Update(this.detroitDataSet.Control);            
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.controlTableAdapter.Update(this.detroitDataSet.Control);
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            this.controlTableAdapter.Fill(this.detroitDataSet.Control);
        }

        public class MyValue
        {
            public int id { get; set; }
            public string name { get; set; }
        }


        private FormSecurityModule fsModule = new FormSecurityModule();
        private void setFormReadMode()
        {
            this.dataGridView1.ReadOnly = true;
            this.btnOk.Enabled = false;
            this.btnSave.Enabled = false;
        }
        private void setFormWriteMode() { }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (((DataGridView)sender).Columns[ e.ColumnIndex ]
                .Name == "PartId" && (((DataGridView)sender).IsCurrentCellInEditMode))
            {
                ((DataGridView)sender).CurrentRow.Cells["KeyString"].Value = 
                    
                    ((DataRowView)(
                        ((BindingSource)(
                            ((DataGridViewComboBoxColumn)(
                                ((DataGridView)sender).Columns[e.ColumnIndex])
                              ).DataSource)
                         ).Current)
                    ).Row.ItemArray[1].ToString();

            //    ((DataGridView)sender).CurrentRow.Cells["KeyString"].Value = ((DataGridView)sender).CurrentCell.Value.ToString();
            //    //this.fKControlStationBindingSource.

            } 
        }

        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && (this.dataGridView1.CurrentCell.ColumnIndex == 4)
                && this.dataGridView1.CurrentCell.ReadOnly == false)
            {
                this.dataGridView1.CurrentCell.Value = DBNull.Value;
            }
        }



    }
}
