namespace AIChessDatabase
{
    partial class PlayMatchWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayMatchWindow));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.edMatch = new AIChessDatabase.Controls.EditMatchPanel();
            this.pViewer = new AIChessDatabase.Controls.PositionViewer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.cPlayer = new AIChessDatabase.Controls.ChessPlayer();
            this.mmDisplay = new AIChessDatabase.Controls.MatchMoveDisplay();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1956, 1311);
            this.splitContainer1.SplitterDistance = 962;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.edMatch);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.pViewer);
            this.splitContainer3.Size = new System.Drawing.Size(962, 1311);
            this.splitContainer3.SplitterDistance = 713;
            this.splitContainer3.SplitterWidth = 6;
            this.splitContainer3.TabIndex = 0;
            // 
            // edMatch
            // 
            this.edMatch.AllowSave = false;
            this.edMatch.ConnectionIndex = 0;
            this.edMatch.CurrentMatch = null;
            this.edMatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.edMatch.Location = new System.Drawing.Point(0, 0);
            this.edMatch.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.edMatch.Name = "edMatch";
            this.edMatch.Size = new System.Drawing.Size(962, 713);
            this.edMatch.TabIndex = 0;
            this.edMatch.CommentsChanged += new System.EventHandler(this.edMatch_CommentsChanged);
            // 
            // pViewer
            // 
            this.pViewer.BoardPosition = "";
            this.pViewer.Color = true;
            this.pViewer.ConnectionIndex = 0;
            this.pViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pViewer.Location = new System.Drawing.Point(0, 0);
            this.pViewer.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pViewer.Name = "pViewer";
            this.pViewer.Repository = null;
            this.pViewer.Side = false;
            this.pViewer.Size = new System.Drawing.Size(962, 592);
            this.pViewer.TabIndex = 0;
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
            this.splitContainer2.Panel1.Controls.Add(this.cPlayer);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.mmDisplay);
            this.splitContainer2.Size = new System.Drawing.Size(988, 1311);
            this.splitContainer2.SplitterDistance = 889;
            this.splitContainer2.SplitterWidth = 6;
            this.splitContainer2.TabIndex = 0;
            // 
            // cPlayer
            // 
            this.cPlayer.AddComments = false;
            this.cPlayer.AllowSetPosition = true;
            this.cPlayer.BackColor = System.Drawing.Color.SaddleBrown;
            this.cPlayer.ConnectionIndex = 0;
            this.cPlayer.CurrentMatch = null;
            this.cPlayer.Display = this.mmDisplay;
            this.cPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cPlayer.Keywords = false;
            this.cPlayer.Location = new System.Drawing.Point(0, 0);
            this.cPlayer.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.cPlayer.Name = "cPlayer";
            this.cPlayer.NewComment = false;
            this.cPlayer.PosViewer = this.pViewer;
            this.cPlayer.Size = new System.Drawing.Size(988, 889);
            this.cPlayer.TabIndex = 0;
            // 
            // mmDisplay
            // 
            this.mmDisplay.AllowAddMoves = true;
            this.mmDisplay.AutoScroll = true;
            this.mmDisplay.BackColor = System.Drawing.Color.White;
            this.mmDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mmDisplay.Location = new System.Drawing.Point(0, 0);
            this.mmDisplay.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.mmDisplay.Name = "mmDisplay";
            this.mmDisplay.Player = this.cPlayer;
            this.mmDisplay.Size = new System.Drawing.Size(988, 416);
            this.mmDisplay.TabIndex = 0;
            this.mmDisplay.MoveChanged += new System.EventHandler(this.mmDisplay_MoveChanged);
            this.mmDisplay.MatchFinished += new System.EventHandler(this.mmDisplay_MatchFinished);
            // 
            // PlayMatchWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1956, 1311);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "PlayMatchWindow";
            this.Text = "PlayMatchWindow";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PlayMatchWindow_FormClosed);
            this.Load += new System.EventHandler(this.PlayMatchWindow_Load);
            this.Shown += new System.EventHandler(this.PlayMatchWindow_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Controls.ChessPlayer cPlayer;
        private Controls.MatchMoveDisplay mmDisplay;
        private Controls.EditMatchPanel edMatch;
        private Controls.PositionViewer pViewer;
    }
}