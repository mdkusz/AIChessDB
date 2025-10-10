namespace AIChessDatabase.Dialogs
{
    partial class DlgBulkCopyDB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgBulkCopyDB));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbDestinationDB = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.cbDuplicates = new System.Windows.Forms.CheckBox();
            this.pbCopy = new System.Windows.Forms.ProgressBar();
            this.bCancel = new System.Windows.Forms.Button();
            this.bExport = new System.Windows.Forms.Button();
            this.sfDlg = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "{0} Matches in the Source Database";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 63);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Destination Database";
            // 
            // cbDestinationDB
            // 
            this.cbDestinationDB.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.cbDestinationDB.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbDestinationDB_Name;
            this.cbDestinationDB.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbDestinationDB_Description;
            this.cbDestinationDB.FormattingEnabled = true;
            this.cbDestinationDB.Location = new System.Drawing.Point(20, 82);
            this.cbDestinationDB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbDestinationDB.Name = "cbDestinationDB";
            this.cbDestinationDB.Size = new System.Drawing.Size(364, 24);
            this.cbDestinationDB.TabIndex = 2;
            this.cbDestinationDB.SelectedIndexChanged += new System.EventHandler(this.cbDestinationDB_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 128);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Copy Chunks of Size";
            // 
            // txtSize
            // 
            this.txtSize.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtSize.AccessibleName = global::AIChessDatabase.Properties.UIElements.txtSize_Name;
            this.txtSize.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.txtSize_Description;
            this.txtSize.Location = new System.Drawing.Point(20, 148);
            this.txtSize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(132, 22);
            this.txtSize.TabIndex = 4;
            this.txtSize.TextChanged += new System.EventHandler(this.cbDestinationDB_SelectedIndexChanged);
            // 
            // cbDuplicates
            // 
            this.cbDuplicates.AccessibleRole = System.Windows.Forms.AccessibleRole.Default;
            this.cbDuplicates.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbDuplicates_Name;
            this.cbDuplicates.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbDuplicates_Description;
            this.cbDuplicates.AutoSize = true;
            this.cbDuplicates.Location = new System.Drawing.Point(171, 150);
            this.cbDuplicates.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbDuplicates.Name = "cbDuplicates";
            this.cbDuplicates.Size = new System.Drawing.Size(175, 20);
            this.cbDuplicates.TabIndex = 5;
            this.cbDuplicates.Text = global::AIChessDatabase.Properties.UIResources.CB_COPYDUPLICATES;
            this.cbDuplicates.UseVisualStyleBackColor = true;
            // 
            // pbCopy
            // 
            this.pbCopy.AccessibleRole = System.Windows.Forms.AccessibleRole.ProgressBar;
            this.pbCopy.AccessibleName = global::AIChessDatabase.Properties.UIElements.pbCopy_Name;
            this.pbCopy.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.pbCopy_Description;
            this.pbCopy.Location = new System.Drawing.Point(16, 209);
            this.pbCopy.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbCopy.Name = "pbCopy";
            this.pbCopy.Size = new System.Drawing.Size(369, 28);
            this.pbCopy.TabIndex = 6;
            // 
            // bCancel
            // 
            this.bCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.Default;
            this.bCancel.AccessibleName = global::AIChessDatabase.Properties.UIElements.bCancel_Name;
            this.bCancel.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bCancel_Description;
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(285, 287);
            this.bCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(100, 28);
            this.bCancel.TabIndex = 7;
            this.bCancel.Text = global::AIChessDatabase.Properties.UIResources.BTN_CANCEL;
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bExport
            // 
            this.bExport.AccessibleRole = System.Windows.Forms.AccessibleRole.Default;
            this.bExport.AccessibleName = global::AIChessDatabase.Properties.UIElements.bExport_Name;
            this.bExport.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bExport_Description;
            this.bExport.Enabled = false;
            this.bExport.Location = new System.Drawing.Point(171, 287);
            this.bExport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bExport.Name = "bExport";
            this.bExport.Size = new System.Drawing.Size(100, 28);
            this.bExport.TabIndex = 8;
            this.bExport.Text = global::AIChessDatabase.Properties.UIResources.BTN_EXPORT;
            this.bExport.UseVisualStyleBackColor = true;
            this.bExport.Click += new System.EventHandler(this.bExport_Click);
            // 
            // DlgBulkCopyDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 330);
            this.Controls.Add(this.bExport);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.pbCopy);
            this.Controls.Add(this.cbDuplicates);
            this.Controls.Add(this.txtSize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbDestinationDB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgBulkCopyDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export Database";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DlgBulkCopyDB_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DlgBulkCopyDB_FormClosed);
            this.Shown += new System.EventHandler(this.DlgBulkCopyDB_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbDestinationDB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.CheckBox cbDuplicates;
        private System.Windows.Forms.ProgressBar pbCopy;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bExport;
        private System.Windows.Forms.SaveFileDialog sfDlg;
    }
}