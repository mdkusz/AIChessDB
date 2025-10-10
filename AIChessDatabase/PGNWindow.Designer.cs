using AIChessDatabase.Properties;

namespace AIChessDatabase
{
    partial class PGNWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PGNWindow));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dgMatches = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sel = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.white = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.black = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ply = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.moves = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsData = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.bView = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bCheck = new System.Windows.Forms.ToolStripButton();
            this.bSelAll = new System.Windows.Forms.ToolStripButton();
            this.bUncheck = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bInsert = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.lMatchCount = new System.Windows.Forms.ToolStripLabel();
            this.pgInsert = new System.Windows.Forms.ToolStripProgressBar();
            this.moveDisplay = new AIChessDatabase.Controls.MatchMoveDisplay();
            this.chessPlayer = new AIChessDatabase.Controls.ChessPlayer();
            this.sfDlg = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMatches)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsData)).BeginInit();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.chessPlayer);
            this.splitContainer1.Size = new System.Drawing.Size(1894, 885);
            this.splitContainer1.SplitterDistance = 1036;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
            this.splitContainer2.Panel2.Controls.Add(this.moveDisplay);
            this.splitContainer2.Size = new System.Drawing.Size(1036, 885);
            this.splitContainer2.SplitterDistance = 430;
            this.splitContainer2.SplitterWidth = 6;
            this.splitContainer2.TabIndex = 0;
            // 
            // dgMatches
            // 
            this.dgMatches.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.PGNdgMatches_Description;
            this.dgMatches.AccessibleName = global::AIChessDatabase.Properties.UIElements.dgMatches_Name;
            this.dgMatches.AccessibleRole = System.Windows.Forms.AccessibleRole.Table;
            this.dgMatches.AllowUserToAddRows = false;
            this.dgMatches.AllowUserToDeleteRows = false;
            this.dgMatches.AutoGenerateColumns = false;
            this.dgMatches.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgMatches.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgMatches.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.sel,
            this.description,
            this.date,
            this.white,
            this.black,
            this.result,
            this.ply,
            this.moves});
            this.dgMatches.DataSource = this.bsData;
            this.dgMatches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgMatches.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgMatches.Location = new System.Drawing.Point(0, 39);
            this.dgMatches.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgMatches.Name = "dgMatches";
            this.dgMatches.ReadOnly = true;
            this.dgMatches.RowHeadersWidth = 51;
            this.dgMatches.RowTemplate.Height = 18;
            this.dgMatches.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgMatches.Size = new System.Drawing.Size(1036, 391);
            this.dgMatches.TabIndex = 2;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "id";
            this.ID.HeaderText = "Id";
            this.ID.MinimumWidth = 6;
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Visible = false;
            this.ID.Width = 41;
            // 
            // sel
            // 
            this.sel.DataPropertyName = "selected";
            this.sel.FalseValue = "0";
            this.sel.HeaderText = global::AIChessDatabase.Properties.UIResources.COL_SELECTED;
            this.sel.MinimumWidth = 6;
            this.sel.Name = "sel";
            this.sel.ReadOnly = true;
            this.sel.TrueValue = "1";
            this.sel.Width = 78;
            // 
            // description
            // 
            this.description.DataPropertyName = "match";
            this.description.HeaderText = global::AIChessDatabase.Properties.UIResources.COL_MATCH;
            this.description.MinimumWidth = 6;
            this.description.Name = "description";
            this.description.ReadOnly = true;
            this.description.Width = 89;
            // 
            // date
            // 
            this.date.DataPropertyName = "date";
            this.date.HeaderText = global::AIChessDatabase.Properties.UIResources.COL_DATE;
            this.date.MinimumWidth = 6;
            this.date.Name = "date";
            this.date.ReadOnly = true;
            this.date.Width = 80;
            // 
            // white
            // 
            this.white.DataPropertyName = "white";
            this.white.HeaderText = global::AIChessDatabase.Properties.UIResources.COL_WHITE;
            this.white.MinimumWidth = 6;
            this.white.Name = "white";
            this.white.ReadOnly = true;
            this.white.Width = 86;
            // 
            // black
            // 
            this.black.DataPropertyName = "black";
            this.black.HeaderText = global::AIChessDatabase.Properties.UIResources.COL_BLACK;
            this.black.MinimumWidth = 6;
            this.black.Name = "black";
            this.black.ReadOnly = true;
            this.black.Width = 84;
            // 
            // result
            // 
            this.result.DataPropertyName = "result";
            this.result.HeaderText = global::AIChessDatabase.Properties.UIResources.COL_RESULT;
            this.result.MinimumWidth = 6;
            this.result.Name = "result";
            this.result.ReadOnly = true;
            this.result.Width = 91;
            // 
            // ply
            // 
            this.ply.DataPropertyName = "moves";
            this.ply.HeaderText = global::AIChessDatabase.Properties.UIResources.COL_MOVES;
            this.ply.MinimumWidth = 6;
            this.ply.Name = "ply";
            this.ply.ReadOnly = true;
            this.ply.Width = 65;
            // 
            // moves
            // 
            this.moves.DataPropertyName = "fullmoves";
            this.moves.HeaderText = global::AIChessDatabase.Properties.UIResources.COL_FMOVES;
            this.moves.MinimumWidth = 6;
            this.moves.Name = "moves";
            this.moves.ReadOnly = true;
            this.moves.Width = 91;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1036, 39);
            this.panel1.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.PGN_toolStrip1_Description;
            this.toolStrip1.AccessibleName = global::AIChessDatabase.Properties.UIElements.PGN_toolStrip1_Name;
            this.toolStrip1.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bView,
            this.toolStripSeparator1,
            this.bCheck,
            this.bSelAll,
            this.bUncheck,
            this.toolStripSeparator2,
            this.bInsert,
            this.toolStripSeparator3,
            this.lMatchCount,
            this.pgInsert});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1036, 43);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // bView
            // 
            this.bView.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bView_Description;
            this.bView.AccessibleName = global::AIChessDatabase.Properties.UIElements.bView_Name;
            this.bView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bView.Image = ((System.Drawing.Image)(resources.GetObject("bView.Image")));
            this.bView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bView.Name = "bView";
            this.bView.Size = new System.Drawing.Size(34, 38);
            this.bView.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_VIEWMATCH;
            this.bView.Click += new System.EventHandler(this.bView_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 43);
            // 
            // bCheck
            // 
            this.bCheck.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bCheck_Description;
            this.bCheck.AccessibleName = global::AIChessDatabase.Properties.UIElements.bCheck_Name;
            this.bCheck.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bCheck.Image = ((System.Drawing.Image)(resources.GetObject("bCheck.Image")));
            this.bCheck.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bCheck.Name = "bCheck";
            this.bCheck.Size = new System.Drawing.Size(34, 38);
            this.bCheck.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_CHECKROWS;
            this.bCheck.Click += new System.EventHandler(this.bCheck_Click);
            // 
            // bSelAll
            // 
            this.bSelAll.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bSelAll_Description;
            this.bSelAll.AccessibleName = global::AIChessDatabase.Properties.UIElements.bSelAll_Name;
            this.bSelAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bSelAll.Image = ((System.Drawing.Image)(resources.GetObject("bSelAll.Image")));
            this.bSelAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bSelAll.Name = "bSelAll";
            this.bSelAll.Size = new System.Drawing.Size(34, 38);
            this.bSelAll.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_SELALL;
            this.bSelAll.Click += new System.EventHandler(this.bSelAll_Click);
            // 
            // bUncheck
            // 
            this.bUncheck.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bUncheck_Description;
            this.bUncheck.AccessibleName = global::AIChessDatabase.Properties.UIElements.bUncheck_Name;
            this.bUncheck.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bUncheck.Image = ((System.Drawing.Image)(resources.GetObject("bUncheck.Image")));
            this.bUncheck.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bUncheck.Name = "bUncheck";
            this.bUncheck.Size = new System.Drawing.Size(34, 38);
            this.bUncheck.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_UNCHECKROWS;
            this.bUncheck.Click += new System.EventHandler(this.bUncheck_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 43);
            // 
            // bInsert
            // 
            this.bInsert.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bInsert_Description;
            this.bInsert.AccessibleName = global::AIChessDatabase.Properties.UIElements.bInsert_Name;
            this.bInsert.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bInsert.Image = ((System.Drawing.Image)(resources.GetObject("bInsert.Image")));
            this.bInsert.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bInsert.Name = "bInsert";
            this.bInsert.Size = new System.Drawing.Size(34, 38);
            this.bInsert.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_INSERTCHECKED;
            this.bInsert.Click += new System.EventHandler(this.bInsert_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 43);
            // 
            // lMatchCount
            // 
            this.lMatchCount.Name = "lMatchCount";
            this.lMatchCount.Size = new System.Drawing.Size(0, 38);
            // 
            // pgInsert
            // 
            this.pgInsert.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.pgInsert_Description;
            this.pgInsert.AccessibleName = global::AIChessDatabase.Properties.UIElements.pgInsert_Name;
            this.pgInsert.AccessibleRole = System.Windows.Forms.AccessibleRole.ProgressBar;
            this.pgInsert.Name = "pgInsert";
            this.pgInsert.Size = new System.Drawing.Size(225, 38);
            this.pgInsert.Step = 1;
            // 
            // moveDisplay
            // 
            this.moveDisplay.AllowAddMoves = false;
            this.moveDisplay.AutoScroll = true;
            this.moveDisplay.BackColor = System.Drawing.Color.White;
            this.moveDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.moveDisplay.Location = new System.Drawing.Point(0, 0);
            this.moveDisplay.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.moveDisplay.Name = "moveDisplay";
            this.moveDisplay.Player = this.chessPlayer;
            this.moveDisplay.Size = new System.Drawing.Size(1036, 449);
            this.moveDisplay.TabIndex = 0;
            // 
            // chessPlayer
            // 
            this.chessPlayer.AddComments = false;
            this.chessPlayer.AllowSetPosition = false;
            this.chessPlayer.BackColor = System.Drawing.Color.SaddleBrown;
            this.chessPlayer.ConnectionIndex = 0;
            this.chessPlayer.CurrentMatch = null;
            this.chessPlayer.Display = this.moveDisplay;
            this.chessPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chessPlayer.Keywords = false;
            this.chessPlayer.Location = new System.Drawing.Point(0, 0);
            this.chessPlayer.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.chessPlayer.Name = "chessPlayer";
            this.chessPlayer.NewComment = false;
            this.chessPlayer.PosViewer = null;
            this.chessPlayer.Size = new System.Drawing.Size(852, 885);
            this.chessPlayer.TabIndex = 0;
            // 
            // PGNWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1894, 885);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "PGNWindow";
            this.Text = "PGNWindow";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PGNWindow_FormClosed);
            this.Load += new System.EventHandler(this.PGNWindow_Load);
            this.Shown += new System.EventHandler(this.PGNWindow_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgMatches)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsData)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.BindingSource bsData;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgMatches;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton bView;
        private Controls.ChessPlayer chessPlayer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton bCheck;
        private System.Windows.Forms.ToolStripButton bUncheck;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton bInsert;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel lMatchCount;
        private Controls.MatchMoveDisplay moveDisplay;
        private System.Windows.Forms.ToolStripButton bSelAll;
        private System.Windows.Forms.ToolStripProgressBar pgInsert;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn sel;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn date;
        private System.Windows.Forms.DataGridViewTextBoxColumn white;
        private System.Windows.Forms.DataGridViewTextBoxColumn black;
        private System.Windows.Forms.DataGridViewTextBoxColumn result;
        private System.Windows.Forms.DataGridViewTextBoxColumn ply;
        private System.Windows.Forms.DataGridViewTextBoxColumn moves;
        private System.Windows.Forms.SaveFileDialog sfDlg;
    }
}