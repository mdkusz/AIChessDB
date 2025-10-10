namespace AIChessDatabase.Controls
{
    partial class TinyBoard
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
            this.lMove = new System.Windows.Forms.Label();
            this.pbBoard = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbBoard)).BeginInit();
            this.SuspendLayout();
            // 
            // lMove
            // 
            this.lMove.AutoSize = true;
            this.lMove.Dock = System.Windows.Forms.DockStyle.Top;
            this.lMove.Location = new System.Drawing.Point(0, 0);
            this.lMove.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lMove.Name = "lMove";
            this.lMove.Size = new System.Drawing.Size(0, 16);
            this.lMove.TabIndex = 0;
            // 
            // pbBoard
            // 
            this.pbBoard.AccessibleRole = System.Windows.Forms.AccessibleRole.Graphic;
            this.pbBoard.AccessibleName = global::AIChessDatabase.Properties.UIElements.pbBoard_Name;
            this.pbBoard.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.pbBoard_Description;
            this.pbBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbBoard.Location = new System.Drawing.Point(0, 16);
            this.pbBoard.Margin = new System.Windows.Forms.Padding(4);
            this.pbBoard.Name = "pbBoard";
            this.pbBoard.Size = new System.Drawing.Size(221, 204);
            this.pbBoard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbBoard.TabIndex = 1;
            this.pbBoard.TabStop = false;
            // 
            // TinyBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbBoard);
            this.Controls.Add(this.lMove);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TinyBoard";
            this.Size = new System.Drawing.Size(221, 220);
            ((System.ComponentModel.ISupportInitialize)(this.pbBoard)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lMove;
        private System.Windows.Forms.PictureBox pbBoard;
    }
}
