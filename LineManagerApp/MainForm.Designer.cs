namespace LineManagerApp
{
    partial class MainForm
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.linesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.productionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lineQueueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.chassisBufferToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uncopletedChassisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applicationLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testUtilslToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startHostLineService1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopHostLineService1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.callMethod1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mediaPanelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 886);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1357, 25);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(173, 20);
            this.toolStripStatusLabel1.Text = "Administrator main form";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.linesToolStripMenuItem,
            this.productionToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.testUtilslToolStripMenuItem,
            this.windowsToolStripMenuItem,
            this.exitToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.MdiWindowListItem = this.windowsToolStripMenuItem;
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1357, 28);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // linesToolStripMenuItem
            // 
            this.linesToolStripMenuItem.Name = "linesToolStripMenuItem";
            this.linesToolStripMenuItem.Size = new System.Drawing.Size(54, 24);
            this.linesToolStripMenuItem.Text = "&Lines";
            this.linesToolStripMenuItem.Click += new System.EventHandler(this.linesToolStripMenuItem_Click);
            // 
            // productionToolStripMenuItem
            // 
            this.productionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lineQueueToolStripMenuItem,
            this.toolStripSeparator1,
            this.chassisBufferToolStripMenuItem,
            this.uncopletedChassisToolStripMenuItem,
            this.stockToolStripMenuItem});
            this.productionToolStripMenuItem.Name = "productionToolStripMenuItem";
            this.productionToolStripMenuItem.Size = new System.Drawing.Size(93, 24);
            this.productionToolStripMenuItem.Text = "&Production";
            // 
            // lineQueueToolStripMenuItem
            // 
            this.lineQueueToolStripMenuItem.Name = "lineQueueToolStripMenuItem";
            this.lineQueueToolStripMenuItem.Size = new System.Drawing.Size(204, 24);
            this.lineQueueToolStripMenuItem.Text = "&Line queue";
            this.lineQueueToolStripMenuItem.Click += new System.EventHandler(this.lineQueueToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(201, 6);
            // 
            // chassisBufferToolStripMenuItem
            // 
            this.chassisBufferToolStripMenuItem.Name = "chassisBufferToolStripMenuItem";
            this.chassisBufferToolStripMenuItem.Size = new System.Drawing.Size(204, 24);
            this.chassisBufferToolStripMenuItem.Text = "&Chassis buffer";
            this.chassisBufferToolStripMenuItem.Click += new System.EventHandler(this.chassisBufferToolStripMenuItem_Click);
            // 
            // uncopletedChassisToolStripMenuItem
            // 
            this.uncopletedChassisToolStripMenuItem.Name = "uncopletedChassisToolStripMenuItem";
            this.uncopletedChassisToolStripMenuItem.Size = new System.Drawing.Size(204, 24);
            this.uncopletedChassisToolStripMenuItem.Text = "&Uncopleted chassis";
            this.uncopletedChassisToolStripMenuItem.Click += new System.EventHandler(this.uncopletedChassisToolStripMenuItem_Click);
            // 
            // stockToolStripMenuItem
            // 
            this.stockToolStripMenuItem.Name = "stockToolStripMenuItem";
            this.stockToolStripMenuItem.Size = new System.Drawing.Size(204, 24);
            this.stockToolStripMenuItem.Text = "&Stock";
            this.stockToolStripMenuItem.Click += new System.EventHandler(this.stockToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.applicationLogToolStripMenuItem,
            this.mediaPanelToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(57, 24);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // applicationLogToolStripMenuItem
            // 
            this.applicationLogToolStripMenuItem.Name = "applicationLogToolStripMenuItem";
            this.applicationLogToolStripMenuItem.Size = new System.Drawing.Size(181, 24);
            this.applicationLogToolStripMenuItem.Text = "&Application log";
            this.applicationLogToolStripMenuItem.Click += new System.EventHandler(this.applicationLogToolStripMenuItem_Click);
            // 
            // testUtilslToolStripMenuItem
            // 
            this.testUtilslToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startHostLineService1ToolStripMenuItem,
            this.stopHostLineService1ToolStripMenuItem,
            this.callMethod1ToolStripMenuItem,
            this.testFormToolStripMenuItem});
            this.testUtilslToolStripMenuItem.Name = "testUtilslToolStripMenuItem";
            this.testUtilslToolStripMenuItem.Size = new System.Drawing.Size(92, 24);
            this.testUtilslToolStripMenuItem.Text = "Test &utils ...";
            this.testUtilslToolStripMenuItem.Visible = false;
            // 
            // startHostLineService1ToolStripMenuItem
            // 
            this.startHostLineService1ToolStripMenuItem.Name = "startHostLineService1ToolStripMenuItem";
            this.startHostLineService1ToolStripMenuItem.Size = new System.Drawing.Size(234, 24);
            this.startHostLineService1ToolStripMenuItem.Text = "Start Host LineService1 ";
            this.startHostLineService1ToolStripMenuItem.Click += new System.EventHandler(this.startHostLineService1ToolStripMenuItem_Click);
            // 
            // stopHostLineService1ToolStripMenuItem
            // 
            this.stopHostLineService1ToolStripMenuItem.Name = "stopHostLineService1ToolStripMenuItem";
            this.stopHostLineService1ToolStripMenuItem.Size = new System.Drawing.Size(234, 24);
            this.stopHostLineService1ToolStripMenuItem.Text = "Start Host LineService2";
            this.stopHostLineService1ToolStripMenuItem.Click += new System.EventHandler(this.stopHostLineService1ToolStripMenuItem_Click);
            // 
            // callMethod1ToolStripMenuItem
            // 
            this.callMethod1ToolStripMenuItem.Name = "callMethod1ToolStripMenuItem";
            this.callMethod1ToolStripMenuItem.Size = new System.Drawing.Size(234, 24);
            this.callMethod1ToolStripMenuItem.Text = "Call method_1 ...";
            this.callMethod1ToolStripMenuItem.Click += new System.EventHandler(this.callMethod1ToolStripMenuItem_Click);
            // 
            // testFormToolStripMenuItem
            // 
            this.testFormToolStripMenuItem.Name = "testFormToolStripMenuItem";
            this.testFormToolStripMenuItem.Size = new System.Drawing.Size(234, 24);
            this.testFormToolStripMenuItem.Text = "Test form";
            this.testFormToolStripMenuItem.Click += new System.EventHandler(this.testFormToolStripMenuItem_Click);
            // 
            // windowsToolStripMenuItem
            // 
            this.windowsToolStripMenuItem.Checked = true;
            this.windowsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
            this.windowsToolStripMenuItem.Size = new System.Drawing.Size(82, 24);
            this.windowsToolStripMenuItem.Text = "&Windows";
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(45, 24);
            this.exitToolStripMenuItem1.Text = "&Exit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // mediaPanelToolStripMenuItem
            // 
            this.mediaPanelToolStripMenuItem.Name = "mediaPanelToolStripMenuItem";
            this.mediaPanelToolStripMenuItem.Size = new System.Drawing.Size(181, 24);
            this.mediaPanelToolStripMenuItem.Text = "Media Panel";
            this.mediaPanelToolStripMenuItem.Click += new System.EventHandler(this.mediaPanelToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1357, 911);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Text = "Line Manager application";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem linesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem testUtilslToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startHostLineService1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopHostLineService1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem callMethod1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testFormToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem productionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lineQueueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chassisBufferToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uncopletedChassisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applicationLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mediaPanelToolStripMenuItem;
    }
}