namespace AIChessDatabase
{
    partial class PlayEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayEditor));
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.bFont = new System.Windows.Forms.ToolStripButton();
            this.lPlayList = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cbPlays = new System.Windows.Forms.ToolStripComboBox();
            this.bAddPlay = new System.Windows.Forms.ToolStripButton();
            this.bRemovePlay = new System.Windows.Forms.ToolStripButton();
            this.bPlayFromList = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bDefaultPlay = new System.Windows.Forms.ToolStripButton();
            this.bConsole = new System.Windows.Forms.ToolStripButton();
            this.bSaveConfiguration = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.lAdd = new System.Windows.Forms.ToolStripLabel();
            this.cbAddItem = new System.Windows.Forms.ToolStripComboBox();
            this.tsbAddItem = new System.Windows.Forms.ToolStripButton();
            this.tsbRemoveItem = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.rtvPlay = new DesktopControls.Controls.ReflectiveTreeView();
            this.flpPlay = new DesktopControls.Controls.InputEditors.InputEditorFlowContainer();
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
            this.panel1.Size = new System.Drawing.Size(1662, 40);
            this.panel1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cfPlay_ToolStrip_Description;
            this.toolStrip1.AccessibleName = global::AIChessDatabase.Properties.UIElements.cfPlay_ToolStrip_Name;
            this.toolStrip1.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bFont,
            this.lPlayList,
            this.toolStripSeparator1,
            this.cbPlays,
            this.bAddPlay,
            this.bRemovePlay,
            this.bPlayFromList,
            this.toolStripSeparator2,
            this.bDefaultPlay,
            this.bConsole,
            this.bSaveConfiguration,
            this.toolStripSeparator3,
            this.lAdd,
            this.cbAddItem,
            this.tsbAddItem,
            this.tsbRemoveItem});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1662, 33);
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
            // lPlayList
            // 
            this.lPlayList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lPlayList.Name = "lPlayList";
            this.lPlayList.Size = new System.Drawing.Size(56, 28);
            this.lPlayList.Text = "Plays";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // cbPlays
            // 
            this.cbPlays.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbPlays_Description;
            this.cbPlays.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbPlays_Name;
            this.cbPlays.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.cbPlays.AutoSize = false;
            this.cbPlays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPlays.Name = "cbPlays";
            this.cbPlays.Size = new System.Drawing.Size(136, 33);
            this.cbPlays.SelectedIndexChanged += new System.EventHandler(this.cbPlays_SelectedIndexChanged);
            // 
            // bAddPlay
            // 
            this.bAddPlay.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bAddPlay_Description;
            this.bAddPlay.AccessibleName = global::AIChessDatabase.Properties.UIElements.bAddPlay_Name;
            this.bAddPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bAddPlay.Image = ((System.Drawing.Image)(resources.GetObject("bAddPlay.Image")));
            this.bAddPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bAddPlay.Name = "bAddPlay";
            this.bAddPlay.Size = new System.Drawing.Size(34, 28);
            this.bAddPlay.Click += new System.EventHandler(this.bAddPlay_Click);
            // 
            // bRemovePlay
            // 
            this.bRemovePlay.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bRemovePlay_Description;
            this.bRemovePlay.AccessibleName = global::AIChessDatabase.Properties.UIElements.bRemovePlay_Name;
            this.bRemovePlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bRemovePlay.Enabled = false;
            this.bRemovePlay.Image = ((System.Drawing.Image)(resources.GetObject("bRemovePlay.Image")));
            this.bRemovePlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bRemovePlay.Name = "bRemovePlay";
            this.bRemovePlay.Size = new System.Drawing.Size(34, 28);
            this.bRemovePlay.Click += new System.EventHandler(this.bRemove_Click);
            // 
            // bPlayFromList
            // 
            this.bPlayFromList.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bPlayFromList_Description;
            this.bPlayFromList.AccessibleName = global::AIChessDatabase.Properties.UIElements.bPlayFromList_Name;
            this.bPlayFromList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bPlayFromList.Image = ((System.Drawing.Image)(resources.GetObject("bPlayFromList.Image")));
            this.bPlayFromList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bPlayFromList.Name = "bPlayFromList";
            this.bPlayFromList.Size = new System.Drawing.Size(34, 28);
            this.bPlayFromList.Click += new System.EventHandler(this.bPlayFromList_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 33);
            // 
            // bDefaultPlay
            // 
            this.bDefaultPlay.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bDefaultPlay_Description;
            this.bDefaultPlay.AccessibleName = global::AIChessDatabase.Properties.UIElements.bDefaultPlay_Name;
            this.bDefaultPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bDefaultPlay.Image = ((System.Drawing.Image)(resources.GetObject("bDefaultPlay.Image")));
            this.bDefaultPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bDefaultPlay.Name = "bDefaultPlay";
            this.bDefaultPlay.Size = new System.Drawing.Size(34, 28);
            this.bDefaultPlay.Click += new System.EventHandler(this.bDefaultPlay_Click);
            // 
            // bConsole
            // 
            this.bConsole.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bConsole_Description;
            this.bConsole.AccessibleName = global::AIChessDatabase.Properties.UIElements.bConsole_Name;
            this.bConsole.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bConsole.Image = ((System.Drawing.Image)(resources.GetObject("bConsole.Image")));
            this.bConsole.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bConsole.Name = "bConsole";
            this.bConsole.Size = new System.Drawing.Size(34, 28);
            this.bConsole.Click += new System.EventHandler(this.bConsole_Click);
            // 
            // bSaveConfiguration
            // 
            this.bSaveConfiguration.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bSaveConfiguration_Description;
            this.bSaveConfiguration.AccessibleName = global::AIChessDatabase.Properties.UIElements.bSaveConfiguration_Name;
            this.bSaveConfiguration.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bSaveConfiguration.Image = ((System.Drawing.Image)(resources.GetObject("bSaveConfiguration.Image")));
            this.bSaveConfiguration.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bSaveConfiguration.Name = "bSaveConfiguration";
            this.bSaveConfiguration.Size = new System.Drawing.Size(34, 28);
            this.bSaveConfiguration.Click += new System.EventHandler(this.bSaveConfiguration_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 33);
            // 
            // lAdd
            // 
            this.lAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lAdd.Name = "lAdd";
            this.lAdd.Size = new System.Drawing.Size(47, 28);
            this.lAdd.Text = "Add";
            // 
            // cbAddItem
            // 
            this.cbAddItem.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbAddItem_Description;
            this.cbAddItem.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbAddItem_Name;
            this.cbAddItem.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.cbAddItem.AutoSize = false;
            this.cbAddItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAddItem.Name = "cbAddItem";
            this.cbAddItem.Size = new System.Drawing.Size(136, 33);
            this.cbAddItem.SelectedIndexChanged += new System.EventHandler(this.cbAddItem_SelectedIndexChanged);
            // 
            // tsbAddItem
            // 
            this.tsbAddItem.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.tsbAddItem_Description;
            this.tsbAddItem.AccessibleName = global::AIChessDatabase.Properties.UIElements.tsbAddItem_Name;
            this.tsbAddItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddItem.Enabled = false;
            this.tsbAddItem.Image = ((System.Drawing.Image)(resources.GetObject("tsbAddItem.Image")));
            this.tsbAddItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddItem.Name = "tsbAddItem";
            this.tsbAddItem.Size = new System.Drawing.Size(34, 28);
            this.tsbAddItem.Click += new System.EventHandler(this.tsbAddItem_Click);
            // 
            // tsbRemoveItem
            // 
            this.tsbRemoveItem.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.tsbRemoveItem_Description;
            this.tsbRemoveItem.AccessibleName = global::AIChessDatabase.Properties.UIElements.tsbRemoveItem_Name;
            this.tsbRemoveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRemoveItem.Enabled = false;
            this.tsbRemoveItem.Image = ((System.Drawing.Image)(resources.GetObject("tsbRemoveItem.Image")));
            this.tsbRemoveItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRemoveItem.Name = "tsbRemoveItem";
            this.tsbRemoveItem.Size = new System.Drawing.Size(34, 28);
            this.tsbRemoveItem.Click += new System.EventHandler(this.tsbRemoveItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 40);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.rtvPlay);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.flpPlay);
            this.splitContainer1.Size = new System.Drawing.Size(1662, 841);
            this.splitContainer1.SplitterDistance = 810;
            this.splitContainer1.TabIndex = 1;
            // 
            // rtvPlay
            // 
            this.rtvPlay.AccessibleDescription = "";
            this.rtvPlay.AccessibleName = global::AIChessDatabase.Properties.UIElements.rtvPlay_Name;
            this.rtvPlay.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.rtvPlay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtvPlay.Location = new System.Drawing.Point(0, 0);
            this.rtvPlay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rtvPlay.Name = "rtvPlay";
            this.rtvPlay.ShowNodeToolTips = true;
            this.rtvPlay.Size = new System.Drawing.Size(810, 841);
            this.rtvPlay.TabIndex = 0;
            this.rtvPlay.TreeObject = null;
            this.rtvPlay.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.rtvPlay_AfterSelect);
            // 
            // flpPlay
            // 
            this.flpPlay.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.flpPlay_Description;
            this.flpPlay.AccessibleName = global::AIChessDatabase.Properties.UIElements.flpPlay_Name;
            this.flpPlay.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
            this.flpPlay.AutoScroll = true;
            this.flpPlay.BackColor = System.Drawing.SystemColors.Desktop;
            this.flpPlay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpPlay.EditorFactory = null;
            this.flpPlay.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpPlay.Location = new System.Drawing.Point(0, 0);
            this.flpPlay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.flpPlay.Name = "flpPlay";
            this.flpPlay.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.flpPlay.Size = new System.Drawing.Size(848, 841);
            this.flpPlay.TabIndex = 5;
            this.flpPlay.WrapContents = false;
            // 
            // PlayEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1662, 881);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "PlayEditor";
            this.Text = "cfPlay";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PlayEditor_FormClosing);
            this.Load += new System.EventHandler(this.cfPlay_Load);
            this.Shown += new System.EventHandler(this.PlayEditor_Shown);
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
        private DesktopControls.Controls.ReflectiveTreeView rtvPlay;
        private DesktopControls.Controls.InputEditors.InputEditorFlowContainer flpPlay;
        private System.Windows.Forms.ToolStripButton bFont;
        private System.Windows.Forms.ToolStripLabel lPlayList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox cbPlays;
        private System.Windows.Forms.ToolStripButton bPlayFromList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton bSaveConfiguration;
        private System.Windows.Forms.FontDialog fontdlg;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel lAdd;
        private System.Windows.Forms.ToolStripComboBox cbAddItem;
        private System.Windows.Forms.ToolStripButton tsbAddItem;
        private System.Windows.Forms.ToolStripButton tsbRemoveItem;
        private System.Windows.Forms.ToolStripButton bAddPlay;
        private System.Windows.Forms.ToolStripButton bRemovePlay;
        private System.Windows.Forms.ToolStripButton bDefaultPlay;
        private System.Windows.Forms.ToolStripButton bConsole;
    }
}