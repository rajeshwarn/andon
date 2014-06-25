namespace Security
{
    partial class UserPermitions
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
            this.cbUser = new System.Windows.Forms.ComboBox();
            this.usersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.detroit = new Security.Detroit();
            this.label1 = new System.Windows.Forms.Label();
            this.clbUsersPermissions = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDeselectAll1 = new System.Windows.Forms.Button();
            this.btnSelectAll1 = new System.Windows.Forms.Button();
            this.usersTableAdapter = new Security.DetroitTableAdapters.UsersTableAdapter();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.usersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detroit)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(488, 407);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(407, 407);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 15;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // cbUser
            // 
            this.cbUser.DataSource = this.usersBindingSource;
            this.cbUser.DisplayMember = "Name";
            this.cbUser.FormattingEnabled = true;
            this.cbUser.Location = new System.Drawing.Point(11, 28);
            this.cbUser.Name = "cbUser";
            this.cbUser.Size = new System.Drawing.Size(187, 21);
            this.cbUser.TabIndex = 17;
            this.cbUser.ValueMember = "Name";
            this.cbUser.SelectedValueChanged += new System.EventHandler(this.cbUser_SelectedValueChanged);
            // 
            // usersBindingSource
            // 
            this.usersBindingSource.DataMember = "Users";
            this.usersBindingSource.DataSource = this.detroit;
            // 
            // detroit
            // 
            this.detroit.DataSetName = "Detroit";
            this.detroit.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "User";
            // 
            // clbUsersPermissions
            // 
            this.clbUsersPermissions.ColumnWidth = 200;
            this.clbUsersPermissions.FormattingEnabled = true;
            this.clbUsersPermissions.Location = new System.Drawing.Point(11, 97);
            this.clbUsersPermissions.MultiColumn = true;
            this.clbUsersPermissions.Name = "clbUsersPermissions";
            this.clbUsersPermissions.Size = new System.Drawing.Size(552, 289);
            this.clbUsersPermissions.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "User permissions";
            // 
            // btnDeselectAll1
            // 
            this.btnDeselectAll1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDeselectAll1.Location = new System.Drawing.Point(488, 68);
            this.btnDeselectAll1.Name = "btnDeselectAll1";
            this.btnDeselectAll1.Size = new System.Drawing.Size(75, 23);
            this.btnDeselectAll1.TabIndex = 25;
            this.btnDeselectAll1.Text = "Deselect all";
            this.btnDeselectAll1.UseVisualStyleBackColor = true;
            this.btnDeselectAll1.Click += new System.EventHandler(this.btnDeselectAll1_Click);
            // 
            // btnSelectAll1
            // 
            this.btnSelectAll1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSelectAll1.Location = new System.Drawing.Point(407, 68);
            this.btnSelectAll1.Name = "btnSelectAll1";
            this.btnSelectAll1.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll1.TabIndex = 24;
            this.btnSelectAll1.Text = "Select all";
            this.btnSelectAll1.UseVisualStyleBackColor = true;
            this.btnSelectAll1.Click += new System.EventHandler(this.btnSelectAll1_Click);
            // 
            // usersTableAdapter
            // 
            this.usersTableAdapter.ClearBeforeFill = true;
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSave.Location = new System.Drawing.Point(11, 407);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 26;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // UserPermitions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 442);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnDeselectAll1);
            this.Controls.Add(this.btnSelectAll1);
            this.Controls.Add(this.clbUsersPermissions);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbUser);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "UserPermitions";
            this.Text = "UserPermitions";
            this.Load += new System.EventHandler(this.UserPermitions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.usersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detroit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ComboBox cbUser;
        private Detroit detroit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox clbUsersPermissions;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDeselectAll1;
        private System.Windows.Forms.Button btnSelectAll1;
        private System.Windows.Forms.BindingSource usersBindingSource;
        private DetroitTableAdapters.UsersTableAdapter usersTableAdapter;
        private System.Windows.Forms.Button btnSave;
    }
}