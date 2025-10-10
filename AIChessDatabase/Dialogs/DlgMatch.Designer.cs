namespace AIChessDatabase.Dialogs
{
    partial class DlgMatch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgMatch));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cPlayer = new AIChessDatabase.Controls.ChessPlayer();
            this.mmDisplay = new AIChessDatabase.Controls.MatchMoveDisplay();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.cPlayer);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.mmDisplay);
            this.splitContainer1.Size = new System.Drawing.Size(578, 877);
            this.splitContainer1.SplitterDistance = 666;
            this.splitContainer1.TabIndex = 0;
            // 
            // cPlayer
            // 
            this.cPlayer.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
            this.cPlayer.AccessibleName = global::AIChessDatabase.Properties.UIElements.cPlayer_Name;
            this.cPlayer.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cPlayer_Description;
            this.cPlayer.AllowSetPosition = false;
            this.cPlayer.BackColor = System.Drawing.Color.SaddleBrown;
            this.cPlayer.Display = this.mmDisplay;
            this.cPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cPlayer.Keywords = true;
            this.cPlayer.Location = new System.Drawing.Point(0, 0);
            this.cPlayer.Name = "cPlayer";
            this.cPlayer.NewComment = false;
            this.cPlayer.PosViewer = null;
            this.cPlayer.Size = new System.Drawing.Size(578, 666);
            this.cPlayer.TabIndex = 0;
            // 
            // mmDisplay
            // 
            this.mmDisplay.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
            this.mmDisplay.AccessibleName = global::AIChessDatabase.Properties.UIElements.mmDisplay_Name;
            this.mmDisplay.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.mmDisplay_Description;
            this.mmDisplay.AllowAddMoves = false;
            this.mmDisplay.AutoScroll = true;
            this.mmDisplay.BackColor = System.Drawing.Color.White;
            this.mmDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mmDisplay.Location = new System.Drawing.Point(0, 0);
            this.mmDisplay.Name = "mmDisplay";
            this.mmDisplay.Player = this.cPlayer;
            this.mmDisplay.Size = new System.Drawing.Size(578, 207);
            this.mmDisplay.TabIndex = 0;
            // 
            // DlgMatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 877);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DlgMatch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DlgMatch";
            this.TopMost = true;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Controls.ChessPlayer cPlayer;
        private Controls.MatchMoveDisplay mmDisplay;
    }
}