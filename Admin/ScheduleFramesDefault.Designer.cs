namespace Admin
{
    partial class ScheduleFramesDefault
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
            this.button1 = new System.Windows.Forms.Button();
            this.OrderNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Start = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Finish = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.shiftNumDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.schedulerFrameDefaultBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.detroitDataSet = new Admin.DetroitDataSet();
            this.schedulerFrameDefaultTableAdapter = new Admin.DetroitDataSetTableAdapters.SchedulerFrameDefaultTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dgrFrames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerFrameDefaultBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detroitDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCancel.Location = new System.Drawing.Point(745, 382);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(637, 382);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 28);
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
            this.dgrFrames.DataSource = this.schedulerFrameDefaultBindingSource;
            this.dgrFrames.Location = new System.Drawing.Point(16, 28);
            this.dgrFrames.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgrFrames.Name = "dgrFrames";
            this.dgrFrames.RowHeadersVisible = false;
            this.dgrFrames.RowHeadersWidth = 30;
            this.dgrFrames.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgrFrames.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgrFrames.Size = new System.Drawing.Size(829, 335);
            this.dgrFrames.TabIndex = 9;
            this.dgrFrames.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgrFrames_CellEndEdit);
            this.dgrFrames.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgrFrames_DefaultValuesNeeded);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(16, 382);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(149, 28);
            this.button1.TabIndex = 10;
            this.button1.Text = "&Adjust frames";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // OrderNum
            // 
            this.OrderNum.DataPropertyName = "OrderNum";
            this.OrderNum.HeaderText = "OrderNum";
            this.OrderNum.Name = "OrderNum";
            this.OrderNum.Width = 40;
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
            this.Start.Width = 80;
            // 
            // Finish
            // 
            this.Finish.DataPropertyName = "Finish";
            this.Finish.HeaderText = "Finish";
            this.Finish.Name = "Finish";
            this.Finish.Width = 80;
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
            // 
            // schedulerFrameDefaultBindingSource
            // 
            this.schedulerFrameDefaultBindingSource.DataMember = "SchedulerFrameDefault";
            this.schedulerFrameDefaultBindingSource.DataSource = this.detroitDataSet;
            // 
            // detroitDataSet
            // 
            this.detroitDataSet.DataSetName = "DetroitDataSet";
            this.detroitDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // schedulerFrameDefaultTableAdapter
            // 
            this.schedulerFrameDefaultTableAdapter.ClearBeforeFill = true;
            // 
            // ScheduleFramesDefault
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(861, 425);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dgrFrames);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ScheduleFramesDefault";
            this.Text = "Default schedule frames";
            this.Load += new System.EventHandler(this.ScheduleFramesDefault_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgrFrames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerFrameDefaultBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detroitDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.DataGridView dgrFrames;
        private System.Windows.Forms.Button button1;
        private DetroitDataSet detroitDataSet;
        private System.Windows.Forms.BindingSource schedulerFrameDefaultBindingSource;
        private DetroitDataSetTableAdapters.SchedulerFrameDefaultTableAdapter schedulerFrameDefaultTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Start;
        private System.Windows.Forms.DataGridViewTextBoxColumn Finish;
        private System.Windows.Forms.DataGridViewTextBoxColumn Length;
        private System.Windows.Forms.DataGridViewComboBoxColumn type;
        private System.Windows.Forms.DataGridViewTextBoxColumn shiftNumDataGridViewTextBoxColumn;
    }
}