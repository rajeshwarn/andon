namespace MediaPanel
{
    partial class ConrolForm
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
            this.tbLineId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btConnect = new System.Windows.Forms.Button();
            this.btDisconnect = new System.Windows.Forms.Button();
            this.tbURL = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbFile = new System.Windows.Forms.TextBox();
            this.rbModeVideo = new System.Windows.Forms.RadioButton();
            this.rbModePic = new System.Windows.Forms.RadioButton();
            this.rbModeAndon = new System.Windows.Forms.RadioButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btPlay = new System.Windows.Forms.Button();
            this.laConnectionState = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ServerUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contentAdressListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contentAdressListBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tbLineId
            // 
            this.tbLineId.Location = new System.Drawing.Point(12, 32);
            this.tbLineId.Name = "tbLineId";
            this.tbLineId.Size = new System.Drawing.Size(77, 22);
            this.tbLineId.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Line No:";
            // 
            // btConnect
            // 
            this.btConnect.Location = new System.Drawing.Point(98, 30);
            this.btConnect.Name = "btConnect";
            this.btConnect.Size = new System.Drawing.Size(108, 28);
            this.btConnect.TabIndex = 2;
            this.btConnect.Text = "Connect";
            this.btConnect.UseVisualStyleBackColor = true;
            this.btConnect.Click += new System.EventHandler(this.btConnect_Click);
            // 
            // btDisconnect
            // 
            this.btDisconnect.Enabled = false;
            this.btDisconnect.Location = new System.Drawing.Point(212, 30);
            this.btDisconnect.Name = "btDisconnect";
            this.btDisconnect.Size = new System.Drawing.Size(108, 28);
            this.btDisconnect.TabIndex = 3;
            this.btDisconnect.Text = "Dicsonnect";
            this.btDisconnect.UseVisualStyleBackColor = true;
            this.btDisconnect.Click += new System.EventHandler(this.btDisconnect_Click);
            // 
            // tbURL
            // 
            this.tbURL.Location = new System.Drawing.Point(12, 107);
            this.tbURL.Name = "tbURL";
            this.tbURL.Size = new System.Drawing.Size(595, 22);
            this.tbURL.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(371, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Media server URL (example: \"http://localhost:8080/files/\"):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(236, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Media file (example: \"myVideo.avi\") :";
            // 
            // tbFile
            // 
            this.tbFile.Location = new System.Drawing.Point(12, 157);
            this.tbFile.Name = "tbFile";
            this.tbFile.Size = new System.Drawing.Size(595, 22);
            this.tbFile.TabIndex = 6;
            // 
            // rbModeVideo
            // 
            this.rbModeVideo.AutoSize = true;
            this.rbModeVideo.Location = new System.Drawing.Point(12, 199);
            this.rbModeVideo.Name = "rbModeVideo";
            this.rbModeVideo.Size = new System.Drawing.Size(65, 21);
            this.rbModeVideo.TabIndex = 8;
            this.rbModeVideo.Text = "Video\r\n";
            this.rbModeVideo.UseVisualStyleBackColor = true;
            this.rbModeVideo.CheckedChanged += new System.EventHandler(this.rbModeVideo_CheckedChanged);
            // 
            // rbModePic
            // 
            this.rbModePic.AutoSize = true;
            this.rbModePic.Location = new System.Drawing.Point(83, 199);
            this.rbModePic.Name = "rbModePic";
            this.rbModePic.Size = new System.Drawing.Size(73, 21);
            this.rbModePic.TabIndex = 9;
            this.rbModePic.Text = "Picture";
            this.rbModePic.UseVisualStyleBackColor = true;
            this.rbModePic.CheckedChanged += new System.EventHandler(this.rbModePic_CheckedChanged);
            // 
            // rbModeAndon
            // 
            this.rbModeAndon.AutoSize = true;
            this.rbModeAndon.Checked = true;
            this.rbModeAndon.Location = new System.Drawing.Point(162, 199);
            this.rbModeAndon.Name = "rbModeAndon";
            this.rbModeAndon.Size = new System.Drawing.Size(102, 21);
            this.rbModeAndon.TabIndex = 10;
            this.rbModeAndon.TabStop = true;
            this.rbModeAndon.Text = "Andon Takt";
            this.rbModeAndon.UseVisualStyleBackColor = true;
            this.rbModeAndon.CheckedChanged += new System.EventHandler(this.rbModeAndon_CheckedChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fileNameDataGridViewTextBoxColumn,
            this.ServerUrl});
            this.dataGridView1.DataSource = this.contentAdressListBindingSource;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.dataGridView1.Location = new System.Drawing.Point(12, 279);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(595, 139);
            this.dataGridView1.TabIndex = 14;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // btPlay
            // 
            this.btPlay.Enabled = false;
            this.btPlay.Location = new System.Drawing.Point(279, 195);
            this.btPlay.Name = "btPlay";
            this.btPlay.Size = new System.Drawing.Size(102, 28);
            this.btPlay.TabIndex = 15;
            this.btPlay.Text = "PLAY  >>>";
            this.btPlay.UseVisualStyleBackColor = true;
            this.btPlay.Click += new System.EventHandler(this.btPlay_Click);
            // 
            // laConnectionState
            // 
            this.laConnectionState.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laConnectionState.ForeColor = System.Drawing.Color.LightGray;
            this.laConnectionState.Location = new System.Drawing.Point(472, 35);
            this.laConnectionState.Name = "laConnectionState";
            this.laConnectionState.Size = new System.Drawing.Size(135, 17);
            this.laConnectionState.TabIndex = 16;
            this.laConnectionState.Text = "No connection";
            this.laConnectionState.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 420);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(382, 17);
            this.label4.TabIndex = 17;
            this.label4.Text = "Tip: You may add (Ins), edit (F2) or delete (Del) these links.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 257);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(451, 17);
            this.label5.TabIndex = 18;
            this.label5.Text = "Saved content links (Doudle-click to use selected media. Press PLAY):";
            // 
            // ServerUrl
            // 
            this.ServerUrl.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ServerUrl.DataPropertyName = "ServerUrl";
            this.ServerUrl.HeaderText = "Media server URL";
            this.ServerUrl.Name = "ServerUrl";
            // 
            // fileNameDataGridViewTextBoxColumn
            // 
            this.fileNameDataGridViewTextBoxColumn.DataPropertyName = "FileName";
            this.fileNameDataGridViewTextBoxColumn.HeaderText = "Media file";
            this.fileNameDataGridViewTextBoxColumn.Name = "fileNameDataGridViewTextBoxColumn";
            this.fileNameDataGridViewTextBoxColumn.Width = 150;
            // 
            // contentAdressListBindingSource
            // 
            this.contentAdressListBindingSource.DataSource = typeof(MediaPanel.ContentAdressList);
            // 
            // ConrolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 455);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.laConnectionState);
            this.Controls.Add(this.btPlay);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.rbModeAndon);
            this.Controls.Add(this.rbModePic);
            this.Controls.Add(this.rbModeVideo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbURL);
            this.Controls.Add(this.btDisconnect);
            this.Controls.Add(this.btConnect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbLineId);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ConrolForm";
            this.Text = "Media Panel";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConrolForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contentAdressListBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbLineId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btConnect;
        private System.Windows.Forms.Button btDisconnect;
        private System.Windows.Forms.TextBox tbURL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbFile;
        private System.Windows.Forms.RadioButton rbModeVideo;
        private System.Windows.Forms.RadioButton rbModePic;
        private System.Windows.Forms.RadioButton rbModeAndon;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btPlay;
        public System.Windows.Forms.BindingSource contentAdressListBindingSource;
        private System.Windows.Forms.Label laConnectionState;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServerUrl;
    }
}