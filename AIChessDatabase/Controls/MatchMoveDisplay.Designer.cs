using AIChessDatabase.Properties;

namespace AIChessDatabase.Controls
{
    partial class MatchMoveDisplay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MatchMoveDisplay));
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.bTopDown = new System.Windows.Forms.ToolStripButton();
            this.bLeftRight = new System.Windows.Forms.ToolStripButton();
            this.sepAddMove = new System.Windows.Forms.ToolStripSeparator();
            this.txtAddMove = new System.Windows.Forms.ToolStripTextBox();
            this.bAddMove = new System.Windows.Forms.ToolStripButton();
            this.bDelMove = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pbMatch = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMatch)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(766, 39);
            this.panel1.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.MatchMoveDisplay_toolStrip1_Description;
            this.toolStrip1.AccessibleName = global::AIChessDatabase.Properties.UIElements.MatchMoveDisplay_toolStrip1_Name;
            this.toolStrip1.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bTopDown,
            this.bLeftRight,
            this.sepAddMove,
            this.txtAddMove,
            this.bAddMove,
            this.bDelMove});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(766, 29);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // bTopDown
            // 
            this.bTopDown.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bTopDown_Description;
            this.bTopDown.AccessibleName = global::AIChessDatabase.Properties.UIElements.bTopDown_Name;
            this.bTopDown.Checked = true;
            this.bTopDown.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bTopDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bTopDown.Image = ((System.Drawing.Image)(resources.GetObject("bTopDown.Image")));
            this.bTopDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bTopDown.Name = "bTopDown";
            this.bTopDown.Size = new System.Drawing.Size(34, 24);
            this.bTopDown.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_READTOPDOWN;
            this.bTopDown.Click += new System.EventHandler(this.bTopDown_Click);
            // 
            // bLeftRight
            // 
            this.bLeftRight.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bLeftRight_Description;
            this.bLeftRight.AccessibleName = global::AIChessDatabase.Properties.UIElements.bLeftRight_Name;
            this.bLeftRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bLeftRight.Image = ((System.Drawing.Image)(resources.GetObject("bLeftRight.Image")));
            this.bLeftRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bLeftRight.Name = "bLeftRight";
            this.bLeftRight.Size = new System.Drawing.Size(34, 24);
            this.bLeftRight.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_READLEFTRIGHT;
            this.bLeftRight.Click += new System.EventHandler(this.bLeftRight_Click);
            // 
            // sepAddMove
            // 
            this.sepAddMove.Name = "sepAddMove";
            this.sepAddMove.Size = new System.Drawing.Size(6, 31);
            this.sepAddMove.Visible = false;
            // 
            // txtAddMove
            // 
            this.txtAddMove.AcceptsReturn = true;
            this.txtAddMove.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.txtAddMove_Description;
            this.txtAddMove.AccessibleName = global::AIChessDatabase.Properties.UIElements.txtAddMove_Name;
            this.txtAddMove.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtAddMove.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtAddMove.Name = "txtAddMove";
            this.txtAddMove.Size = new System.Drawing.Size(148, 31);
            this.txtAddMove.Visible = false;
            this.txtAddMove.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAddMove_KeyDown);
            this.txtAddMove.TextChanged += new System.EventHandler(this.txtAddMove_TextChanged);
            // 
            // bAddMove
            // 
            this.bAddMove.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bAddMove_Description;
            this.bAddMove.AccessibleName = global::AIChessDatabase.Properties.UIElements.bAddMove_Name;
            this.bAddMove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bAddMove.Enabled = false;
            this.bAddMove.Image = ((System.Drawing.Image)(resources.GetObject("bAddMove.Image")));
            this.bAddMove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bAddMove.Name = "bAddMove";
            this.bAddMove.Size = new System.Drawing.Size(34, 24);
            this.bAddMove.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_ADDMOVE;
            this.bAddMove.Visible = false;
            this.bAddMove.Click += new System.EventHandler(this.bAddMove_Click);
            // 
            // bDelMove
            // 
            this.bDelMove.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bDelMove_Description;
            this.bDelMove.AccessibleName = global::AIChessDatabase.Properties.UIElements.bDelMove_Name;
            this.bDelMove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bDelMove.Enabled = false;
            this.bDelMove.Image = ((System.Drawing.Image)(resources.GetObject("bDelMove.Image")));
            this.bDelMove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bDelMove.Name = "bDelMove";
            this.bDelMove.Size = new System.Drawing.Size(34, 24);
            this.bDelMove.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_DELMOVE;
            this.bDelMove.Visible = false;
            this.bDelMove.Click += new System.EventHandler(this.bDelMove_Click);
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.pbMatch);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 39);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(766, 309);
            this.panel2.TabIndex = 2;
            // 
            // pbMatch
            // 
            this.pbMatch.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.pbMatch_Description;
            this.pbMatch.AccessibleName = global::AIChessDatabase.Properties.UIElements.pbMatch_Name;
            this.pbMatch.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
            this.pbMatch.Location = new System.Drawing.Point(0, 0);
            this.pbMatch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbMatch.Name = "pbMatch";
            this.pbMatch.Size = new System.Drawing.Size(100, 50);
            this.pbMatch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbMatch.TabIndex = 0;
            this.pbMatch.TabStop = false;
            this.pbMatch.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbMatch_MouseClick);
            // 
            // MatchMoveDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MatchMoveDisplay";
            this.Size = new System.Drawing.Size(766, 348);
            this.SizeChanged += new System.EventHandler(this.MatchMoveDisplay_SizeChanged);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMatch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton bTopDown;
        private System.Windows.Forms.ToolStripButton bLeftRight;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pbMatch;
        private System.Windows.Forms.ToolStripSeparator sepAddMove;
        private System.Windows.Forms.ToolStripTextBox txtAddMove;
        private System.Windows.Forms.ToolStripButton bAddMove;
        private System.Windows.Forms.ToolStripButton bDelMove;
    }
}
