namespace LogisticTcp
{
    partial class LogisticScreen
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogisticScreen));
            this.paBlack = new System.Windows.Forms.Panel();
            this.dgrTails = new System.Windows.Forms.DataGridView();
            this.Batch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgrRequests = new System.Windows.Forms.DataGridView();
            this.Station = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Parts = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Waiting = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Checked = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paLowRight = new System.Windows.Forms.Panel();
            this.laTime = new System.Windows.Forms.Label();
            this.laMessage = new System.Windows.Forms.Label();
            this.laLineName = new System.Windows.Forms.Label();
            this.laMem = new System.Windows.Forms.Label();
            this.shapeContainer3 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape3 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.paLowLeft = new System.Windows.Forms.Panel();
            this.laNextBatchValue = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.laTaktsTillNextBatchValue = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.laPlanValue = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.laMonthplanValue = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.shapeContainer2 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape2 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.laFrame = new System.Windows.Forms.Label();
            this.laProduct = new System.Windows.Forms.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.shapeContainer4 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape5 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.paBlack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgrTails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgrRequests)).BeginInit();
            this.paLowRight.SuspendLayout();
            this.paLowLeft.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // paBlack
            // 
            this.paBlack.BackColor = System.Drawing.Color.Black;
            this.paBlack.Controls.Add(this.dgrTails);
            this.paBlack.Controls.Add(this.dgrRequests);
            this.paBlack.Controls.Add(this.paLowRight);
            this.paBlack.Controls.Add(this.paLowLeft);
            this.paBlack.Controls.Add(this.panel1);
            this.paBlack.Controls.Add(this.shapeContainer4);
            this.paBlack.Location = new System.Drawing.Point(0, 0);
            this.paBlack.Margin = new System.Windows.Forms.Padding(0);
            this.paBlack.Name = "paBlack";
            this.paBlack.Size = new System.Drawing.Size(960, 540);
            this.paBlack.TabIndex = 27;
            // 
            // dgrTails
            // 
            this.dgrTails.AllowUserToResizeColumns = false;
            this.dgrTails.AllowUserToResizeRows = false;
            this.dgrTails.BackgroundColor = System.Drawing.Color.Black;
            this.dgrTails.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgrTails.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgrTails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgrTails.ColumnHeadersHeight = 40;
            this.dgrTails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgrTails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Batch,
            this.Type,
            this.Tail});
            this.dgrTails.Cursor = System.Windows.Forms.Cursors.Arrow;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgrTails.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgrTails.EnableHeadersVisualStyles = false;
            this.dgrTails.GridColor = System.Drawing.Color.DimGray;
            this.dgrTails.Location = new System.Drawing.Point(496, 92);
            this.dgrTails.MultiSelect = false;
            this.dgrTails.Name = "dgrTails";
            this.dgrTails.ReadOnly = true;
            this.dgrTails.RowHeadersVisible = false;
            this.dgrTails.RowTemplate.Height = 36;
            this.dgrTails.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgrTails.Size = new System.Drawing.Size(452, 251);
            this.dgrTails.TabIndex = 55;
            // 
            // Batch
            // 
            this.Batch.HeaderText = "Batch";
            this.Batch.Name = "Batch";
            this.Batch.ReadOnly = true;
            this.Batch.Width = 160;
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            this.Type.Width = 160;
            // 
            // Tail
            // 
            this.Tail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Tail.HeaderText = "Tail";
            this.Tail.Name = "Tail";
            this.Tail.ReadOnly = true;
            // 
            // dgrRequests
            // 
            this.dgrRequests.AllowUserToResizeColumns = false;
            this.dgrRequests.AllowUserToResizeRows = false;
            this.dgrRequests.BackgroundColor = System.Drawing.Color.Black;
            this.dgrRequests.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgrRequests.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgrRequests.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgrRequests.ColumnHeadersHeight = 40;
            this.dgrRequests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgrRequests.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Station,
            this.Parts,
            this.Waiting,
            this.Checked,
            this.OrderNum});
            this.dgrRequests.Cursor = System.Windows.Forms.Cursors.Arrow;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgrRequests.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgrRequests.EnableHeadersVisualStyles = false;
            this.dgrRequests.GridColor = System.Drawing.Color.DimGray;
            this.dgrRequests.Location = new System.Drawing.Point(12, 92);
            this.dgrRequests.MultiSelect = false;
            this.dgrRequests.Name = "dgrRequests";
            this.dgrRequests.ReadOnly = true;
            this.dgrRequests.RowHeadersVisible = false;
            this.dgrRequests.RowTemplate.Height = 36;
            this.dgrRequests.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgrRequests.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgrRequests.Size = new System.Drawing.Size(466, 334);
            this.dgrRequests.TabIndex = 54;
            this.dgrRequests.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgrRequests_CellDoubleClick);
            // 
            // Station
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            this.Station.DefaultCellStyle = dataGridViewCellStyle4;
            this.Station.HeaderText = "Station";
            this.Station.Name = "Station";
            this.Station.ReadOnly = true;
            // 
            // Parts
            // 
            this.Parts.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Parts.HeaderText = "Parts";
            this.Parts.Name = "Parts";
            this.Parts.ReadOnly = true;
            // 
            // Waiting
            // 
            this.Waiting.HeaderText = "Waiting";
            this.Waiting.Name = "Waiting";
            this.Waiting.ReadOnly = true;
            // 
            // Checked
            // 
            this.Checked.HeaderText = "Chk";
            this.Checked.Name = "Checked";
            this.Checked.ReadOnly = true;
            this.Checked.Width = 80;
            // 
            // OrderNum
            // 
            this.OrderNum.HeaderText = "OrderNum";
            this.OrderNum.Name = "OrderNum";
            this.OrderNum.ReadOnly = true;
            this.OrderNum.Visible = false;
            // 
            // paLowRight
            // 
            this.paLowRight.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.paLowRight.Controls.Add(this.laTime);
            this.paLowRight.Controls.Add(this.laMessage);
            this.paLowRight.Controls.Add(this.laLineName);
            this.paLowRight.Controls.Add(this.laMem);
            this.paLowRight.Controls.Add(this.shapeContainer3);
            this.paLowRight.ForeColor = System.Drawing.Color.White;
            this.paLowRight.Location = new System.Drawing.Point(481, 360);
            this.paLowRight.Name = "paLowRight";
            this.paLowRight.Size = new System.Drawing.Size(480, 180);
            this.paLowRight.TabIndex = 45;
            // 
            // laTime
            // 
            this.laTime.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laTime.ForeColor = System.Drawing.Color.White;
            this.laTime.Location = new System.Drawing.Point(372, 12);
            this.laTime.Name = "laTime";
            this.laTime.Size = new System.Drawing.Size(95, 30);
            this.laTime.TabIndex = 53;
            this.laTime.Text = "00:00:00";
            this.laTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.laTime.DoubleClick += new System.EventHandler(this.laTime_DoubleClick_1);
            // 
            // laMessage
            // 
            this.laMessage.AutoSize = true;
            this.laMessage.Font = new System.Drawing.Font("Lucida Console", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laMessage.ForeColor = System.Drawing.Color.Tomato;
            this.laMessage.Location = new System.Drawing.Point(115, 137);
            this.laMessage.Name = "laMessage";
            this.laMessage.Size = new System.Drawing.Size(236, 27);
            this.laMessage.TabIndex = 35;
            this.laMessage.Text = "Connecting ...";
            // 
            // laLineName
            // 
            this.laLineName.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laLineName.ForeColor = System.Drawing.Color.White;
            this.laLineName.Location = new System.Drawing.Point(15, 4);
            this.laLineName.Name = "laLineName";
            this.laLineName.Size = new System.Drawing.Size(452, 30);
            this.laLineName.TabIndex = 52;
            this.laLineName.Text = "Tact time left";
            this.laLineName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // laMem
            // 
            this.laMem.Font = new System.Drawing.Font("Calibri", 65.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laMem.ForeColor = System.Drawing.Color.White;
            this.laMem.Location = new System.Drawing.Point(36, 42);
            this.laMem.Name = "laMem";
            this.laMem.Size = new System.Drawing.Size(403, 106);
            this.laMem.TabIndex = 26;
            this.laMem.Text = "45:00";
            this.laMem.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // shapeContainer3
            // 
            this.shapeContainer3.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer3.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer3.Name = "shapeContainer3";
            this.shapeContainer3.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape3});
            this.shapeContainer3.Size = new System.Drawing.Size(480, 180);
            this.shapeContainer3.TabIndex = 0;
            this.shapeContainer3.TabStop = false;
            // 
            // lineShape3
            // 
            this.lineShape3.BorderColor = System.Drawing.Color.White;
            this.lineShape3.Name = "lineShape1";
            this.lineShape3.X1 = 0;
            this.lineShape3.X2 = 480;
            this.lineShape3.Y1 = 0;
            this.lineShape3.Y2 = 0;
            // 
            // paLowLeft
            // 
            this.paLowLeft.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.paLowLeft.Controls.Add(this.laNextBatchValue);
            this.paLowLeft.Controls.Add(this.label5);
            this.paLowLeft.Controls.Add(this.laTaktsTillNextBatchValue);
            this.paLowLeft.Controls.Add(this.label7);
            this.paLowLeft.Controls.Add(this.laPlanValue);
            this.paLowLeft.Controls.Add(this.label3);
            this.paLowLeft.Controls.Add(this.laMonthplanValue);
            this.paLowLeft.Controls.Add(this.label4);
            this.paLowLeft.Controls.Add(this.shapeContainer2);
            this.paLowLeft.ForeColor = System.Drawing.Color.White;
            this.paLowLeft.Location = new System.Drawing.Point(0, 459);
            this.paLowLeft.Name = "paLowLeft";
            this.paLowLeft.Size = new System.Drawing.Size(480, 81);
            this.paLowLeft.TabIndex = 40;
            // 
            // laNextBatchValue
            // 
            this.laNextBatchValue.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laNextBatchValue.ForeColor = System.Drawing.Color.White;
            this.laNextBatchValue.Location = new System.Drawing.Point(351, 6);
            this.laNextBatchValue.Name = "laNextBatchValue";
            this.laNextBatchValue.Size = new System.Drawing.Size(101, 30);
            this.laNextBatchValue.TabIndex = 59;
            this.laNextBatchValue.Text = "-1";
            this.laNextBatchValue.UseWaitCursor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(222, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 30);
            this.label5.TabIndex = 57;
            this.label5.Text = "Tacts till:";
            // 
            // laTaktsTillNextBatchValue
            // 
            this.laTaktsTillNextBatchValue.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laTaktsTillNextBatchValue.ForeColor = System.Drawing.Color.White;
            this.laTaktsTillNextBatchValue.Location = new System.Drawing.Point(351, 38);
            this.laTaktsTillNextBatchValue.Name = "laTaktsTillNextBatchValue";
            this.laTaktsTillNextBatchValue.Size = new System.Drawing.Size(101, 30);
            this.laTaktsTillNextBatchValue.TabIndex = 58;
            this.laTaktsTillNextBatchValue.Text = "-2";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(222, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(124, 30);
            this.label7.TabIndex = 56;
            this.label7.Text = "Next batch:";
            // 
            // laPlanValue
            // 
            this.laPlanValue.AutoSize = true;
            this.laPlanValue.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laPlanValue.ForeColor = System.Drawing.Color.White;
            this.laPlanValue.Location = new System.Drawing.Point(141, 6);
            this.laPlanValue.Name = "laPlanValue";
            this.laPlanValue.Size = new System.Drawing.Size(33, 30);
            this.laPlanValue.TabIndex = 55;
            this.laPlanValue.Text = "-1";
            this.laPlanValue.UseWaitCursor = true;
            this.laPlanValue.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(12, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 30);
            this.label3.TabIndex = 53;
            this.label3.Text = "Month plan:";
            this.label3.Visible = false;
            // 
            // laMonthplanValue
            // 
            this.laMonthplanValue.AutoSize = true;
            this.laMonthplanValue.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laMonthplanValue.ForeColor = System.Drawing.Color.White;
            this.laMonthplanValue.Location = new System.Drawing.Point(141, 38);
            this.laMonthplanValue.Name = "laMonthplanValue";
            this.laMonthplanValue.Size = new System.Drawing.Size(33, 30);
            this.laMonthplanValue.TabIndex = 54;
            this.laMonthplanValue.Text = "-2";
            this.laMonthplanValue.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(12, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 30);
            this.label4.TabIndex = 52;
            this.label4.Text = "Plan:";
            this.label4.Visible = false;
            // 
            // shapeContainer2
            // 
            this.shapeContainer2.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer2.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer2.Name = "shapeContainer2";
            this.shapeContainer2.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape2});
            this.shapeContainer2.Size = new System.Drawing.Size(480, 81);
            this.shapeContainer2.TabIndex = 0;
            this.shapeContainer2.TabStop = false;
            // 
            // lineShape2
            // 
            this.lineShape2.BorderColor = System.Drawing.Color.White;
            this.lineShape2.Name = "lineShape1";
            this.lineShape2.X1 = 0;
            this.lineShape2.X2 = 480;
            this.lineShape2.Y1 = 0;
            this.lineShape2.Y2 = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.laFrame);
            this.panel1.Controls.Add(this.laProduct);
            this.panel1.Controls.Add(this.shapeContainer1);
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(960, 86);
            this.panel1.TabIndex = 39;
            this.panel1.DoubleClick += new System.EventHandler(this.panel1_DoubleClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::LogisticTcp.Properties.Resources.scania_logo_bw;
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(17, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(220, 58);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 54;
            this.pictureBox1.TabStop = false;
            // 
            // laFrame
            // 
            this.laFrame.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laFrame.ForeColor = System.Drawing.Color.White;
            this.laFrame.Location = new System.Drawing.Point(435, 39);
            this.laFrame.Name = "laFrame";
            this.laFrame.Size = new System.Drawing.Size(519, 30);
            this.laFrame.TabIndex = 51;
            this.laFrame.Text = "Frame name";
            this.laFrame.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // laProduct
            // 
            this.laProduct.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laProduct.ForeColor = System.Drawing.Color.White;
            this.laProduct.Location = new System.Drawing.Point(284, 9);
            this.laProduct.Name = "laProduct";
            this.laProduct.Size = new System.Drawing.Size(664, 30);
            this.laProduct.TabIndex = 33;
            this.laProduct.Text = "Batch: NA";
            this.laProduct.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(960, 86);
            this.shapeContainer1.TabIndex = 0;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.BorderColor = System.Drawing.Color.White;
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 0;
            this.lineShape1.X2 = 960;
            this.lineShape1.Y1 = 83;
            this.lineShape1.Y2 = 83;
            // 
            // shapeContainer4
            // 
            this.shapeContainer4.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer4.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer4.Name = "shapeContainer4";
            this.shapeContainer4.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape5});
            this.shapeContainer4.Size = new System.Drawing.Size(960, 540);
            this.shapeContainer4.TabIndex = 46;
            this.shapeContainer4.TabStop = false;
            // 
            // lineShape5
            // 
            this.lineShape5.BorderColor = System.Drawing.Color.White;
            this.lineShape5.Name = "lineShape5";
            this.lineShape5.X1 = 480;
            this.lineShape5.X2 = 480;
            this.lineShape5.Y1 = 553;
            this.lineShape5.Y2 = 84;
            // 
            // LogisticScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 540);
            this.Controls.Add(this.paBlack);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LogisticScreen";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.LogisticScreen_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlackScreen_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BlackScreen_MouseMove);
            this.paBlack.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgrTails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgrRequests)).EndInit();
            this.paLowRight.ResumeLayout(false);
            this.paLowRight.PerformLayout();
            this.paLowLeft.ResumeLayout(false);
            this.paLowLeft.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel paBlack;
        private System.Windows.Forms.Panel paLowRight;
        private System.Windows.Forms.Label laFrame;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer3;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape3;
        private System.Windows.Forms.Panel paLowLeft;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer2;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label laProduct;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private System.Windows.Forms.Label laMessage;
        private System.Windows.Forms.Label laMem;
        private System.Windows.Forms.Label laLineName;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer4;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape5;
        private System.Windows.Forms.Label laPlanValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label laMonthplanValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgrRequests;
        private System.Windows.Forms.DataGridView dgrTails;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Batch;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tail;
        private System.Windows.Forms.DataGridViewTextBoxColumn Station;
        private System.Windows.Forms.DataGridViewTextBoxColumn Parts;
        private System.Windows.Forms.DataGridViewTextBoxColumn Waiting;
        private System.Windows.Forms.DataGridViewTextBoxColumn Checked;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderNum;
        private System.Windows.Forms.Label laNextBatchValue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label laTaktsTillNextBatchValue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label laTime;
    }
}

