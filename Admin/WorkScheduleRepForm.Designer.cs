namespace Admin
{
    partial class WorkScheduleRepForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.WorkScheduleBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.DetroitDataSet = new Admin.DetroitDataSet();
            this.repWS = new Microsoft.Reporting.WinForms.ReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.WorkScheduleBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DetroitDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // WorkScheduleBindingSource
            // 
            this.WorkScheduleBindingSource.DataMember = "WorkSchedule";
            this.WorkScheduleBindingSource.DataSource = this.DetroitDataSet;
            // 
            // DetroitDataSet
            // 
            this.DetroitDataSet.DataSetName = "DetroitDataSet";
            this.DetroitDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // repWS
            // 
            this.repWS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            reportDataSource1.Name = "wsDataset";
            reportDataSource1.Value = this.WorkScheduleBindingSource;
            this.repWS.LocalReport.DataSources.Add(reportDataSource1);
            this.repWS.LocalReport.ReportEmbeddedResource = "Admin.WorkSchedule.rdlc";
            this.repWS.Location = new System.Drawing.Point(-1, -4);
            this.repWS.Name = "repWS";
            this.repWS.Size = new System.Drawing.Size(646, 519);
            this.repWS.TabIndex = 0;
            // 
            // WorkScheduleRepForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 511);
            this.Controls.Add(this.repWS);
            this.Name = "WorkScheduleRepForm";
            this.Text = "WorkScheduleRepForm";
            this.Load += new System.EventHandler(this.WorkScheduleRepForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.WorkScheduleBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DetroitDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public Microsoft.Reporting.WinForms.ReportViewer repWS;
        public System.Windows.Forms.BindingSource WorkScheduleBindingSource;
        private DetroitDataSet DetroitDataSet;
    }
}