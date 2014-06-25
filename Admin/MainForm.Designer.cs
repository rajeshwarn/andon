namespace Admin
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
            this.components = new System.ComponentModel.Container();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.linesAndStationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stationControlsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.partsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultSheduleFramesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scheduleFramesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.batchTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.productionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.batchesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lineQueueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.workScheduleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.chassisBufferToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uncompletedChassiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.errorLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.securityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createNewUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userPermissionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detroitDataSet1 = new PlannerLib.DetroitDataSet();
            this.detroitDataSet2 = new LineService.DetroitDataSet();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logMessageBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.detroitDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detroitDataSet2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logMessageBindingSource)).BeginInit();
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
            this.settingsToolStripMenuItem,
            this.productionToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.securityToolStripMenuItem,
            this.windowsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.MdiWindowListItem = this.windowsToolStripMenuItem;
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1357, 28);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.linesAndStationsToolStripMenuItem,
            this.stationsToolStripMenuItem,
            this.stationControlsToolStripMenuItem,
            this.partsToolStripMenuItem,
            this.defaultSheduleFramesToolStripMenuItem,
            this.scheduleFramesToolStripMenuItem,
            this.toolStripSeparator2,
            this.batchTypesToolStripMenuItem,
            this.mapsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // linesAndStationsToolStripMenuItem
            // 
            this.linesAndStationsToolStripMenuItem.Name = "linesAndStationsToolStripMenuItem";
            this.linesAndStationsToolStripMenuItem.Size = new System.Drawing.Size(231, 24);
            this.linesAndStationsToolStripMenuItem.Text = "&Lines";
            this.linesAndStationsToolStripMenuItem.Click += new System.EventHandler(this.linesAndStationsToolStripMenuItem_Click);
            // 
            // stationsToolStripMenuItem
            // 
            this.stationsToolStripMenuItem.Name = "stationsToolStripMenuItem";
            this.stationsToolStripMenuItem.Size = new System.Drawing.Size(231, 24);
            this.stationsToolStripMenuItem.Text = "&Stations";
            this.stationsToolStripMenuItem.Click += new System.EventHandler(this.stationsToolStripMenuItem_Click);
            // 
            // stationControlsToolStripMenuItem
            // 
            this.stationControlsToolStripMenuItem.Name = "stationControlsToolStripMenuItem";
            this.stationControlsToolStripMenuItem.Size = new System.Drawing.Size(231, 24);
            this.stationControlsToolStripMenuItem.Text = "Station &controls";
            this.stationControlsToolStripMenuItem.Click += new System.EventHandler(this.stationControlsToolStripMenuItem_Click);
            // 
            // partsToolStripMenuItem
            // 
            this.partsToolStripMenuItem.Name = "partsToolStripMenuItem";
            this.partsToolStripMenuItem.Size = new System.Drawing.Size(231, 24);
            this.partsToolStripMenuItem.Text = "&Parts";
            this.partsToolStripMenuItem.Click += new System.EventHandler(this.partsToolStripMenuItem_Click);
            // 
            // defaultSheduleFramesToolStripMenuItem
            // 
            this.defaultSheduleFramesToolStripMenuItem.Name = "defaultSheduleFramesToolStripMenuItem";
            this.defaultSheduleFramesToolStripMenuItem.Size = new System.Drawing.Size(231, 24);
            this.defaultSheduleFramesToolStripMenuItem.Text = "&Default shedule frames";
            this.defaultSheduleFramesToolStripMenuItem.Click += new System.EventHandler(this.defaultSheduleFramesToolStripMenuItem_Click);
            // 
            // scheduleFramesToolStripMenuItem
            // 
            this.scheduleFramesToolStripMenuItem.Name = "scheduleFramesToolStripMenuItem";
            this.scheduleFramesToolStripMenuItem.Size = new System.Drawing.Size(231, 24);
            this.scheduleFramesToolStripMenuItem.Text = "S&chedule frames";
            this.scheduleFramesToolStripMenuItem.Click += new System.EventHandler(this.scheduleFramesToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(228, 6);
            // 
            // batchTypesToolStripMenuItem
            // 
            this.batchTypesToolStripMenuItem.Name = "batchTypesToolStripMenuItem";
            this.batchTypesToolStripMenuItem.Size = new System.Drawing.Size(231, 24);
            this.batchTypesToolStripMenuItem.Text = "Batch &types";
            this.batchTypesToolStripMenuItem.Click += new System.EventHandler(this.batchTypesToolStripMenuItem_Click);
            // 
            // mapsToolStripMenuItem
            // 
            this.mapsToolStripMenuItem.Name = "mapsToolStripMenuItem";
            this.mapsToolStripMenuItem.Size = new System.Drawing.Size(231, 24);
            this.mapsToolStripMenuItem.Text = "&Maps";
            this.mapsToolStripMenuItem.Click += new System.EventHandler(this.mapsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(228, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(231, 24);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // productionToolStripMenuItem
            // 
            this.productionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.batchesToolStripMenuItem,
            this.lineQueueToolStripMenuItem,
            this.workScheduleToolStripMenuItem,
            this.toolStripSeparator3,
            this.chassisBufferToolStripMenuItem,
            this.uncompletedChassiesToolStripMenuItem,
            this.stockToolStripMenuItem});
            this.productionToolStripMenuItem.Name = "productionToolStripMenuItem";
            this.productionToolStripMenuItem.Size = new System.Drawing.Size(93, 24);
            this.productionToolStripMenuItem.Text = "Production";
            // 
            // batchesToolStripMenuItem
            // 
            this.batchesToolStripMenuItem.Name = "batchesToolStripMenuItem";
            this.batchesToolStripMenuItem.Size = new System.Drawing.Size(217, 24);
            this.batchesToolStripMenuItem.Text = "&Batches";
            this.batchesToolStripMenuItem.Click += new System.EventHandler(this.batchesToolStripMenuItem_Click);
            // 
            // lineQueueToolStripMenuItem
            // 
            this.lineQueueToolStripMenuItem.Name = "lineQueueToolStripMenuItem";
            this.lineQueueToolStripMenuItem.Size = new System.Drawing.Size(217, 24);
            this.lineQueueToolStripMenuItem.Text = "&Line queue";
            this.lineQueueToolStripMenuItem.Click += new System.EventHandler(this.lineQueueToolStripMenuItem_Click);
            // 
            // workScheduleToolStripMenuItem
            // 
            this.workScheduleToolStripMenuItem.Name = "workScheduleToolStripMenuItem";
            this.workScheduleToolStripMenuItem.Size = new System.Drawing.Size(217, 24);
            this.workScheduleToolStripMenuItem.Text = "&Work schedule";
            this.workScheduleToolStripMenuItem.Visible = false;
            this.workScheduleToolStripMenuItem.Click += new System.EventHandler(this.workScheduleToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(214, 6);
            // 
            // chassisBufferToolStripMenuItem
            // 
            this.chassisBufferToolStripMenuItem.Name = "chassisBufferToolStripMenuItem";
            this.chassisBufferToolStripMenuItem.Size = new System.Drawing.Size(217, 24);
            this.chassisBufferToolStripMenuItem.Text = "Chassis buffer";
            this.chassisBufferToolStripMenuItem.Click += new System.EventHandler(this.chassisBufferToolStripMenuItem_Click);
            // 
            // uncompletedChassiesToolStripMenuItem
            // 
            this.uncompletedChassiesToolStripMenuItem.Name = "uncompletedChassiesToolStripMenuItem";
            this.uncompletedChassiesToolStripMenuItem.Size = new System.Drawing.Size(217, 24);
            this.uncompletedChassiesToolStripMenuItem.Text = "Uncompleted chassis";
            this.uncompletedChassiesToolStripMenuItem.Click += new System.EventHandler(this.uncompletedChassiesToolStripMenuItem_Click);
            // 
            // stockToolStripMenuItem
            // 
            this.stockToolStripMenuItem.Name = "stockToolStripMenuItem";
            this.stockToolStripMenuItem.Size = new System.Drawing.Size(217, 24);
            this.stockToolStripMenuItem.Text = "&Stock";
            this.stockToolStripMenuItem.Click += new System.EventHandler(this.stockToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logToolStripMenuItem,
            this.errorLogToolStripMenuItem,
            this.testToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(57, 24);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // logToolStripMenuItem
            // 
            this.logToolStripMenuItem.Name = "logToolStripMenuItem";
            this.logToolStripMenuItem.Size = new System.Drawing.Size(184, 24);
            this.logToolStripMenuItem.Text = "&Application Log";
            this.logToolStripMenuItem.Click += new System.EventHandler(this.logToolStripMenuItem_Click);
            // 
            // errorLogToolStripMenuItem
            // 
            this.errorLogToolStripMenuItem.Name = "errorLogToolStripMenuItem";
            this.errorLogToolStripMenuItem.Size = new System.Drawing.Size(184, 24);
            this.errorLogToolStripMenuItem.Text = "Error Log";
            this.errorLogToolStripMenuItem.Click += new System.EventHandler(this.errorLogToolStripMenuItem_Click);
            // 
            // securityToolStripMenuItem
            // 
            this.securityToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createNewUserToolStripMenuItem,
            this.logonToolStripMenuItem,
            this.userPermissionsToolStripMenuItem});
            this.securityToolStripMenuItem.Name = "securityToolStripMenuItem";
            this.securityToolStripMenuItem.Size = new System.Drawing.Size(73, 24);
            this.securityToolStripMenuItem.Text = "Security";
            // 
            // createNewUserToolStripMenuItem
            // 
            this.createNewUserToolStripMenuItem.Name = "createNewUserToolStripMenuItem";
            this.createNewUserToolStripMenuItem.Size = new System.Drawing.Size(189, 24);
            this.createNewUserToolStripMenuItem.Text = "Create new user";
            this.createNewUserToolStripMenuItem.Click += new System.EventHandler(this.createNewUserToolStripMenuItem_Click);
            // 
            // logonToolStripMenuItem
            // 
            this.logonToolStripMenuItem.Name = "logonToolStripMenuItem";
            this.logonToolStripMenuItem.Size = new System.Drawing.Size(189, 24);
            this.logonToolStripMenuItem.Text = "Logon ...";
            this.logonToolStripMenuItem.Click += new System.EventHandler(this.logonToolStripMenuItem_Click);
            // 
            // userPermissionsToolStripMenuItem
            // 
            this.userPermissionsToolStripMenuItem.Name = "userPermissionsToolStripMenuItem";
            this.userPermissionsToolStripMenuItem.Size = new System.Drawing.Size(189, 24);
            this.userPermissionsToolStripMenuItem.Text = "User permissions";
            this.userPermissionsToolStripMenuItem.Click += new System.EventHandler(this.userPermissionsToolStripMenuItem_Click);
            // 
            // windowsToolStripMenuItem
            // 
            this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
            this.windowsToolStripMenuItem.Size = new System.Drawing.Size(82, 24);
            this.windowsToolStripMenuItem.Text = "&Windows";
            // 
            // detroitDataSet1
            // 
            this.detroitDataSet1.DataSetName = "DetroitDataSet";
            this.detroitDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // detroitDataSet2
            // 
            this.detroitDataSet2.DataSetName = "DetroitDataSet";
            this.detroitDataSet2.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(184, 24);
            this.testToolStripMenuItem.Text = "Test";
            // 
            // logMessageBindingSource
            // 
            this.logMessageBindingSource.DataSource = typeof(LineService.LogMessage);
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
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Admin application";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.detroitDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detroitDataSet2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logMessageBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem linesAndStationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stationControlsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem batchTypesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem partsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem productionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem batchesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lineQueueToolStripMenuItem;
        //private wsLineHostController.LineHostController lineHostController1;
        private System.Windows.Forms.ToolStripMenuItem scheduleFramesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem workScheduleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uncompletedChassiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem chassisBufferToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defaultSheduleFramesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem securityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createNewUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userPermissionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowsToolStripMenuItem;
        private PlannerLib.DetroitDataSet detroitDataSet1;
        private LineService.DetroitDataSet detroitDataSet2;
        private System.Windows.Forms.ToolStripMenuItem stationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem errorLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.BindingSource logMessageBindingSource;
    }
}