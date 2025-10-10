namespace AIChessDatabase.Dialogs
{
    partial class DlgBulkImportPGN
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgBulkImportPGN));
            this.ofDlg = new System.Windows.Forms.OpenFileDialog();
            this.sfDlg = new System.Windows.Forms.SaveFileDialog();
            this.cbDelete = new System.Windows.Forms.CheckBox();
            this.cbDuplicates = new System.Windows.Forms.CheckBox();
            this.pbFiles = new System.Windows.Forms.ProgressBar();
            this.bCancel = new System.Windows.Forms.Button();
            this.bFiles = new System.Windows.Forms.Button();
            this.bImport = new System.Windows.Forms.Button();
            this.pbMatches = new System.Windows.Forms.ProgressBar();
            this.lFiles = new System.Windows.Forms.Label();
            this.lMatches = new System.Windows.Forms.Label();
            this.lFileCount = new System.Windows.Forms.Label();
            this.lMatchCount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ofDlg
            // 
            this.ofDlg.Multiselect = true;
            // 
            // cbDelete
            // 
            this.cbDelete.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbDelete_Description;
            this.cbDelete.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbDelete_Name;
            this.cbDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbDelete.AutoSize = true;
            this.cbDelete.Location = new System.Drawing.Point(17, 16);
            this.cbDelete.Margin = new System.Windows.Forms.Padding(4);
            this.cbDelete.Name = "cbDelete";
            this.cbDelete.Size = new System.Drawing.Size(171, 20);
            this.cbDelete.TabIndex = 0;
            this.cbDelete.Text = global::AIChessDatabase.Properties.UIResources.CB_DELETEPGNFILES;
            this.cbDelete.UseVisualStyleBackColor = true;
            // 
            // cbDuplicates
            // 
            this.cbDuplicates.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbDuplicates_Description;
            this.cbDuplicates.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbDuplicates_Name;
            this.cbDuplicates.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbDuplicates.AutoSize = true;
            this.cbDuplicates.Location = new System.Drawing.Point(17, 46);
            this.cbDuplicates.Margin = new System.Windows.Forms.Padding(4);
            this.cbDuplicates.Name = "cbDuplicates";
            this.cbDuplicates.Size = new System.Drawing.Size(180, 20);
            this.cbDuplicates.TabIndex = 1;
            this.cbDuplicates.Text = global::AIChessDatabase.Properties.UIResources.CB_DUPLICATES;
            this.cbDuplicates.UseVisualStyleBackColor = true;
            // 
            // pbFiles
            // 
            this.pbFiles.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.pbFiles_Description;
            this.pbFiles.AccessibleName = global::AIChessDatabase.Properties.UIElements.pbFiles_Name;
            this.pbFiles.AccessibleRole = System.Windows.Forms.AccessibleRole.ProgressBar;
            this.pbFiles.Location = new System.Drawing.Point(16, 103);
            this.pbFiles.Margin = new System.Windows.Forms.Padding(4);
            this.pbFiles.Name = "pbFiles";
            this.pbFiles.Size = new System.Drawing.Size(439, 28);
            this.pbFiles.TabIndex = 2;
            // 
            // bCancel
            // 
            this.bCancel.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bCancel_Description;
            this.bCancel.AccessibleName = global::AIChessDatabase.Properties.UIElements.bCancel_Name;
            this.bCancel.Location = new System.Drawing.Point(355, 231);
            this.bCancel.Margin = new System.Windows.Forms.Padding(4);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(100, 28);
            this.bCancel.TabIndex = 4;
            this.bCancel.Text = global::AIChessDatabase.Properties.UIResources.BTN_CANCEL;
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bFiles
            // 
            this.bFiles.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bFiles_Description;
            this.bFiles.AccessibleName = global::AIChessDatabase.Properties.UIElements.bFiles_Name;
            this.bFiles.Location = new System.Drawing.Point(16, 231);
            this.bFiles.Margin = new System.Windows.Forms.Padding(4);
            this.bFiles.Name = "bFiles";
            this.bFiles.Size = new System.Drawing.Size(185, 28);
            this.bFiles.TabIndex = 5;
            this.bFiles.Text = global::AIChessDatabase.Properties.UIResources.BTN_SELFILES;
            this.bFiles.UseVisualStyleBackColor = true;
            this.bFiles.Click += new System.EventHandler(this.bFiles_Click);
            // 
            // bImport
            // 
            this.bImport.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bImport_Description;
            this.bImport.AccessibleName = global::AIChessDatabase.Properties.UIElements.bImport_Name;
            this.bImport.Enabled = false;
            this.bImport.Location = new System.Drawing.Point(235, 231);
            this.bImport.Margin = new System.Windows.Forms.Padding(4);
            this.bImport.Name = "bImport";
            this.bImport.Size = new System.Drawing.Size(100, 28);
            this.bImport.TabIndex = 6;
            this.bImport.Text = global::AIChessDatabase.Properties.UIResources.BTN_IMPORT;
            this.bImport.UseVisualStyleBackColor = true;
            this.bImport.Click += new System.EventHandler(this.bImport_Click);
            // 
            // pbMatches
            // 
            this.pbMatches.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.pbMatches_Description;
            this.pbMatches.AccessibleName = global::AIChessDatabase.Properties.UIElements.pbMatches_Name;
            this.pbMatches.AccessibleRole = System.Windows.Forms.AccessibleRole.ProgressBar;
            this.pbMatches.Location = new System.Drawing.Point(16, 165);
            this.pbMatches.Margin = new System.Windows.Forms.Padding(4);
            this.pbMatches.Name = "pbMatches";
            this.pbMatches.Size = new System.Drawing.Size(439, 28);
            this.pbMatches.TabIndex = 7;
            // 
            // lFiles
            // 
            this.lFiles.AutoSize = true;
            this.lFiles.Location = new System.Drawing.Point(17, 84);
            this.lFiles.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lFiles.Name = "lFiles";
            this.lFiles.Size = new System.Drawing.Size(36, 16);
            this.lFiles.TabIndex = 8;
            this.lFiles.Text = "FIles";
            // 
            // lMatches
            // 
            this.lMatches.AutoSize = true;
            this.lMatches.Location = new System.Drawing.Point(17, 140);
            this.lMatches.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lMatches.Name = "lMatches";
            this.lMatches.Size = new System.Drawing.Size(58, 16);
            this.lMatches.TabIndex = 9;
            this.lMatches.Text = "Matches";
            // 
            // lFileCount
            // 
            this.lFileCount.AutoSize = true;
            this.lFileCount.Location = new System.Drawing.Point(114, 83);
            this.lFileCount.Name = "lFileCount";
            this.lFileCount.Size = new System.Drawing.Size(0, 16);
            this.lFileCount.TabIndex = 10;
            // 
            // lMatchCount
            // 
            this.lMatchCount.AutoSize = true;
            this.lMatchCount.Location = new System.Drawing.Point(114, 140);
            this.lMatchCount.Name = "lMatchCount";
            this.lMatchCount.Size = new System.Drawing.Size(0, 16);
            this.lMatchCount.TabIndex = 11;
            // 
            // DlgBulkImportPGN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 274);
            this.Controls.Add(this.lMatchCount);
            this.Controls.Add(this.lFileCount);
            this.Controls.Add(this.lMatches);
            this.Controls.Add(this.lFiles);
            this.Controls.Add(this.pbMatches);
            this.Controls.Add(this.bImport);
            this.Controls.Add(this.bFiles);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.pbFiles);
            this.Controls.Add(this.cbDuplicates);
            this.Controls.Add(this.cbDelete);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgBulkImportPGN";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DlgBulkImportPGN";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DlgBulkImportPGN_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DlgBulkImportPGN_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofDlg;
        private System.Windows.Forms.SaveFileDialog sfDlg;
        private System.Windows.Forms.CheckBox cbDelete;
        private System.Windows.Forms.CheckBox cbDuplicates;
        private System.Windows.Forms.ProgressBar pbFiles;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bFiles;
        private System.Windows.Forms.Button bImport;
        private System.Windows.Forms.ProgressBar pbMatches;
        private System.Windows.Forms.Label lFiles;
        private System.Windows.Forms.Label lMatches;
        private System.Windows.Forms.Label lFileCount;
        private System.Windows.Forms.Label lMatchCount;
    }
}