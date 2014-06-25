namespace Admin
{
    partial class BatchTypeMap
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.stepDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stationIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.batchTypeMapBatchTypeStationsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.fKBatchTypeMapBatchTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.batchTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.detroitDataSet = new Admin.DetroitDataSet();
            this.isMain = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.fKMapPointsBatchTypeMapBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.assembLineBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.batchTypeTableAdapter = new Admin.DetroitDataSetTableAdapters.BatchTypeTableAdapter();
            this.batchTypeMapTableAdapter = new Admin.DetroitDataSetTableAdapters.BatchTypeMapTableAdapter();
            this.assembLineTableAdapter = new Admin.DetroitDataSetTableAdapters.AssembLineTableAdapter();
            this.mapPointsTableAdapter = new Admin.DetroitDataSetTableAdapters.MapPointsTableAdapter();
            this.batchTypeStationsTableAdapter = new Admin.DetroitDataSetTableAdapters.BatchTypeStationsTableAdapter();
            this.mapNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Takt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LineId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.NextLineId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.NextLineAuto = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.batchTypeMapBatchTypeStationsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fKBatchTypeMapBatchTypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.batchTypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detroitDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fKMapPointsBatchTypeMapBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.assembLineBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(12, 436);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCancel.Location = new System.Drawing.Point(537, 436);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(456, 436);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 17;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 203);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Assembly order on the line";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.stepDataGridViewTextBoxColumn,
            this.stationIdDataGridViewTextBoxColumn,
            this.isMain});
            this.dataGridView1.DataSource = this.fKMapPointsBatchTypeMapBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(12, 219);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 31;
            this.dataGridView1.Size = new System.Drawing.Size(597, 200);
            this.dataGridView1.TabIndex = 20;
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            this.dataGridView1.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridView1_DefaultValuesNeeded);
            this.dataGridView1.Enter += new System.EventHandler(this.dataGridView1_Enter);
            // 
            // stepDataGridViewTextBoxColumn
            // 
            this.stepDataGridViewTextBoxColumn.DataPropertyName = "Step";
            this.stepDataGridViewTextBoxColumn.HeaderText = "Step";
            this.stepDataGridViewTextBoxColumn.Name = "stepDataGridViewTextBoxColumn";
            this.stepDataGridViewTextBoxColumn.Width = 70;
            // 
            // stationIdDataGridViewTextBoxColumn
            // 
            this.stationIdDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.stationIdDataGridViewTextBoxColumn.DataPropertyName = "StationId";
            this.stationIdDataGridViewTextBoxColumn.DataSource = this.batchTypeMapBatchTypeStationsBindingSource;
            this.stationIdDataGridViewTextBoxColumn.DisplayMember = "Name";
            this.stationIdDataGridViewTextBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.stationIdDataGridViewTextBoxColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stationIdDataGridViewTextBoxColumn.HeaderText = "Station";
            this.stationIdDataGridViewTextBoxColumn.Name = "stationIdDataGridViewTextBoxColumn";
            this.stationIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.stationIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.stationIdDataGridViewTextBoxColumn.ValueMember = "Station_Id";
            // 
            // batchTypeMapBatchTypeStationsBindingSource
            // 
            this.batchTypeMapBatchTypeStationsBindingSource.DataMember = "BatchTypeMap_BatchTypeStations";
            this.batchTypeMapBatchTypeStationsBindingSource.DataSource = this.fKBatchTypeMapBatchTypeBindingSource;
            // 
            // fKBatchTypeMapBatchTypeBindingSource
            // 
            this.fKBatchTypeMapBatchTypeBindingSource.DataMember = "FK_BatchTypeMap_BatchType";
            this.fKBatchTypeMapBatchTypeBindingSource.DataSource = this.batchTypeBindingSource;
            // 
            // batchTypeBindingSource
            // 
            this.batchTypeBindingSource.DataMember = "BatchType";
            this.batchTypeBindingSource.DataSource = this.detroitDataSet;
            // 
            // detroitDataSet
            // 
            this.detroitDataSet.DataSetName = "DetroitDataSet";
            this.detroitDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // isMain
            // 
            this.isMain.DataPropertyName = "IsMain";
            this.isMain.FalseValue = "0";
            this.isMain.HeaderText = "IsMain";
            this.isMain.IndeterminateValue = "1";
            this.isMain.Name = "isMain";
            this.isMain.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isMain.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isMain.TrueValue = "1";
            // 
            // fKMapPointsBatchTypeMapBindingSource
            // 
            this.fKMapPointsBatchTypeMapBindingSource.DataMember = "FK_MapPoints_BatchTypeMap";
            this.fKMapPointsBatchTypeMapBindingSource.DataSource = this.fKBatchTypeMapBatchTypeBindingSource;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Select batch type ...";
            // 
            // comboBox1
            // 
            this.comboBox1.DataSource = this.batchTypeBindingSource;
            this.comboBox1.DisplayMember = "Name";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 25);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(204, 21);
            this.comboBox1.TabIndex = 22;
            this.comboBox1.ValueMember = "Id";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Batch goes on the lines:";
            // 
            // dataGridView2
            // 
            this.dataGridView2.AutoGenerateColumns = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.mapNameDataGridViewTextBoxColumn,
            this.Takt,
            this.LineId,
            this.NextLineId,
            this.NextLineAuto});
            this.dataGridView2.DataSource = this.fKBatchTypeMapBatchTypeBindingSource;
            this.dataGridView2.Location = new System.Drawing.Point(12, 72);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersWidth = 31;
            this.dataGridView2.Size = new System.Drawing.Size(597, 123);
            this.dataGridView2.TabIndex = 24;
            this.dataGridView2.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellEndEdit);
            this.dataGridView2.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellLeave);
            this.dataGridView2.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridView2_DefaultValuesNeeded);
            this.dataGridView2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dataGridView2_KeyUp);
            // 
            // assembLineBindingSource
            // 
            this.assembLineBindingSource.DataMember = "AssembLine";
            this.assembLineBindingSource.DataSource = this.detroitDataSet;
            // 
            // batchTypeTableAdapter
            // 
            this.batchTypeTableAdapter.ClearBeforeFill = true;
            // 
            // batchTypeMapTableAdapter
            // 
            this.batchTypeMapTableAdapter.ClearBeforeFill = true;
            // 
            // assembLineTableAdapter
            // 
            this.assembLineTableAdapter.ClearBeforeFill = true;
            // 
            // mapPointsTableAdapter
            // 
            this.mapPointsTableAdapter.ClearBeforeFill = true;
            // 
            // batchTypeStationsTableAdapter
            // 
            this.batchTypeStationsTableAdapter.ClearBeforeFill = true;
            // 
            // mapNameDataGridViewTextBoxColumn
            // 
            this.mapNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.mapNameDataGridViewTextBoxColumn.DataPropertyName = "MapName";
            this.mapNameDataGridViewTextBoxColumn.HeaderText = "Map route name";
            this.mapNameDataGridViewTextBoxColumn.Name = "mapNameDataGridViewTextBoxColumn";
            // 
            // Takt
            // 
            this.Takt.DataPropertyName = "Takt";
            this.Takt.HeaderText = "Takt";
            this.Takt.Name = "Takt";
            this.Takt.Width = 70;
            // 
            // LineId
            // 
            this.LineId.DataPropertyName = "LineId";
            this.LineId.DataSource = this.assembLineBindingSource;
            this.LineId.DisplayMember = "Code";
            this.LineId.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.LineId.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LineId.HeaderText = "Line code";
            this.LineId.Name = "LineId";
            this.LineId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.LineId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.LineId.ValueMember = "Id";
            // 
            // NextLineId
            // 
            this.NextLineId.DataPropertyName = "NextLineId";
            this.NextLineId.DataSource = this.assembLineBindingSource;
            dataGridViewCellStyle2.Format = "N0";
            this.NextLineId.DefaultCellStyle = dataGridViewCellStyle2;
            this.NextLineId.DisplayMember = "Code";
            this.NextLineId.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.NextLineId.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NextLineId.HeaderText = "Next line code";
            this.NextLineId.Name = "NextLineId";
            this.NextLineId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.NextLineId.ValueMember = "Id";
            // 
            // NextLineAuto
            // 
            this.NextLineAuto.DataPropertyName = "NextLineAuto";
            this.NextLineAuto.FalseValue = "0";
            this.NextLineAuto.HeaderText = "NextAuto";
            this.NextLineAuto.IndeterminateValue = "1";
            this.NextLineAuto.Name = "NextLineAuto";
            this.NextLineAuto.TrueValue = "1";
            this.NextLineAuto.Visible = false;
            // 
            // BatchTypeMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 469);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "BatchTypeMap";
            this.Text = "BatchTypeMap";
            this.Load += new System.EventHandler(this.BatchTypeMap_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.batchTypeMapBatchTypeStationsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fKBatchTypeMapBatchTypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.batchTypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detroitDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fKMapPointsBatchTypeMapBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.assembLineBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataGridView2;
        private DetroitDataSet detroitDataSet;
        private System.Windows.Forms.BindingSource batchTypeBindingSource;
        private DetroitDataSetTableAdapters.BatchTypeTableAdapter batchTypeTableAdapter;
        private System.Windows.Forms.BindingSource fKBatchTypeMapBatchTypeBindingSource;
        private DetroitDataSetTableAdapters.BatchTypeMapTableAdapter batchTypeMapTableAdapter;
        private System.Windows.Forms.BindingSource assembLineBindingSource;
        private DetroitDataSetTableAdapters.AssembLineTableAdapter assembLineTableAdapter;
        private System.Windows.Forms.BindingSource fKMapPointsBatchTypeMapBindingSource;
        private DetroitDataSetTableAdapters.MapPointsTableAdapter mapPointsTableAdapter;
        private System.Windows.Forms.BindingSource batchTypeMapBatchTypeStationsBindingSource;
        private DetroitDataSetTableAdapters.BatchTypeStationsTableAdapter batchTypeStationsTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn stepDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn stationIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isMain;
        private System.Windows.Forms.DataGridViewTextBoxColumn mapNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Takt;
        private System.Windows.Forms.DataGridViewComboBoxColumn LineId;
        private System.Windows.Forms.DataGridViewComboBoxColumn NextLineId;
        private System.Windows.Forms.DataGridViewCheckBoxColumn NextLineAuto;
    }
}