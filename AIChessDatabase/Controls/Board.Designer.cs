using AIChessDatabase.Properties;

namespace AIChessDatabase.Controls
{
    partial class Board
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Board));
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.bReset = new System.Windows.Forms.ToolStripButton();
            this.bBack = new System.Windows.Forms.ToolStripButton();
            this.bNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.txtMove = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bRotateView = new System.Windows.Forms.ToolStripButton();
            this.bInitialPos = new System.Windows.Forms.ToolStripButton();
            this.bMarkMoves = new System.Windows.Forms.ToolStripButton();
            this.pbBoard = new System.Windows.Forms.PictureBox();
            this.bAddComments = new System.Windows.Forms.ToolStripButton();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBoard)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(780, 38);
            this.panel1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.Board_toolStrip1_Description;
            this.toolStrip1.AccessibleName = global::AIChessDatabase.Properties.UIElements.Board_toolStrip1_Name;
            this.toolStrip1.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bReset,
            this.bBack,
            this.bNext,
            this.toolStripSeparator1,
            this.txtMove,
            this.toolStripSeparator2,
            this.bRotateView,
            this.bInitialPos,
            this.bMarkMoves,
            this.bAddComments});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.toolStrip1.Size = new System.Drawing.Size(780, 38);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // bReset
            // 
            this.bReset.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bReset_Description;
            this.bReset.AccessibleName = global::AIChessDatabase.Properties.UIElements.bReset_Name;
            this.bReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bReset.Image = ((System.Drawing.Image)(resources.GetObject("bReset.Image")));
            this.bReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bReset.Name = "bReset";
            this.bReset.Size = new System.Drawing.Size(34, 33);
            this.bReset.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_RESET;
            this.bReset.Click += new System.EventHandler(this.bReset_Click);
            // 
            // bBack
            // 
            this.bBack.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bBack_Description;
            this.bBack.AccessibleName = global::AIChessDatabase.Properties.UIElements.bBack_Name;
            this.bBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bBack.Enabled = false;
            this.bBack.Image = ((System.Drawing.Image)(resources.GetObject("bBack.Image")));
            this.bBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bBack.Name = "bBack";
            this.bBack.Size = new System.Drawing.Size(34, 33);
            this.bBack.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_BACK;
            this.bBack.Click += new System.EventHandler(this.bBack_Click);
            // 
            // bNext
            // 
            this.bNext.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bNext_Description;
            this.bNext.AccessibleName = global::AIChessDatabase.Properties.UIElements.bNext_Name;
            this.bNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bNext.Enabled = false;
            this.bNext.Image = ((System.Drawing.Image)(resources.GetObject("bNext.Image")));
            this.bNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bNext.Name = "bNext";
            this.bNext.Size = new System.Drawing.Size(34, 33);
            this.bNext.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_NEXT;
            this.bNext.Click += new System.EventHandler(this.bNext_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 38);
            // 
            // txtMove
            // 
            this.txtMove.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.txtMove_Description;
            this.txtMove.AccessibleName = global::AIChessDatabase.Properties.UIElements.txtMove_Name;
            this.txtMove.AccessibleRole = System.Windows.Forms.AccessibleRole.StaticText;
            this.txtMove.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.txtMove.Name = "txtMove";
            this.txtMove.Size = new System.Drawing.Size(0, 33);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 38);
            // 
            // bRotateView
            // 
            this.bRotateView.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bRotateView_Description;
            this.bRotateView.AccessibleName = global::AIChessDatabase.Properties.UIElements.bRotateView_Name;
            this.bRotateView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bRotateView.Image = ((System.Drawing.Image)(resources.GetObject("bRotateView.Image")));
            this.bRotateView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bRotateView.Name = "bRotateView";
            this.bRotateView.Size = new System.Drawing.Size(34, 33);
            this.bRotateView.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_ROTATEVIEW;
            this.bRotateView.Click += new System.EventHandler(this.bRotateView_Click);
            // 
            // bInitialPos
            // 
            this.bInitialPos.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bInitialPos_Description;
            this.bInitialPos.AccessibleName = global::AIChessDatabase.Properties.UIElements.bInitialPos_Name;
            this.bInitialPos.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bInitialPos.Image = ((System.Drawing.Image)(resources.GetObject("bInitialPos.Image")));
            this.bInitialPos.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bInitialPos.Name = "bInitialPos";
            this.bInitialPos.Size = new System.Drawing.Size(34, 33);
            this.bInitialPos.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_INITIALPOS;
            this.bInitialPos.Visible = false;
            this.bInitialPos.Click += new System.EventHandler(this.bInitialPos_Click);
            // 
            // bMarkMoves
            // 
            this.bMarkMoves.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bMarkMoves_Description;
            this.bMarkMoves.AccessibleName = global::AIChessDatabase.Properties.UIElements.bMarkMoves_Name;
            this.bMarkMoves.Checked = true;
            this.bMarkMoves.CheckOnClick = true;
            this.bMarkMoves.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bMarkMoves.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bMarkMoves.Image = ((System.Drawing.Image)(resources.GetObject("bMarkMoves.Image")));
            this.bMarkMoves.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bMarkMoves.Name = "bMarkMoves";
            this.bMarkMoves.Size = new System.Drawing.Size(34, 33);
            this.bMarkMoves.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_MARKMSQUARE;
            // 
            // pbBoard
            // 
            this.pbBoard.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.pbBoard_Description;
            this.pbBoard.AccessibleName = global::AIChessDatabase.Properties.UIElements.pbBoard_Name;
            this.pbBoard.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
            this.pbBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbBoard.Location = new System.Drawing.Point(0, 38);
            this.pbBoard.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbBoard.Name = "pbBoard";
            this.pbBoard.Size = new System.Drawing.Size(780, 800);
            this.pbBoard.TabIndex = 1;
            this.pbBoard.TabStop = false;
            // 
            // bAddComments
            // 
            this.bAddComments.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.bAddComments.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bAddComments_Description;
            this.bAddComments.AccessibleName = global::AIChessDatabase.Properties.UIElements.bAddComments_Name;
            this.bAddComments.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bAddComments.CheckOnClick = true;
            this.bAddComments.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_ADDCOMMENTS;
            this.bAddComments.Image = ((System.Drawing.Image)(resources.GetObject("bAddComments.Image")));
            this.bAddComments.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bAddComments.Name = "bAddComments";
            this.bAddComments.Size = new System.Drawing.Size(34, 33);
            this.bAddComments.Visible = false;
            // 
            // Board
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbBoard);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Board";
            this.Size = new System.Drawing.Size(780, 838);
            this.Load += new System.EventHandler(this.Board_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBoard)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.PictureBox pbBoard;
        private System.Windows.Forms.ToolStripButton bReset;
        private System.Windows.Forms.ToolStripButton bBack;
        private System.Windows.Forms.ToolStripButton bNext;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel txtMove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton bRotateView;
        private System.Windows.Forms.ToolStripButton bInitialPos;
        private System.Windows.Forms.ToolStripButton bMarkMoves;
        private System.Windows.Forms.ToolStripButton bAddComments;
    }
}
