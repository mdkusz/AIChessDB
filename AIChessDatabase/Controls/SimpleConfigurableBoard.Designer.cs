namespace AIChessDatabase.Controls
{
    partial class SimpleConfigurableBoard
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
            this.pbBoard = new System.Windows.Forms.PictureBox();
            this.pTools = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbBoard)).BeginInit();
            this.SuspendLayout();
            // 
            // pbBoard
            // 
            this.pbBoard.AccessibleRole = System.Windows.Forms.AccessibleRole.Graphic;
            this.pbBoard.AccessibleName = global::AIChessDatabase.Properties.UIElements.pbBoard_Name;
            this.pbBoard.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.pbBoard_Description;
            this.pbBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbBoard.Location = new System.Drawing.Point(0, 0);
            this.pbBoard.Name = "pbBoard";
            this.pbBoard.Size = new System.Drawing.Size(520, 520);
            this.pbBoard.TabIndex = 3;
            this.pbBoard.TabStop = false;
            this.pbBoard.DoubleClick += new System.EventHandler(this.pbBoard_DoubleClick);
            this.pbBoard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbBoard_MouseDown);
            this.pbBoard.MouseLeave += new System.EventHandler(this.pbBoard_MouseLeave);
            // 
            // pTools
            // 
            this.pTools.BackColor = System.Drawing.Color.SaddleBrown;
            this.pTools.BackgroundImage = global::AIChessDatabase.Properties.Resources.ChessPiecesArray;
            this.pTools.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pTools.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pTools.Location = new System.Drawing.Point(0, 520);
            this.pTools.Name = "pTools";
            this.pTools.Size = new System.Drawing.Size(520, 120);
            this.pTools.TabIndex = 2;
            this.pTools.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pTools_MouseDown);
            this.pTools.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pTools_MouseMove);
            // 
            // SimpleConfigurableBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbBoard);
            this.Controls.Add(this.pTools);
            this.Name = "SimpleConfigurableBoard";
            this.Size = new System.Drawing.Size(520, 640);
            ((System.ComponentModel.ISupportInitialize)(this.pbBoard)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbBoard;
        private System.Windows.Forms.Panel pTools;
    }
}
