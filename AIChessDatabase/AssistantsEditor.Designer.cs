namespace AIChessDatabase
{
    partial class AssistantsEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssistantsEditor));
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.bFont = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lAPI = new System.Windows.Forms.ToolStripLabel();
            this.cbAPIManager = new System.Windows.Forms.ToolStripComboBox();
            this.lItem = new System.Windows.Forms.ToolStripLabel();
            this.cbItemType = new System.Windows.Forms.ToolStripComboBox();
            this.bNewItem = new System.Windows.Forms.ToolStripButton();
            this.lInstance = new System.Windows.Forms.ToolStripLabel();
            this.cbInstance = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cbOtherInstances = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bDelItem = new System.Windows.Forms.ToolStripButton();
            this.bSaveItem = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.rtvStructure = new DesktopControls.Controls.ReflectiveTreeView();
            this.flpEditor = new DesktopControls.Controls.InputEditors.InputEditorFlowContainer();
            this.fontdlg = new System.Windows.Forms.FontDialog();
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
            this.panel1.Size = new System.Drawing.Size(1320, 35);
            this.panel1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.AEditor_Toolbar_Description;
            this.toolStrip1.AccessibleName = global::AIChessDatabase.Properties.UIElements.AEditor_Toolbar_Name;
            this.toolStrip1.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bFont,
            this.toolStripSeparator1,
            this.lAPI,
            this.cbAPIManager,
            this.lItem,
            this.cbItemType,
            this.bNewItem,
            this.lInstance,
            this.cbInstance,
            this.toolStripLabel1,
            this.cbOtherInstances,
            this.toolStripSeparator2,
            this.bDelItem,
            this.bSaveItem});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1320, 33);
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
            this.bFont.Size = new System.Drawing.Size(34, 28);
            this.bFont.Click += new System.EventHandler(this.bFont_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // lAPI
            // 
            this.lAPI.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lAPI.Name = "lAPI";
            this.lAPI.Size = new System.Drawing.Size(42, 28);
            this.lAPI.Text = "API";
            // 
            // cbAPIManager
            // 
            this.cbAPIManager.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbAPIManager_Description;
            this.cbAPIManager.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbAPIManager_Name;
            this.cbAPIManager.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.cbAPIManager.AutoSize = false;
            this.cbAPIManager.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAPIManager.Name = "cbAPIManager";
            this.cbAPIManager.Size = new System.Drawing.Size(136, 33);
            this.cbAPIManager.Sorted = true;
            this.cbAPIManager.SelectedIndexChanged += new System.EventHandler(this.cbAPIManager_SelectedIndexChanged);
            // 
            // lItem
            // 
            this.lItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lItem.Name = "lItem";
            this.lItem.Size = new System.Drawing.Size(59, 28);
            this.lItem.Text = global::AIChessDatabase.Properties.UIResources.LAB_ITEMS;
            // 
            // cbItemType
            // 
            this.cbItemType.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbItemType_Description;
            this.cbItemType.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbItemType_Name;
            this.cbItemType.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.cbItemType.AutoSize = false;
            this.cbItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbItemType.Name = "cbItemType";
            this.cbItemType.Size = new System.Drawing.Size(136, 33);
            this.cbItemType.Sorted = true;
            this.cbItemType.SelectedIndexChanged += new System.EventHandler(this.cbItemType_SelectedIndexChanged);
            // 
            // bNewItem
            // 
            this.bNewItem.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bNewItem_Description;
            this.bNewItem.AccessibleName = global::AIChessDatabase.Properties.UIElements.bNewItem_Name;
            this.bNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bNewItem.Enabled = false;
            this.bNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bNewItem.Image")));
            this.bNewItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bNewItem.Name = "bNewItem";
            this.bNewItem.Size = new System.Drawing.Size(34, 28);
            this.bNewItem.Click += new System.EventHandler(this.bNewItem_Click);
            // 
            // lInstance
            // 
            this.lInstance.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lInstance.Name = "lInstance";
            this.lInstance.Size = new System.Drawing.Size(84, 28);
            this.lInstance.Text = global::AIChessDatabase.Properties.UIResources.LAB_Instance;
            // 
            // cbInstance
            // 
            this.cbInstance.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbOtherInstance_Description;
            this.cbInstance.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbInstance_Name;
            this.cbInstance.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.cbInstance.AutoSize = false;
            this.cbInstance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInstance.Name = "cbInstance";
            this.cbInstance.Size = new System.Drawing.Size(136, 33);
            this.cbInstance.Sorted = true;
            this.cbInstance.SelectedIndexChanged += new System.EventHandler(this.cbInstance_SelectedIndexChanged);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(146, 28);
            this.toolStripLabel1.Text = global::AIChessDatabase.Properties.UIResources.LAB_OtherInstances;
            // 
            // cbOtherInstances
            // 
            this.cbOtherInstances.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbOtherInstance_Name;
            this.cbOtherInstances.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.cbOtherInstances.AutoSize = false;
            this.cbOtherInstances.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOtherInstances.Name = "cbOtherInstances";
            this.cbOtherInstances.Size = new System.Drawing.Size(136, 33);
            this.cbOtherInstances.Sorted = true;
            this.cbOtherInstances.SelectedIndexChanged += new System.EventHandler(this.cbOtherInstances_SelectedIndexChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 33);
            // 
            // bDelItem
            // 
            this.bDelItem.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bDelItem_Description;
            this.bDelItem.AccessibleName = global::AIChessDatabase.Properties.UIElements.bDelItem_Name;
            this.bDelItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bDelItem.Enabled = false;
            this.bDelItem.Image = ((System.Drawing.Image)(resources.GetObject("bDelItem.Image")));
            this.bDelItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bDelItem.Name = "bDelItem";
            this.bDelItem.Size = new System.Drawing.Size(34, 28);
            this.bDelItem.Click += new System.EventHandler(this.bDelItem_Click);
            // 
            // bSaveItem
            // 
            this.bSaveItem.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bSaveItem_Description;
            this.bSaveItem.AccessibleName = global::AIChessDatabase.Properties.UIElements.bSaveItem_Name;
            this.bSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bSaveItem.Enabled = false;
            this.bSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("bSaveItem.Image")));
            this.bSaveItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bSaveItem.Name = "bSaveItem";
            this.bSaveItem.Size = new System.Drawing.Size(34, 28);
            this.bSaveItem.Click += new System.EventHandler(this.bSaveItem_Click);
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
            this.splitContainer1.Size = new System.Drawing.Size(1320, 725);
            this.splitContainer1.SplitterDistance = 639;
            this.splitContainer1.TabIndex = 1;
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
            this.rtvStructure.Size = new System.Drawing.Size(639, 725);
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
            this.flpEditor.Size = new System.Drawing.Size(677, 725);
            this.flpEditor.TabIndex = 6;
            this.flpEditor.WrapContents = false;
            // 
            // AssistantsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1320, 760);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "AssistantsEditor";
            this.Text = "AssistantsEditor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AssistantsEditor_FormClosing);
            this.Load += new System.EventHandler(this.AssistantsEditor_Load);
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
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel lAPI;
        private System.Windows.Forms.ToolStripComboBox cbAPIManager;
        private System.Windows.Forms.ToolStripLabel lItem;
        private System.Windows.Forms.ToolStripComboBox cbItemType;
        private System.Windows.Forms.ToolStripButton bNewItem;
        private System.Windows.Forms.ToolStripLabel lInstance;
        private System.Windows.Forms.ToolStripComboBox cbInstance;
        private System.Windows.Forms.ToolStripButton bDelItem;
        private System.Windows.Forms.ToolStripButton bSaveItem;
        private System.Windows.Forms.FontDialog fontdlg;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cbOtherInstances;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}