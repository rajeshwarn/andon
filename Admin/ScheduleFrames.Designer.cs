namespace Admin
{
    partial class ScheduleFrames
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
            this.dgrFrames = new System.Windows.Forms.DataGridView();
            this.OrderNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Start = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Finish = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.shiftNumDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assembLineSchedulerFrameBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.assembLineBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.detroitDataSet = new Admin.DetroitDataSet();
            this.schedulerFrameBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.dpkDate = new System.Windows.Forms.DateTimePicker();
            this.btnCopyFrames = new System.Windows.Forms.Button();
            this.btnPasteFrames = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnFillDefault = new System.Windows.Forms.Button();
            this.schedulerFrameTableAdapter = new Admin.DetroitDataSetTableAdapters.SchedulerFrameTableAdapter();
            this.label3 = new System.Windows.Forms.Label();
            this.cbLine = new System.Windows.Forms.ComboBox();
            this.assembLineTableAdapter = new Admin.DetroitDataSetTableAdapters.AssembLineTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dgrFrames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.assembLineSchedulerFrameBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.assembLineBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detroitDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerFrameBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCancel.Location = new System.Drawing.Point(559, 409);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(478, 409);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // dgrFrames
            // 
            this.dgrFrames.AutoGenerateColumns = false;
            this.dgrFrames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgrFrames.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OrderNum,
            this.nameDataGridViewTextBoxColumn,
            this.Start,
            this.Finish,
            this.Length,
            this.type,
            this.shiftNumDataGridViewTextBoxColumn});
            this.dgrFrames.DataSource = this.assembLineSchedulerFrameBindingSource;
            this.dgrFrames.Location = new System.Drawing.Point(12, 63);
            this.dgrFrames.Name = "dgrFrames";
            this.dgrFrames.RowHeadersWidth = 30;
            this.dgrFrames.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgrFrames.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgrFrames.Size = new System.Drawing.Size(622, 300);
            this.dgrFrames.TabIndex = 9;
            this.dgrFrames.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgrFrames_CellEndEdit);
            this.dgrFrames.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgrFrames_DefaultValuesNeeded);
            // 
            // OrderNum
            // 
            this.OrderNum.DataPropertyName = "OrderNum";
            this.OrderNum.HeaderText = "Num";
            this.OrderNum.Name = "OrderNum";
            this.OrderNum.Width = 60;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // Start
            // 
            this.Start.DataPropertyName = "Start";
            this.Start.HeaderText = "Start";
            this.Start.Name = "Start";
            // 
            // Finish
            // 
            this.Finish.DataPropertyName = "Finish";
            this.Finish.HeaderText = "Finish";
            this.Finish.Name = "Finish";
            // 
            // Length
            // 
            this.Length.DataPropertyName = "Length";
            this.Length.HeaderText = "Length";
            this.Length.Name = "Length";
            this.Length.Width = 50;
            // 
            // type
            // 
            this.type.DataPropertyName = "Type";
            this.type.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.type.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.type.HeaderText = "Type";
            this.type.Name = "type";
            this.type.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.type.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.type.Width = 60;
            // 
            // shiftNumDataGridViewTextBoxColumn
            // 
            this.shiftNumDataGridViewTextBoxColumn.DataPropertyName = "ShiftNum";
            this.shiftNumDataGridViewTextBoxColumn.HeaderText = "ShiftNum";
            this.shiftNumDataGridViewTextBoxColumn.Name = "shiftNumDataGridViewTextBoxColumn";
            this.shiftNumDataGridViewTextBoxColumn.Width = 50;
            // 
            // assembLineSchedulerFrameBindingSource
            // 
            this.assembLineSchedulerFrameBindingSource.DataMember = "AssembLine_SchedulerFrame";
            this.assembLineSchedulerFrameBindingSource.DataSource = this.assembLineBindingSource;
            // 
            // assembLineBindingSource
            // 
            this.assembLineBindingSource.DataMember = "AssembLine";
            this.assembLineBindingSource.DataSource = this.detroitDataSet;
            // 
            // detroitDataSet
            // 
            this.detroitDataSet.DataSetName = "DetroitDataSet";
            this.detroitDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // schedulerFrameBindingSource
            // 
            this.schedulerFrameBindingSource.DataMember = "SchedulerFrame";
            this.schedulerFrameBindingSource.DataSource = this.detroitDataSet;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(12, 409);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "&Adjust frames";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dpkDate
            // 
            this.dpkDate.Location = new System.Drawing.Point(12, 381);
            this.dpkDate.Name = "dpkDate";
            this.dpkDate.Size = new System.Drawing.Size(138, 20);
            this.dpkDate.TabIndex = 12;
            this.dpkDate.Value = new System.DateTime(2012, 4, 25, 0, 0, 0, 0);
            this.dpkDate.ValueChanged += new System.EventHandler(this.dpkDate_ValueChanged);
            // 
            // btnCopyFrames
            // 
            this.btnCopyFrames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyFrames.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCopyFrames.Location = new System.Drawing.Point(249, 380);
            this.btnCopyFrames.Name = "btnCopyFrames";
            this.btnCopyFrames.Size = new System.Drawing.Size(112, 23);
            this.btnCopyFrames.TabIndex = 13;
            this.btnCopyFrames.Text = "&Copy frames";
            this.btnCopyFrames.UseVisualStyleBackColor = true;
            this.btnCopyFrames.Click += new System.EventHandler(this.btnCopyFrames_Click);
            // 
            // btnPasteFrames
            // 
            this.btnPasteFrames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPasteFrames.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnPasteFrames.Location = new System.Drawing.Point(249, 409);
            this.btnPasteFrames.Name = "btnPasteFrames";
            this.btnPasteFrames.Size = new System.Drawing.Size(112, 23);
            this.btnPasteFrames.TabIndex = 14;
            this.btnPasteFrames.Text = "&Paste frames";
            this.btnPasteFrames.UseVisualStyleBackColor = true;
            this.btnPasteFrames.Click += new System.EventHandler(this.btnPasteFrames_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(156, 378);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 23);
            this.label1.TabIndex = 15;
            this.label1.Text = "<<";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(190, 378);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 23);
            this.label2.TabIndex = 16;
            this.label2.Text = ">>";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // btnFillDefault
            // 
            this.btnFillDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFillDefault.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnFillDefault.Location = new System.Drawing.Point(131, 409);
            this.btnFillDefault.Name = "btnFillDefault";
            this.btnFillDefault.Size = new System.Drawing.Size(112, 23);
            this.btnFillDefault.TabIndex = 17;
            this.btnFillDefault.Text = "&Fill default";
            this.btnFillDefault.UseVisualStyleBackColor = true;
            this.btnFillDefault.Click += new System.EventHandler(this.btnFillDefault_Click);
            // 
            // schedulerFrameTableAdapter
            // 
            this.schedulerFrameTableAdapter.ClearBeforeFill = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Select assembly line";
            // 
            // cbLine
            // 
            this.cbLine.DataSource = this.assembLineBindingSource;
            this.cbLine.DisplayMember = "Name";
            this.cbLine.FormattingEnabled = true;
            this.cbLine.Location = new System.Drawing.Point(12, 25);
            this.cbLine.Name = "cbLine";
            this.cbLine.Size = new System.Drawing.Size(204, 21);
            this.cbLine.TabIndex = 29;
            this.cbLine.ValueMember = "Id";
            // 
            // assembLineTableAdapter
            // 
            this.assembLineTableAdapter.ClearBeforeFill = true;
            // 
            // ScheduleFrames
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(646, 444);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbLine);
            this.Controls.Add(this.btnFillDefault);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnPasteFrames);
            this.Controls.Add(this.btnCopyFrames);
            this.Controls.Add(this.dpkDate);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dgrFrames);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ScheduleFrames";
            this.Text = "Schedule frames";
            this.Load += new System.EventHandler(this.ScheduleFrames_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgrFrames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.assembLineSchedulerFrameBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.assembLineBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detroitDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerFrameBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.DataGridView dgrFrames;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DateTimePicker dpkDate;
        private System.Windows.Forms.Button btnCopyFrames;
        private System.Windows.Forms.Button btnPasteFrames;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnFillDefault;
        private DetroitDataSet detroitDataSet;
        private System.Windows.Forms.BindingSource schedulerFrameBindingSource;
        private DetroitDataSetTableAdapters.SchedulerFrameTableAdapter schedulerFrameTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Start;
        private System.Windows.Forms.DataGridViewTextBoxColumn Finish;
        private System.Windows.Forms.DataGridViewTextBoxColumn Length;
        private System.Windows.Forms.DataGridViewComboBoxColumn type;
        private System.Windows.Forms.DataGridViewTextBoxColumn shiftNumDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbLine;
        private System.Windows.Forms.BindingSource assembLineBindingSource;
        private DetroitDataSetTableAdapters.AssembLineTableAdapter assembLineTableAdapter;
        private System.Windows.Forms.BindingSource assembLineSchedulerFrameBindingSource;
    }
}