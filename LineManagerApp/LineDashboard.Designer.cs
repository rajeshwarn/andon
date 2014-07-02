namespace LineManagerApp
{
    partial class LineDashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LineDashboard));
            this.cbOPCMode = new System.Windows.Forms.CheckBox();
            this.assembLineTableAdapter = new LineManagerApp.DataSet1TableAdapters.AssembLineTableAdapter();
            this.dataSet11 = new LineManagerApp.DataSet1();
            this.laMessage = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.setCurrentTaktToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setTaktToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToAutoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testMemLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gbxControlPanel = new System.Windows.Forms.GroupBox();
            this.laFreezedTakt = new System.Windows.Forms.Label();
            this.btnLogisticScreen = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.laFrameValue = new System.Windows.Forms.Label();
            this.laFrame = new System.Windows.Forms.Label();
            this.laTimeValue = new System.Windows.Forms.Label();
            this.laTime = new System.Windows.Forms.Label();
            this.laLineName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLineMove = new System.Windows.Forms.Button();
            this.laTakt = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnLineStop = new System.Windows.Forms.Button();
            this.btnLineStart = new System.Windows.Forms.Button();
            this.laSumStopValue = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.laPlanValue = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.paColorPanel = new System.Windows.Forms.Panel();
            this.laLive = new System.Windows.Forms.Label();
            this.laPlanFakt = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.logMessageBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataSet11)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.gbxControlPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.paColorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logMessageBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // cbOPCMode
            // 
            this.cbOPCMode.AutoSize = true;
            this.cbOPCMode.Checked = true;
            this.cbOPCMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOPCMode.Location = new System.Drawing.Point(895, 46);
            this.cbOPCMode.Margin = new System.Windows.Forms.Padding(4);
            this.cbOPCMode.Name = "cbOPCMode";
            this.cbOPCMode.Size = new System.Drawing.Size(98, 21);
            this.cbOPCMode.TabIndex = 35;
            this.cbOPCMode.Text = "OPC Mode";
            this.cbOPCMode.UseVisualStyleBackColor = true;
            this.cbOPCMode.Visible = false;
            this.cbOPCMode.CheckedChanged += new System.EventHandler(this.cbOPCMode_CheckedChanged);
            // 
            // assembLineTableAdapter
            // 
            this.assembLineTableAdapter.ClearBeforeFill = true;
            // 
            // dataSet11
            // 
            this.dataSet11.DataSetName = "DataSet1";
            this.dataSet11.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // laMessage
            // 
            this.laMessage.AutoSize = true;
            this.laMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laMessage.ForeColor = System.Drawing.Color.Red;
            this.laMessage.Location = new System.Drawing.Point(431, 122);
            this.laMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laMessage.Name = "laMessage";
            this.laMessage.Size = new System.Drawing.Size(17, 17);
            this.laMessage.TabIndex = 38;
            this.laMessage.Text = "_";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.toolStripButton2,
            this.toolStripSeparator2,
            this.toolStripLabel1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1043, 27);
            this.toolStrip1.TabIndex = 39;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(78, 24);
            this.toolStripButton1.Text = "Save view";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(97, 24);
            this.toolStripButton2.Text = "Restore view";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setCurrentTaktToolStripMenuItem,
            this.setTaktToolStripMenuItem,
            this.resetToAutoToolStripMenuItem,
            this.testMemLogToolStripMenuItem});
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(102, 24);
            this.toolStripLabel1.Text = "Takt timer ...";
            // 
            // setCurrentTaktToolStripMenuItem
            // 
            this.setCurrentTaktToolStripMenuItem.Name = "setCurrentTaktToolStripMenuItem";
            this.setCurrentTaktToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.setCurrentTaktToolStripMenuItem.Text = "Set current takt ...";
            this.setCurrentTaktToolStripMenuItem.Click += new System.EventHandler(this.setCurrentTaktToolStripMenuItem_Click);
            // 
            // setTaktToolStripMenuItem
            // 
            this.setTaktToolStripMenuItem.Name = "setTaktToolStripMenuItem";
            this.setTaktToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.setTaktToolStripMenuItem.Text = "Set takt permanently ...";
            this.setTaktToolStripMenuItem.Click += new System.EventHandler(this.setTaktToolStripMenuItem_Click);
            // 
            // resetToAutoToolStripMenuItem
            // 
            this.resetToAutoToolStripMenuItem.Name = "resetToAutoToolStripMenuItem";
            this.resetToAutoToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.resetToAutoToolStripMenuItem.Text = "Reset to auto";
            this.resetToAutoToolStripMenuItem.Click += new System.EventHandler(this.resetToAutoToolStripMenuItem_Click);
            // 
            // testMemLogToolStripMenuItem
            // 
            this.testMemLogToolStripMenuItem.Name = "testMemLogToolStripMenuItem";
            this.testMemLogToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.testMemLogToolStripMenuItem.Text = "Test MemLog";
            this.testMemLogToolStripMenuItem.Click += new System.EventHandler(this.testMemLogToolStripMenuItem_Click);
            // 
            // gbxControlPanel
            // 
            this.gbxControlPanel.Controls.Add(this.laFreezedTakt);
            this.gbxControlPanel.Controls.Add(this.btnLogisticScreen);
            this.gbxControlPanel.Controls.Add(this.button8);
            this.gbxControlPanel.Controls.Add(this.laFrameValue);
            this.gbxControlPanel.Controls.Add(this.laFrame);
            this.gbxControlPanel.Controls.Add(this.laTimeValue);
            this.gbxControlPanel.Controls.Add(this.laTime);
            this.gbxControlPanel.Controls.Add(this.laLineName);
            this.gbxControlPanel.Controls.Add(this.label1);
            this.gbxControlPanel.Controls.Add(this.laMessage);
            this.gbxControlPanel.Controls.Add(this.btnLineMove);
            this.gbxControlPanel.Controls.Add(this.laTakt);
            this.gbxControlPanel.Controls.Add(this.label7);
            this.gbxControlPanel.Controls.Add(this.btnLineStop);
            this.gbxControlPanel.Controls.Add(this.btnLineStart);
            this.gbxControlPanel.Location = new System.Drawing.Point(16, 113);
            this.gbxControlPanel.Margin = new System.Windows.Forms.Padding(4);
            this.gbxControlPanel.Name = "gbxControlPanel";
            this.gbxControlPanel.Padding = new System.Windows.Forms.Padding(4);
            this.gbxControlPanel.Size = new System.Drawing.Size(820, 150);
            this.gbxControlPanel.TabIndex = 40;
            this.gbxControlPanel.TabStop = false;
            this.gbxControlPanel.Text = "Control panel";
            // 
            // laFreezedTakt
            // 
            this.laFreezedTakt.AutoSize = true;
            this.laFreezedTakt.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laFreezedTakt.ForeColor = System.Drawing.Color.Black;
            this.laFreezedTakt.Location = new System.Drawing.Point(431, 102);
            this.laFreezedTakt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laFreezedTakt.Name = "laFreezedTakt";
            this.laFreezedTakt.Size = new System.Drawing.Size(46, 17);
            this.laFreezedTakt.TabIndex = 55;
            this.laFreezedTakt.Text = "(auto)";
            // 
            // btnLogisticScreen
            // 
            this.btnLogisticScreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnLogisticScreen.Location = new System.Drawing.Point(600, 94);
            this.btnLogisticScreen.Margin = new System.Windows.Forms.Padding(4);
            this.btnLogisticScreen.Name = "btnLogisticScreen";
            this.btnLogisticScreen.Size = new System.Drawing.Size(141, 39);
            this.btnLogisticScreen.TabIndex = 54;
            this.btnLogisticScreen.Text = "Logistic Screen";
            this.btnLogisticScreen.UseVisualStyleBackColor = true;
            this.btnLogisticScreen.Click += new System.EventHandler(this.btnLogisticScreen_Click);
            // 
            // button8
            // 
            this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button8.Location = new System.Drawing.Point(181, 94);
            this.button8.Margin = new System.Windows.Forms.Padding(4);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(76, 39);
            this.button8.TabIndex = 53;
            this.button8.Text = "Reset";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // laFrameValue
            // 
            this.laFrameValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laFrameValue.Location = new System.Drawing.Point(663, 62);
            this.laFrameValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laFrameValue.Name = "laFrameValue";
            this.laFrameValue.Size = new System.Drawing.Size(147, 16);
            this.laFrameValue.TabIndex = 52;
            this.laFrameValue.Text = ".";
            this.laFrameValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // laFrame
            // 
            this.laFrame.AutoSize = true;
            this.laFrame.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laFrame.Location = new System.Drawing.Point(596, 62);
            this.laFrame.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laFrame.Name = "laFrame";
            this.laFrame.Size = new System.Drawing.Size(52, 17);
            this.laFrame.TabIndex = 51;
            this.laFrame.Text = "Frame:";
            // 
            // laTimeValue
            // 
            this.laTimeValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laTimeValue.Location = new System.Drawing.Point(663, 38);
            this.laTimeValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laTimeValue.Name = "laTimeValue";
            this.laTimeValue.Size = new System.Drawing.Size(109, 16);
            this.laTimeValue.TabIndex = 46;
            this.laTimeValue.Text = "00:00:00";
            this.laTimeValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // laTime
            // 
            this.laTime.AutoSize = true;
            this.laTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laTime.Location = new System.Drawing.Point(604, 38);
            this.laTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laTime.Name = "laTime";
            this.laTime.Size = new System.Drawing.Size(43, 17);
            this.laTime.TabIndex = 45;
            this.laTime.Text = "Time:";
            // 
            // laLineName
            // 
            this.laLineName.AutoSize = true;
            this.laLineName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laLineName.Location = new System.Drawing.Point(9, 62);
            this.laLineName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laLineName.Name = "laLineName";
            this.laLineName.Size = new System.Drawing.Size(26, 17);
            this.laLineName.TabIndex = 44;
            this.laLineName.Text = "---";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(9, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 17);
            this.label1.TabIndex = 43;
            this.label1.Text = "Assembly line name: ";
            // 
            // btnLineMove
            // 
            this.btnLineMove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnLineMove.Location = new System.Drawing.Point(60, 65);
            this.btnLineMove.Margin = new System.Windows.Forms.Padding(4);
            this.btnLineMove.Name = "btnLineMove";
            this.btnLineMove.Size = new System.Drawing.Size(76, 39);
            this.btnLineMove.TabIndex = 42;
            this.btnLineMove.Text = "Move";
            this.btnLineMove.UseVisualStyleBackColor = true;
            this.btnLineMove.Visible = false;
            this.btnLineMove.Click += new System.EventHandler(this.btnLineMove_Click);
            // 
            // laTakt
            // 
            this.laTakt.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laTakt.Location = new System.Drawing.Point(337, 54);
            this.laTakt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laTakt.Name = "laTakt";
            this.laTakt.Size = new System.Drawing.Size(188, 46);
            this.laTakt.TabIndex = 41;
            this.laTakt.Text = "00:00";
            this.laTakt.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.laTakt.DoubleClick += new System.EventHandler(this.laTakt_DoubleClick);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(424, 38);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 17);
            this.label7.TabIndex = 40;
            this.label7.Text = "Tact time:";
            // 
            // btnLineStop
            // 
            this.btnLineStop.BackColor = System.Drawing.Color.RosyBrown;
            this.btnLineStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnLineStop.Location = new System.Drawing.Point(97, 92);
            this.btnLineStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnLineStop.Name = "btnLineStop";
            this.btnLineStop.Size = new System.Drawing.Size(76, 41);
            this.btnLineStop.TabIndex = 39;
            this.btnLineStop.Text = "Stop";
            this.btnLineStop.UseVisualStyleBackColor = false;
            this.btnLineStop.Click += new System.EventHandler(this.btnLineStop_Click);
            // 
            // btnLineStart
            // 
            this.btnLineStart.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.btnLineStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnLineStart.Location = new System.Drawing.Point(13, 92);
            this.btnLineStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnLineStart.Name = "btnLineStart";
            this.btnLineStart.Size = new System.Drawing.Size(76, 41);
            this.btnLineStart.TabIndex = 38;
            this.btnLineStart.Text = "Start";
            this.btnLineStart.UseVisualStyleBackColor = false;
            this.btnLineStart.Click += new System.EventHandler(this.btnLineStart_Click);
            // 
            // laSumStopValue
            // 
            this.laSumStopValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laSumStopValue.Location = new System.Drawing.Point(457, 73);
            this.laSumStopValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laSumStopValue.Name = "laSumStopValue";
            this.laSumStopValue.Size = new System.Drawing.Size(80, 16);
            this.laSumStopValue.TabIndex = 50;
            this.laSumStopValue.Text = "00:00";
            this.laSumStopValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.laSumStopValue.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(371, 73);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 17);
            this.label9.TabIndex = 49;
            this.label9.Text = "Sum stop:";
            this.label9.Visible = false;
            // 
            // laPlanValue
            // 
            this.laPlanValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laPlanValue.Location = new System.Drawing.Point(457, 50);
            this.laPlanValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laPlanValue.Name = "laPlanValue";
            this.laPlanValue.Size = new System.Drawing.Size(80, 16);
            this.laPlanValue.TabIndex = 48;
            this.laPlanValue.Text = "-1";
            this.laPlanValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.laPlanValue.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(401, 50);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 17);
            this.label6.TabIndex = 47;
            this.label6.Text = "Plan:";
            this.label6.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.paColorPanel);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(9, 298);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(7, 15, 7, 6);
            this.groupBox1.Size = new System.Drawing.Size(300, 175);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "FA1";
            this.groupBox1.Visible = false;
            // 
            // paColorPanel
            // 
            this.paColorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.paColorPanel.AutoSize = true;
            this.paColorPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.paColorPanel.Controls.Add(this.laLive);
            this.paColorPanel.Controls.Add(this.laPlanFakt);
            this.paColorPanel.Controls.Add(this.button7);
            this.paColorPanel.Controls.Add(this.button4);
            this.paColorPanel.Controls.Add(this.button5);
            this.paColorPanel.Controls.Add(this.button6);
            this.paColorPanel.Controls.Add(this.button3);
            this.paColorPanel.Controls.Add(this.button2);
            this.paColorPanel.Controls.Add(this.button1);
            this.paColorPanel.Controls.Add(this.label4);
            this.paColorPanel.Controls.Add(this.listBox1);
            this.paColorPanel.Controls.Add(this.textBox1);
            this.paColorPanel.Controls.Add(this.label3);
            this.paColorPanel.Controls.Add(this.label2);
            this.paColorPanel.Location = new System.Drawing.Point(7, 15);
            this.paColorPanel.Margin = new System.Windows.Forms.Padding(4);
            this.paColorPanel.Name = "paColorPanel";
            this.paColorPanel.Size = new System.Drawing.Size(287, 154);
            this.paColorPanel.TabIndex = 0;
            // 
            // laLive
            // 
            this.laLive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.laLive.AutoSize = true;
            this.laLive.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laLive.Location = new System.Drawing.Point(191, 126);
            this.laLive.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laLive.Name = "laLive";
            this.laLive.Size = new System.Drawing.Size(32, 17);
            this.laLive.TabIndex = 13;
            this.laLive.Text = "000";
            this.laLive.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // laPlanFakt
            // 
            this.laPlanFakt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.laPlanFakt.AutoSize = true;
            this.laPlanFakt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laPlanFakt.Location = new System.Drawing.Point(191, 10);
            this.laPlanFakt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laPlanFakt.Name = "laPlanFakt";
            this.laPlanFakt.Size = new System.Drawing.Size(73, 17);
            this.laPlanFakt.TabIndex = 12;
            this.laPlanFakt.Text = "P/F: 99/99";
            this.laPlanFakt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.SystemColors.Control;
            this.button7.Location = new System.Drawing.Point(240, 31);
            this.button7.Margin = new System.Windows.Forms.Padding(4);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(31, 28);
            this.button7.TabIndex = 11;
            this.button7.Text = "P";
            this.button7.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.LightCoral;
            this.button4.Location = new System.Drawing.Point(99, 113);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(31, 28);
            this.button4.TabIndex = 10;
            this.button4.Text = "H";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.Tomato;
            this.button5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button5.Location = new System.Drawing.Point(60, 113);
            this.button5.Margin = new System.Windows.Forms.Padding(4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(31, 28);
            this.button5.TabIndex = 9;
            this.button5.Text = "S";
            this.button5.UseVisualStyleBackColor = false;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.White;
            this.button6.Location = new System.Drawing.Point(21, 113);
            this.button6.Margin = new System.Windows.Forms.Padding(4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(31, 28);
            this.button6.TabIndex = 8;
            this.button6.Text = "F";
            this.button6.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Gold;
            this.button3.Location = new System.Drawing.Point(99, 79);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(31, 28);
            this.button3.TabIndex = 7;
            this.button3.Text = "H";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Tomato;
            this.button2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button2.Location = new System.Drawing.Point(60, 79);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(31, 28);
            this.button2.TabIndex = 6;
            this.button2.Text = "S";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Lime;
            this.button1.Location = new System.Drawing.Point(21, 79);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(31, 28);
            this.button1.TabIndex = 5;
            this.button1.Text = "F";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(153, 91);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 17);
            this.label4.TabIndex = 4;
            this.label4.Text = "Tail: 34001";
            // 
            // listBox1
            // 
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBox1.ItemHeight = 17;
            this.listBox1.Location = new System.Drawing.Point(157, 31);
            this.listBox1.Margin = new System.Windows.Forms.Padding(4);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listBox1.Size = new System.Drawing.Size(113, 36);
            this.listBox1.TabIndex = 3;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 31);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(112, 23);
            this.textBox1.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(155, 10);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Buffer";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(12, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Product";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::LineManagerApp.Properties.Resources.scania_logo_gr1;
            this.pictureBox1.Location = new System.Drawing.Point(16, 34);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(293, 71);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 42;
            this.pictureBox1.TabStop = false;
            // 
            // logMessageBindingSource
            // 
            this.logMessageBindingSource.DataSource = typeof(LineManagerApp.LineServiceReference.LogMessage);
            // 
            // LineDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1043, 752);
            this.Controls.Add(this.gbxControlPanel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.laSumStopValue);
            this.Controls.Add(this.cbOPCMode);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.laPlanValue);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LineDashboard";
            this.Text = "Line control panel";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LineDashboard_FormClosing);
            this.Load += new System.EventHandler(this.LineDashboard_Load);
            this.Shown += new System.EventHandler(this.LineDashboard_Shown);
            this.ResizeEnd += new System.EventHandler(this.LineDashboard_ResizeEnd);
            this.LocationChanged += new System.EventHandler(this.LineDashboard_LocationChanged);
            this.RegionChanged += new System.EventHandler(this.LineDashboard_RegionChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dataSet11)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.gbxControlPanel.ResumeLayout(false);
            this.gbxControlPanel.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.paColorPanel.ResumeLayout(false);
            this.paColorPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logMessageBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private DetroitDataSet detroitDataSet1;
        //private DetroitDataSetTableAdapters.AssembLineTableAdapter assembLineTableAdapter1;
        private System.Windows.Forms.CheckBox cbOPCMode;
        private DataSet1TableAdapters.AssembLineTableAdapter assembLineTableAdapter;
        private DataSet1 dataSet11;
        private System.Windows.Forms.Label laMessage;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.GroupBox gbxControlPanel;
        private System.Windows.Forms.Label laLineName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLineMove;
        private System.Windows.Forms.Label laTakt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnLineStop;
        private System.Windows.Forms.Button btnLineStart;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel paColorPanel;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label laSumStopValue;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label laPlanValue;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label laTimeValue;
        private System.Windows.Forms.Label laTime;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label laFrameValue;
        private System.Windows.Forms.Label laFrame;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Label laPlanFakt;
        private System.Windows.Forms.Button btnLogisticScreen;
        private System.Windows.Forms.Label laLive;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripDropDownButton toolStripLabel1;
        private System.Windows.Forms.ToolStripMenuItem setCurrentTaktToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setTaktToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToAutoToolStripMenuItem;
        private System.Windows.Forms.Label laFreezedTakt;
        private System.Windows.Forms.ToolStripMenuItem testMemLogToolStripMenuItem;
        private System.Windows.Forms.BindingSource logMessageBindingSource;
    }
}