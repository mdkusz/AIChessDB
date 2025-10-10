using AIChessDatabase.Properties;
using AIChessDatabase.Interfaces;

namespace AIChessDatabase
{
    partial class QueryWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryWindow));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dgMatches = new QueryDesktopControls.Controls.QueryDataGridPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lFilter = new System.Windows.Forms.ToolStripLabel();
            this.bFilterByTag = new System.Windows.Forms.ToolStripButton();
            this.bFilterByPos = new System.Windows.Forms.ToolStripButton();
            this.bFilterByMov = new System.Windows.Forms.ToolStripButton();
            this.bDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bShow = new System.Windows.Forms.ToolStripButton();
            this.bTagSel = new System.Windows.Forms.ToolStripButton();
            this.bEditMatch = new System.Windows.Forms.ToolStripButton();
            this.bPlay = new System.Windows.Forms.ToolStripButton();
            this.bDelMatch = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cbDatabases = new System.Windows.Forms.ToolStripComboBox();
            this.pbExport = new System.Windows.Forms.ToolStripProgressBar();
            this.lMatchCount = new System.Windows.Forms.ToolStripLabel();
            this.mDisplay = new AIChessDatabase.Controls.MatchMoveDisplay();
            this.cPlayer = new AIChessDatabase.Controls.ChessPlayer();
            this.ofDlg = new System.Windows.Forms.OpenFileDialog();
            this.sfDlg = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.cPlayer);
            this.splitContainer1.Size = new System.Drawing.Size(1929, 1050);
            this.splitContainer1.SplitterDistance = 1142;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.dgMatches);
            this.splitContainer2.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.mDisplay);
            this.splitContainer2.Size = new System.Drawing.Size(1142, 1050);
            this.splitContainer2.SplitterDistance = 695;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 0;
            // 
            // dgMatches
            // 
            this.dgMatches.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.dgMatches_Description;
            this.dgMatches.AccessibleName = global::AIChessDatabase.Properties.UIElements.dgMatches_Name;
            this.dgMatches.AccessibleRole = System.Windows.Forms.AccessibleRole.Table;
            this.dgMatches.ColumnManager = null;
            this.dgMatches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgMatches.FOManager = null;
            this.dgMatches.Location = new System.Drawing.Point(0, 31);
            this.dgMatches.Margin = new System.Windows.Forms.Padding(4);
            this.dgMatches.Name = "dgMatches";
            this.dgMatches.Size = new System.Drawing.Size(1142, 664);
            this.dgMatches.Style = ((QueryDesktopControls.Controls.QueryDataGridPanel.GridPanelStyle)(((QueryDesktopControls.Controls.QueryDataGridPanel.GridPanelStyle.ManageQuery | QueryDesktopControls.Controls.QueryDataGridPanel.GridPanelStyle.ExportData) 
            | QueryDesktopControls.Controls.QueryDataGridPanel.GridPanelStyle.ChangeFormat)));
            this.dgMatches.TabIndex = 3;
            this.dgMatches.Title = "";
            this.dgMatches.QueryChanged += new System.EventHandler(this.dgMatches_QueryChanged);
            this.dgMatches.SelectionChanged += new System.EventHandler(this.dgMatches_SelectionChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1142, 31);
            this.panel1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.QueryWindow_toolStrip1_Description;
            this.toolStrip1.AccessibleName = global::AIChessDatabase.Properties.UIElements.QueryWindow_toolStrip1_Name;
            this.toolStrip1.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lFilter,
            this.bFilterByTag,
            this.bFilterByPos,
            this.bFilterByMov,
            this.bDelete,
            this.toolStripSeparator1,
            this.bShow,
            this.bTagSel,
            this.bEditMatch,
            this.bPlay,
            this.bDelMatch,
            this.toolStripSeparator2,
            this.cbDatabases,
            this.pbExport,
            this.lMatchCount});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1142, 31);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // lFilter
            // 
            this.lFilter.Name = "lFilter";
            this.lFilter.Size = new System.Drawing.Size(42, 28);
            this.lFilter.Text = global::AIChessDatabase.Properties.UIResources.LAB_FILTER;
            // 
            // bFilterByTag
            // 
            this.bFilterByTag.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bFilterByTag_Description;
            this.bFilterByTag.AccessibleName = global::AIChessDatabase.Properties.UIElements.bFilterByTag_Name;
            this.bFilterByTag.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.bFilterByTag.CheckOnClick = true;
            this.bFilterByTag.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bFilterByTag.Image = ((System.Drawing.Image)(resources.GetObject("bFilterByTag.Image")));
            this.bFilterByTag.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bFilterByTag.Name = "bFilterByTag";
            this.bFilterByTag.Size = new System.Drawing.Size(29, 28);
            this.bFilterByTag.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_FILTERBYTAG;
            this.bFilterByTag.Click += new System.EventHandler(this.bFilterByTag_Click);
            // 
            // bFilterByPos
            // 
            this.bFilterByPos.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bFilterByPos_Description;
            this.bFilterByPos.AccessibleName = global::AIChessDatabase.Properties.UIElements.bFilterByPos_Name;
            this.bFilterByPos.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.bFilterByPos.CheckOnClick = true;
            this.bFilterByPos.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bFilterByPos.Image = ((System.Drawing.Image)(resources.GetObject("bFilterByPos.Image")));
            this.bFilterByPos.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bFilterByPos.Name = "bFilterByPos";
            this.bFilterByPos.Size = new System.Drawing.Size(29, 28);
            this.bFilterByPos.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_FILTERBYPOS;
            this.bFilterByPos.Click += new System.EventHandler(this.bFilterByTag_Click);
            // 
            // bFilterByMov
            // 
            this.bFilterByMov.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bFilterByMov_Description;
            this.bFilterByMov.AccessibleName = global::AIChessDatabase.Properties.UIElements.bFilterByMov_Name;
            this.bFilterByMov.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.bFilterByMov.CheckOnClick = true;
            this.bFilterByMov.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bFilterByMov.Image = ((System.Drawing.Image)(resources.GetObject("bFilterByMov.Image")));
            this.bFilterByMov.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bFilterByMov.Name = "bFilterByMov";
            this.bFilterByMov.Size = new System.Drawing.Size(29, 28);
            this.bFilterByMov.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_FILTERBYMOVE;
            this.bFilterByMov.Click += new System.EventHandler(this.bFilterByTag_Click);
            // 
            // bDelete
            // 
            this.bDelete.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.QueryWindow_bDelete_Description;
            this.bDelete.AccessibleName = global::AIChessDatabase.Properties.UIElements.QueryWindow_bDelete_Name;
            this.bDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bDelete.Enabled = false;
            this.bDelete.Image = ((System.Drawing.Image)(resources.GetObject("bDelete.Image")));
            this.bDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(29, 28);
            this.bDelete.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_DELRESULTS;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // bShow
            // 
            this.bShow.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bShow_Description;
            this.bShow.AccessibleName = global::AIChessDatabase.Properties.UIElements.bShow_Name;
            this.bShow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bShow.Enabled = false;
            this.bShow.Image = ((System.Drawing.Image)(resources.GetObject("bShow.Image")));
            this.bShow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bShow.Name = "bShow";
            this.bShow.Size = new System.Drawing.Size(29, 28);
            this.bShow.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_VIEWMATCH;
            this.bShow.Click += new System.EventHandler(this.bShow_Click);
            // 
            // bTagSel
            // 
            this.bTagSel.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bTagSel_Description;
            this.bTagSel.AccessibleName = global::AIChessDatabase.Properties.UIElements.bTagSel_Name;
            this.bTagSel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bTagSel.Enabled = false;
            this.bTagSel.Image = ((System.Drawing.Image)(resources.GetObject("bTagSel.Image")));
            this.bTagSel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bTagSel.Name = "bTagSel";
            this.bTagSel.Size = new System.Drawing.Size(29, 28);
            this.bTagSel.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_LABELMATCHES;
            this.bTagSel.Click += new System.EventHandler(this.bTagSel_Click);
            // 
            // bEditMatch
            // 
            this.bEditMatch.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bEditMatch_Description;
            this.bEditMatch.AccessibleName = global::AIChessDatabase.Properties.UIElements.bEditMatch_Name;
            this.bEditMatch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bEditMatch.Enabled = false;
            this.bEditMatch.Image = ((System.Drawing.Image)(resources.GetObject("bEditMatch.Image")));
            this.bEditMatch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bEditMatch.Name = "bEditMatch";
            this.bEditMatch.Size = new System.Drawing.Size(29, 28);
            this.bEditMatch.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_EDITMATCH;
            this.bEditMatch.Click += new System.EventHandler(this.bEditMatch_Click);
            // 
            // bPlay
            // 
            this.bPlay.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bPlay_Description;
            this.bPlay.AccessibleName = global::AIChessDatabase.Properties.UIElements.bPlay_Name;
            this.bPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bPlay.Enabled = false;
            this.bPlay.Image = ((System.Drawing.Image)(resources.GetObject("bPlay.Image")));
            this.bPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bPlay.Name = "bPlay";
            this.bPlay.Size = new System.Drawing.Size(29, 28);
            this.bPlay.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_PLAYMATCH;
            this.bPlay.Click += new System.EventHandler(this.bPlay_Click);
            // 
            // bDelMatch
            // 
            this.bDelMatch.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bDelMatch_Description;
            this.bDelMatch.AccessibleName = global::AIChessDatabase.Properties.UIElements.bDelMatch_Name;
            this.bDelMatch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bDelMatch.Enabled = false;
            this.bDelMatch.Image = ((System.Drawing.Image)(resources.GetObject("bDelMatch.Image")));
            this.bDelMatch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bDelMatch.Name = "bDelMatch";
            this.bDelMatch.Size = new System.Drawing.Size(29, 28);
            this.bDelMatch.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_DELMATCH;
            this.bDelMatch.Click += new System.EventHandler(this.bDelMatch_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // cbDatabases
            // 
            this.cbDatabases.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbDatabases_Description;
            this.cbDatabases.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbDatabases_Name;
            this.cbDatabases.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.cbDatabases.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDatabases.Name = "cbDatabases";
            this.cbDatabases.Size = new System.Drawing.Size(199, 31);
            this.cbDatabases.SelectedIndexChanged += new System.EventHandler(this.cbDatabases_SelectedIndexChanged);
            // 
            // pbExport
            // 
            this.pbExport.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.pbExport_Description;
            this.pbExport.AccessibleName = global::AIChessDatabase.Properties.UIElements.pbExport_Name;
            this.pbExport.AccessibleRole = System.Windows.Forms.AccessibleRole.ProgressBar;
            this.pbExport.Name = "pbExport";
            this.pbExport.Size = new System.Drawing.Size(150, 28);
            // 
            // lMatchCount
            // 
            this.lMatchCount.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.lMatchCount_Description;
            this.lMatchCount.AccessibleName = global::AIChessDatabase.Properties.UIElements.lMatchCount_Name;
            this.lMatchCount.AccessibleRole = System.Windows.Forms.AccessibleRole.StaticText;
            this.lMatchCount.Name = "lMatchCount";
            this.lMatchCount.Size = new System.Drawing.Size(93, 28);
            this.lMatchCount.Text = "Match Count";
            // 
            // mDisplay
            // 
            this.mDisplay.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.mDisplay_Description;
            this.mDisplay.AccessibleName = global::AIChessDatabase.Properties.UIElements.mDisplay_Name;
            this.mDisplay.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
            this.mDisplay.AllowAddMoves = false;
            this.mDisplay.AutoScroll = true;
            this.mDisplay.BackColor = System.Drawing.Color.White;
            this.mDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mDisplay.Location = new System.Drawing.Point(0, 0);
            this.mDisplay.Margin = new System.Windows.Forms.Padding(5);
            this.mDisplay.Name = "mDisplay";
            this.mDisplay.Player = this.cPlayer;
            this.mDisplay.Size = new System.Drawing.Size(1142, 350);
            this.mDisplay.TabIndex = 0;
            // 
            // cPlayer
            // 
            this.cPlayer.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cPlayer_Description;
            this.cPlayer.AccessibleName = global::AIChessDatabase.Properties.UIElements.cPlayer_Name;
            this.cPlayer.AllowSetPosition = false;
            this.cPlayer.BackColor = System.Drawing.Color.SaddleBrown;
            this.cPlayer.ConnectionIndex = 0;
            this.cPlayer.CurrentMatch = null;
            this.cPlayer.Display = this.mDisplay;
            this.cPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cPlayer.Keywords = true;
            this.cPlayer.Location = new System.Drawing.Point(0, 0);
            this.cPlayer.Margin = new System.Windows.Forms.Padding(5);
            this.cPlayer.Name = "cPlayer";
            this.cPlayer.NewComment = true;
            this.cPlayer.PosViewer = null;
            this.cPlayer.Size = new System.Drawing.Size(782, 1050);
            this.cPlayer.TabIndex = 0;
            // 
            // QueryWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1929, 1050);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "QueryWindow";
            this.Text = "QueryWindow";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.QueryWindow_FormClosed);
            this.Load += new System.EventHandler(this.QueryWindow_Load);
            this.Shown += new System.EventHandler(this.QueryWindow_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void Grid_BeforeRefreshing(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private QueryDesktopControls.Controls.QueryDataGridPanel dgMatches;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Controls.ChessPlayer cPlayer;
        private Controls.MatchMoveDisplay mDisplay;
        private System.Windows.Forms.ToolStripLabel lFilter;
        private System.Windows.Forms.ToolStripButton bShow;
        private System.Windows.Forms.OpenFileDialog ofDlg;
        private System.Windows.Forms.SaveFileDialog sfDlg;
        private System.Windows.Forms.ToolStripButton bDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton bEditMatch;
        private System.Windows.Forms.ToolStripButton bDelMatch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripComboBox cbDatabases;
        private System.Windows.Forms.ToolStripProgressBar pbExport;
        private System.Windows.Forms.ToolStripButton bPlay;
        private System.Windows.Forms.ToolStripButton bTagSel;
        private System.Windows.Forms.ToolStripButton bFilterByTag;
        private System.Windows.Forms.ToolStripButton bFilterByPos;
        private System.Windows.Forms.ToolStripButton bFilterByMov;
        private System.Windows.Forms.ToolStripLabel lMatchCount;
    }
}