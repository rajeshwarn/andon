﻿namespace Admin
{
    partial class ProductBuffer
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
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.batchTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.detroitDataSet = new Admin.DetroitDataSet();
            this.assembLineBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.assembLineProductBufferBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.assembLineTableAdapter = new Admin.DetroitDataSetTableAdapters.AssembLineTableAdapter();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.productBufferTableAdapter = new Admin.DetroitDataSetTableAdapters.ProductBufferTableAdapter();
            this.batchTypeTableAdapter = new Admin.DetroitDataSetTableAdapters.BatchTypeTableAdapter();
            this.TimeRecord = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.batchTypeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.lineIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.nextLineIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.stateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.batchTypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detroitDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.assembLineBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.assembLineProductBufferBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnRefresh.Location = new System.Drawing.Point(12, 354);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "&Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCancel.Location = new System.Drawing.Point(595, 354);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(514, 354);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Chassis needed to be fixed ...";
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TimeRecord,
            this.productNameDataGridViewTextBoxColumn,
            this.batchTypeIdDataGridViewTextBoxColumn,
            this.lineIdDataGridViewTextBoxColumn,
            this.nextLineIdDataGridViewTextBoxColumn,
            this.stateDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.assembLineProductBufferBindingSource;
            this.dataGridView1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dataGridView1.Location = new System.Drawing.Point(12, 69);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.Size = new System.Drawing.Size(658, 279);
            this.dataGridView1.TabIndex = 10;
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
            // assembLineBindingSource
            // 
            this.assembLineBindingSource.DataMember = "AssembLine";
            this.assembLineBindingSource.DataSource = this.detroitDataSet;
            // 
            // assembLineProductBufferBindingSource
            // 
            this.assembLineProductBufferBindingSource.DataMember = "AssembLine_ProductBuffer";
            this.assembLineProductBufferBindingSource.DataSource = this.assembLineBindingSource;
            // 
            // assembLineTableAdapter
            // 
            this.assembLineTableAdapter.ClearBeforeFill = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Select assembly line";
            // 
            // comboBox1
            // 
            this.comboBox1.DataSource = this.assembLineBindingSource;
            this.comboBox1.DisplayMember = "Name";
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 26);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(301, 21);
            this.comboBox1.TabIndex = 29;
            // 
            // productBufferTableAdapter
            // 
            this.productBufferTableAdapter.ClearBeforeFill = true;
            // 
            // batchTypeTableAdapter
            // 
            this.batchTypeTableAdapter.ClearBeforeFill = true;
            // 
            // TimeRecord
            // 
            this.TimeRecord.DataPropertyName = "TimeRecord";
            this.TimeRecord.HeaderText = "TimeRecord";
            this.TimeRecord.Name = "TimeRecord";
            this.TimeRecord.ReadOnly = true;
            // 
            // productNameDataGridViewTextBoxColumn
            // 
            this.productNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.productNameDataGridViewTextBoxColumn.DataPropertyName = "ProductName";
            this.productNameDataGridViewTextBoxColumn.HeaderText = "ProductName";
            this.productNameDataGridViewTextBoxColumn.Name = "productNameDataGridViewTextBoxColumn";
            this.productNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // batchTypeIdDataGridViewTextBoxColumn
            // 
            this.batchTypeIdDataGridViewTextBoxColumn.DataPropertyName = "BatchTypeId";
            this.batchTypeIdDataGridViewTextBoxColumn.DataSource = this.batchTypeBindingSource;
            this.batchTypeIdDataGridViewTextBoxColumn.DisplayMember = "Name";
            this.batchTypeIdDataGridViewTextBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.batchTypeIdDataGridViewTextBoxColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.batchTypeIdDataGridViewTextBoxColumn.HeaderText = "BatchTypeId";
            this.batchTypeIdDataGridViewTextBoxColumn.Name = "batchTypeIdDataGridViewTextBoxColumn";
            this.batchTypeIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.batchTypeIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.batchTypeIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.batchTypeIdDataGridViewTextBoxColumn.ValueMember = "Id";
            // 
            // lineIdDataGridViewTextBoxColumn
            // 
            this.lineIdDataGridViewTextBoxColumn.DataPropertyName = "LineId";
            this.lineIdDataGridViewTextBoxColumn.DataSource = this.assembLineBindingSource;
            this.lineIdDataGridViewTextBoxColumn.DisplayMember = "Name";
            this.lineIdDataGridViewTextBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.lineIdDataGridViewTextBoxColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lineIdDataGridViewTextBoxColumn.HeaderText = "LineId";
            this.lineIdDataGridViewTextBoxColumn.Name = "lineIdDataGridViewTextBoxColumn";
            this.lineIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.lineIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.lineIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.lineIdDataGridViewTextBoxColumn.ValueMember = "Id";
            // 
            // nextLineIdDataGridViewTextBoxColumn
            // 
            this.nextLineIdDataGridViewTextBoxColumn.DataPropertyName = "NextLineId";
            this.nextLineIdDataGridViewTextBoxColumn.DataSource = this.assembLineBindingSource;
            this.nextLineIdDataGridViewTextBoxColumn.DisplayMember = "Name";
            this.nextLineIdDataGridViewTextBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.nextLineIdDataGridViewTextBoxColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nextLineIdDataGridViewTextBoxColumn.HeaderText = "NextLineId";
            this.nextLineIdDataGridViewTextBoxColumn.Name = "nextLineIdDataGridViewTextBoxColumn";
            this.nextLineIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.nextLineIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.nextLineIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.nextLineIdDataGridViewTextBoxColumn.ValueMember = "Id";
            // 
            // stateDataGridViewTextBoxColumn
            // 
            this.stateDataGridViewTextBoxColumn.DataPropertyName = "State";
            this.stateDataGridViewTextBoxColumn.HeaderText = "State";
            this.stateDataGridViewTextBoxColumn.Name = "stateDataGridViewTextBoxColumn";
            this.stateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // ProductBuffer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 389);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Name = "ProductBuffer";
            this.Text = "Chassis buffer";
            this.Load += new System.EventHandler(this.ProductBuffer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.batchTypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detroitDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.assembLineBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.assembLineProductBufferBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private DetroitDataSet detroitDataSet;
        private System.Windows.Forms.BindingSource assembLineBindingSource;
        private DetroitDataSetTableAdapters.AssembLineTableAdapter assembLineTableAdapter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.BindingSource assembLineProductBufferBindingSource;
        private DetroitDataSetTableAdapters.ProductBufferTableAdapter productBufferTableAdapter;
        private System.Windows.Forms.BindingSource batchTypeBindingSource;
        private DetroitDataSetTableAdapters.BatchTypeTableAdapter batchTypeTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeRecord;
        private System.Windows.Forms.DataGridViewTextBoxColumn productNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn batchTypeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn lineIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn nextLineIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn stateDataGridViewTextBoxColumn;
    }
}