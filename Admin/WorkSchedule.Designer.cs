namespace Admin
{
    partial class WorkSchedule
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.dgrWorkSched = new System.Windows.Forms.DataGridView();
            this.productBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.detroitDataSet = new Admin.DetroitDataSet();
            this.btnRecalc = new System.Windows.Forms.Button();
            this.btnReport = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbLine = new System.Windows.Forms.ComboBox();
            this.assembLineBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.assembLineTableAdapter = new Admin.DetroitDataSetTableAdapters.AssembLineTableAdapter();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.productTableAdapter = new Admin.DetroitDataSetTableAdapters.ProductTableAdapter();
            //this.lineHostController1 = new wsLineHostController.LineHostController();
            this.planKeyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FinishTakt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.finishTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgrWorkSched)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detroitDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.assembLineBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCancel.Location = new System.Drawing.Point(559, 394);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(478, 394);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // dgrWorkSched
            // 
            this.dgrWorkSched.AutoGenerateColumns = false;
            this.dgrWorkSched.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgrWorkSched.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.planKeyDataGridViewTextBoxColumn,
            this.productNameDataGridViewTextBoxColumn,
            this.FinishTakt,
            this.finishTimeDataGridViewTextBoxColumn});
            this.dgrWorkSched.DataSource = this.productBindingSource;
            this.dgrWorkSched.Location = new System.Drawing.Point(12, 50);
            this.dgrWorkSched.Name = "dgrWorkSched";
            this.dgrWorkSched.RowHeadersVisible = false;
            this.dgrWorkSched.RowHeadersWidth = 30;
            this.dgrWorkSched.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgrWorkSched.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgrWorkSched.Size = new System.Drawing.Size(622, 329);
            this.dgrWorkSched.TabIndex = 11;
            // 
            // productBindingSource
            // 
            this.productBindingSource.DataMember = "Product";
            this.productBindingSource.DataSource = this.detroitDataSet;
            // 
            // detroitDataSet
            // 
            this.detroitDataSet.DataSetName = "DetroitDataSet";
            this.detroitDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // btnRecalc
            // 
            this.btnRecalc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRecalc.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnRecalc.Location = new System.Drawing.Point(390, 23);
            this.btnRecalc.Name = "btnRecalc";
            this.btnRecalc.Size = new System.Drawing.Size(119, 23);
            this.btnRecalc.TabIndex = 12;
            this.btnRecalc.Text = "&Recalculate";
            this.btnRecalc.UseVisualStyleBackColor = true;
            this.btnRecalc.Visible = false;
            this.btnRecalc.Click += new System.EventHandler(this.btnRecalc_Click);
            // 
            // btnReport
            // 
            this.btnReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReport.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnReport.Location = new System.Drawing.Point(515, 23);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(119, 23);
            this.btnReport.TabIndex = 13;
            this.btnReport.Text = "&Print ...";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Visible = false;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 30;
            this.label1.Text = "Select assembly line";
            // 
            // cbLine
            // 
            this.cbLine.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.assembLineBindingSource, "Id", true));
            this.cbLine.DataSource = this.assembLineBindingSource;
            this.cbLine.DisplayMember = "Name";
            this.cbLine.FormattingEnabled = true;
            this.cbLine.Location = new System.Drawing.Point(12, 23);
            this.cbLine.Name = "cbLine";
            this.cbLine.Size = new System.Drawing.Size(244, 21);
            this.cbLine.TabIndex = 29;
            this.cbLine.ValueMember = "Id";
            this.cbLine.SelectedValueChanged += new System.EventHandler(this.cbLine_SelectedValueChanged);
            // 
            // assembLineBindingSource
            // 
            this.assembLineBindingSource.DataMember = "AssembLine";
            this.assembLineBindingSource.DataSource = this.detroitDataSet;
            // 
            // assembLineTableAdapter
            // 
            this.assembLineTableAdapter.ClearBeforeFill = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(12, 394);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 23);
            this.button1.TabIndex = 31;
            this.button1.Text = "Daily plan";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.Location = new System.Drawing.Point(117, 394);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(104, 23);
            this.button2.TabIndex = 32;
            this.button2.Text = "Monthly plan";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // productTableAdapter
            // 
            this.productTableAdapter.ClearBeforeFill = true;
            // 
            // lineHostController1
            // 
            //this.lineHostController1.ExitCode = 0;
            //this.lineHostController1.ServiceName = "AssemplyLineService";
            // 
            // planKeyDataGridViewTextBoxColumn
            // 
            this.planKeyDataGridViewTextBoxColumn.DataPropertyName = "PlanKey";
            this.planKeyDataGridViewTextBoxColumn.HeaderText = "PlanKey";
            this.planKeyDataGridViewTextBoxColumn.Name = "planKeyDataGridViewTextBoxColumn";
            // 
            // productNameDataGridViewTextBoxColumn
            // 
            this.productNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.productNameDataGridViewTextBoxColumn.DataPropertyName = "ProductName";
            this.productNameDataGridViewTextBoxColumn.HeaderText = "ProductName";
            this.productNameDataGridViewTextBoxColumn.Name = "productNameDataGridViewTextBoxColumn";
            // 
            // FinishTakt
            // 
            this.FinishTakt.DataPropertyName = "FinishTakt";
            this.FinishTakt.HeaderText = "FinishTakt";
            this.FinishTakt.Name = "FinishTakt";
            this.FinishTakt.Width = 60;
            // 
            // finishTimeDataGridViewTextBoxColumn
            // 
            this.finishTimeDataGridViewTextBoxColumn.DataPropertyName = "FinishTime";
            this.finishTimeDataGridViewTextBoxColumn.HeaderText = "FinishTime";
            this.finishTimeDataGridViewTextBoxColumn.Name = "finishTimeDataGridViewTextBoxColumn";
            this.finishTimeDataGridViewTextBoxColumn.Width = 150;
            // 
            // WorkSchedule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(646, 429);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbLine);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.btnRecalc);
            this.Controls.Add(this.dgrWorkSched);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "WorkSchedule";
            this.Text = "Work schedule";
            this.Load += new System.EventHandler(this.WorkSchedule_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgrWorkSched)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detroitDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.assembLineBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.DataGridView dgrWorkSched;
        private System.Windows.Forms.Button btnRecalc;
        private DetroitDataSet detroitDataSet;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbLine;
        private System.Windows.Forms.BindingSource assembLineBindingSource;
        private DetroitDataSetTableAdapters.AssembLineTableAdapter assembLineTableAdapter;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.BindingSource productBindingSource;
        private DetroitDataSetTableAdapters.ProductTableAdapter productTableAdapter;
        //private wsLineHostController.LineHostController lineHostController1;
        private System.Windows.Forms.DataGridViewTextBoxColumn planKeyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FinishTakt;
        private System.Windows.Forms.DataGridViewTextBoxColumn finishTimeDataGridViewTextBoxColumn;
    }
}