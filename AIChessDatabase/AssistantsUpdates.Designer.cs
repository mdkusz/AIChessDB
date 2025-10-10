namespace AIChessDatabase
{
    partial class AssistantsUpdates
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssistantsUpdates));
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.bFont = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bEditDoc = new System.Windows.Forms.ToolStripButton();
            this.bAddDoc = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bApplyUpdates = new System.Windows.Forms.ToolStripButton();
            this.pgUpdates = new System.Windows.Forms.ToolStripProgressBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.rtvStructure = new DesktopControls.Controls.ReflectiveTreeView();
            this.flpEditor = new DesktopControls.Controls.InputEditors.InputEditorFlowContainer();
            this.fontdlg = new System.Windows.Forms.FontDialog();
            this.ofDlg = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1376, 35);
            this.panel1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.AUpdates_Toolbar_Description;
            this.toolStrip1.AccessibleName = global::AIChessDatabase.Properties.UIElements.AUpdates_Toolbar_Name;
            this.toolStrip1.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bFont,
            this.toolStripSeparator1,
            this.bEditDoc,
            this.bAddDoc,
            this.toolStripSeparator2,
            this.bApplyUpdates,
            this.pgUpdates});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1376, 40);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // bFont
            // 
            this.bFont.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bFont_Description;
            this.bFont.AccessibleName = global::AIChessDatabase.Properties.UIElements.bFont_Name;
            this.bFont.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bFont.Image = ((System.Drawing.Image)(resources.GetObject("bFont.Image")));
            this.bFont.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bFont.Name = "bFont";
            this.bFont.Size = new System.Drawing.Size(34, 35);
            this.bFont.Click += new System.EventHandler(this.bFont_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 40);
            // 
            // bEditDoc
            // 
            this.bEditDoc.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bEditDoc_Description;
            this.bEditDoc.AccessibleName = global::AIChessDatabase.Properties.UIElements.bEditDoc_Name;
            this.bEditDoc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bEditDoc.Enabled = false;
            this.bEditDoc.Image = ((System.Drawing.Image)(resources.GetObject("bEditDoc.Image")));
            this.bEditDoc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bEditDoc.Name = "bEditDoc";
            this.bEditDoc.Size = new System.Drawing.Size(34, 35);
            this.bEditDoc.Click += new System.EventHandler(this.bEditDoc_Click);
            // 
            // bAddDoc
            // 
            this.bAddDoc.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bAddDoc_Description;
            this.bAddDoc.AccessibleName = global::AIChessDatabase.Properties.UIElements.bAddDoc_Name;
            this.bAddDoc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bAddDoc.Enabled = false;
            this.bAddDoc.Image = ((System.Drawing.Image)(resources.GetObject("bAddDoc.Image")));
            this.bAddDoc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bAddDoc.Name = "bAddDoc";
            this.bAddDoc.Size = new System.Drawing.Size(34, 35);
            this.bAddDoc.Click += new System.EventHandler(this.bAddDoc_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 40);
            // 
            // bApplyUpdates
            // 
            this.bApplyUpdates.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bApplyUpdates_Description;
            this.bApplyUpdates.AccessibleName = global::AIChessDatabase.Properties.UIElements.bApplyUpdates_Name;
            this.bApplyUpdates.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bApplyUpdates.Image = ((System.Drawing.Image)(resources.GetObject("bApplyUpdates.Image")));
            this.bApplyUpdates.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bApplyUpdates.Name = "bApplyUpdates";
            this.bApplyUpdates.Size = new System.Drawing.Size(34, 35);
            this.bApplyUpdates.Click += new System.EventHandler(this.bApplyUpdates_Click);
            // 
            // pgUpdates
            // 
            this.pgUpdates.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.pgUpdates_Description;
            this.pgUpdates.AccessibleName = global::AIChessDatabase.Properties.UIElements.pgUpdates_Name;
            this.pgUpdates.AccessibleRole = System.Windows.Forms.AccessibleRole.ProgressBar;
            this.pgUpdates.Name = "pgUpdates";
            this.pgUpdates.Size = new System.Drawing.Size(169, 35);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 35);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.rtvStructure);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.flpEditor);
            this.splitContainer1.Size = new System.Drawing.Size(1376, 839);
            this.splitContainer1.SplitterDistance = 666;
            this.splitContainer1.TabIndex = 2;
            // 
            // rtvStructure
            // 
            this.rtvStructure.AccessibleDescription = "";
            this.rtvStructure.AccessibleName = global::AIChessDatabase.Properties.UIElements.rtvStructure_Name;
            this.rtvStructure.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.rtvStructure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtvStructure.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtvStructure.Location = new System.Drawing.Point(0, 0);
            this.rtvStructure.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rtvStructure.Name = "rtvStructure";
            this.rtvStructure.ShowNodeToolTips = true;
            this.rtvStructure.Size = new System.Drawing.Size(666, 839);
            this.rtvStructure.TabIndex = 0;
            this.rtvStructure.TreeObject = null;
            this.rtvStructure.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.rtvStructure_AfterSelect);
            // 
            // flpEditor
            // 
            this.flpEditor.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.flpEditor_Description;
            this.flpEditor.AccessibleName = global::AIChessDatabase.Properties.UIElements.flpEditor_Name;
            this.flpEditor.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
            this.flpEditor.AutoScroll = true;
            this.flpEditor.BackColor = System.Drawing.SystemColors.Desktop;
            this.flpEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpEditor.EditorFactory = null;
            this.flpEditor.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpEditor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flpEditor.Location = new System.Drawing.Point(0, 0);
            this.flpEditor.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.flpEditor.Name = "flpEditor";
            this.flpEditor.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.flpEditor.Size = new System.Drawing.Size(706, 839);
            this.flpEditor.TabIndex = 6;
            this.flpEditor.WrapContents = false;
            // 
            // ofDlg
            // 
            this.ofDlg.Multiselect = true;
            this.ofDlg.RestoreDirectory = true;
            // 
            // AssistantsUpdates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1376, 874);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "AssistantsUpdates";
            this.Text = "AssistantsUpdates";
            this.Load += new System.EventHandler(this.AssistantsUpdates_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DesktopControls.Controls.ReflectiveTreeView rtvStructure;
        private DesktopControls.Controls.InputEditors.InputEditorFlowContainer flpEditor;
        private System.Windows.Forms.ToolStripButton bFont;
        private System.Windows.Forms.FontDialog fontdlg;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton bEditDoc;
        private System.Windows.Forms.ToolStripButton bAddDoc;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton bApplyUpdates;
        private System.Windows.Forms.OpenFileDialog ofDlg;
        private System.Windows.Forms.ToolStripProgressBar pgUpdates;
    }
}