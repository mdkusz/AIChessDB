namespace AIChessDatabase
{
    partial class EditMatchWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditMatchWindow));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.edMatch = new AIChessDatabase.Controls.EditMatchPanel();
            this.mmDisplay = new AIChessDatabase.Controls.MatchMoveDisplay();
            this.cPlayer = new AIChessDatabase.Controls.ChessPlayer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
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
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.cPlayer);
            this.splitContainer1.Size = new System.Drawing.Size(1697, 816);
            this.splitContainer1.SplitterDistance = 913;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.edMatch);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.mmDisplay);
            this.splitContainer2.Size = new System.Drawing.Size(913, 816);
            this.splitContainer2.SplitterDistance = 566;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 0;
            // 
            // edMatch
            // 
            this.edMatch.AllowSave = false;
            this.edMatch.ConnectionIndex = 0;
            this.edMatch.CurrentMatch = null;
            this.edMatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.edMatch.Location = new System.Drawing.Point(0, 0);
            this.edMatch.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.edMatch.Name = "edMatch";
            this.edMatch.Size = new System.Drawing.Size(913, 566);
            this.edMatch.TabIndex = 0;
            this.edMatch.CommentsChanged += new System.EventHandler(this.edMatch_CommentsChanged);
            // 
            // mmDisplay
            // 
            this.mmDisplay.AllowAddMoves = false;
            this.mmDisplay.AutoScroll = true;
            this.mmDisplay.BackColor = System.Drawing.Color.White;
            this.mmDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mmDisplay.Location = new System.Drawing.Point(0, 0);
            this.mmDisplay.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.mmDisplay.Name = "mmDisplay";
            this.mmDisplay.Player = this.cPlayer;
            this.mmDisplay.Size = new System.Drawing.Size(913, 245);
            this.mmDisplay.TabIndex = 0;
            // 
            // cPlayer
            // 
            this.cPlayer.AllowSetPosition = false;
            this.cPlayer.BackColor = System.Drawing.Color.SaddleBrown;
            this.cPlayer.ConnectionIndex = 0;
            this.cPlayer.CurrentMatch = null;
            this.cPlayer.Display = this.mmDisplay;
            this.cPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cPlayer.Keywords = false;
            this.cPlayer.Location = new System.Drawing.Point(0, 0);
            this.cPlayer.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cPlayer.Name = "cPlayer";
            this.cPlayer.NewComment = false;
            this.cPlayer.PosViewer = null;
            this.cPlayer.Size = new System.Drawing.Size(779, 816);
            this.cPlayer.TabIndex = 0;
            this.cPlayer.MoveChanged += new System.EventHandler(this.mmDisplay_MoveChanged);
            // 
            // EditMatchWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1697, 816);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "EditMatchWindow";
            this.Text = "EditMatchWindow";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EditMatchWindow_FormClosed);
            this.Load += new System.EventHandler(this.EditMatchWindow_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Controls.ChessPlayer cPlayer;
        private Controls.MatchMoveDisplay mmDisplay;
        private Controls.EditMatchPanel edMatch;
    }
}