namespace AIChessDatabase.Controls
{
    partial class ConfigurableBoard
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
            this.components = new System.ComponentModel.Container();
            this.pTools = new System.Windows.Forms.Panel();
            this.pbBoard = new System.Windows.Forms.PictureBox();
            this.squareMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbBoard)).BeginInit();
            this.SuspendLayout();
            // 
            // pTools
            // 
            this.pTools.BackColor = System.Drawing.Color.SaddleBrown;
            this.pTools.BackgroundImage = global::AIChessDatabase.Properties.Resources.ChessPiecesArray;
            this.pTools.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pTools.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pTools.Location = new System.Drawing.Point(0, 520);
            this.pTools.Name = "pTools";
            this.pTools.Size = new System.Drawing.Size(520, 180);
            this.pTools.TabIndex = 0;
            this.pTools.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pTools_MouseDown);
            this.pTools.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pTools_MouseMove);
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
            this.pbBoard.TabIndex = 1;
            this.pbBoard.TabStop = false;
            this.pbBoard.DoubleClick += new System.EventHandler(this.pbBoard_DoubleClick);
            this.pbBoard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbBoard_MouseDown);
            this.pbBoard.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbBoard_MouseMove);
            this.pbBoard.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbBoard_MouseUp);
            // 
            // squareMenu
            // 
            this.squareMenu.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuPopup;
            this.squareMenu.AccessibleName = global::AIChessDatabase.Properties.UIElements.squareMenu_Name;
            this.squareMenu.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.squareMenu_Description;
            this.squareMenu.Name = "squareMenu";
            this.squareMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // ConfigurableBoard
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbBoard);
            this.Controls.Add(this.pTools);
            this.Name = "ConfigurableBoard";
            this.Size = new System.Drawing.Size(520, 700);
            ((System.ComponentModel.ISupportInitialize)(this.pbBoard)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pTools;
        private System.Windows.Forms.PictureBox pbBoard;
        private System.Windows.Forms.ContextMenuStrip squareMenu;
    }
}
