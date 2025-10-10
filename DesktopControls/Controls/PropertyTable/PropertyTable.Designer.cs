namespace DesktopControls.Controls.PropertyTable
{
    partial class PropertyTable
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
            this.spContainter = new System.Windows.Forms.SplitContainer();
            this.pGrid = new System.Windows.Forms.FlowLayoutPanel();
            this.pExtendedDoc = new System.Windows.Forms.Panel();
            this.gbTitle = new System.Windows.Forms.GroupBox();
            this.lComment = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.spContainter)).BeginInit();
            this.spContainter.Panel1.SuspendLayout();
            this.spContainter.Panel2.SuspendLayout();
            this.spContainter.SuspendLayout();
            this.pExtendedDoc.SuspendLayout();
            this.gbTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // spContainter
            // 
            this.spContainter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spContainter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.spContainter.Location = new System.Drawing.Point(0, 0);
            this.spContainter.Name = "spContainter";
            this.spContainter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spContainter.Panel1
            // 
            this.spContainter.Panel1.Controls.Add(this.pGrid);
            // 
            // spContainter.Panel2
            // 
            this.spContainter.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.spContainter.Panel2.Controls.Add(this.pExtendedDoc);
            this.spContainter.Size = new System.Drawing.Size(1503, 400);
            this.spContainter.SplitterDistance = 280;
            this.spContainter.TabIndex = 0;
            // 
            // pGrid
            // 
            this.pGrid.AutoScroll = true;
            this.pGrid.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pGrid.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pGrid.Location = new System.Drawing.Point(0, 0);
            this.pGrid.Margin = new System.Windows.Forms.Padding(0);
            this.pGrid.Name = "pGrid";
            this.pGrid.Size = new System.Drawing.Size(1503, 280);
            this.pGrid.TabIndex = 0;
            this.pGrid.WrapContents = false;
            // 
            // pExtendedDoc
            // 
            this.pExtendedDoc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pExtendedDoc.Controls.Add(this.gbTitle);
            this.pExtendedDoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pExtendedDoc.Location = new System.Drawing.Point(0, 0);
            this.pExtendedDoc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pExtendedDoc.Name = "pExtendedDoc";
            this.pExtendedDoc.Size = new System.Drawing.Size(1503, 116);
            this.pExtendedDoc.TabIndex = 2;
            // 
            // gbTitle
            // 
            this.gbTitle.Controls.Add(this.lComment);
            this.gbTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbTitle.Location = new System.Drawing.Point(0, 0);
            this.gbTitle.Name = "gbTitle";
            this.gbTitle.Size = new System.Drawing.Size(1501, 114);
            this.gbTitle.TabIndex = 0;
            this.gbTitle.TabStop = false;
            // 
            // lComment
            // 
            this.lComment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lComment.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lComment.Location = new System.Drawing.Point(3, 18);
            this.lComment.Name = "lComment";
            this.lComment.Size = new System.Drawing.Size(1495, 93);
            this.lComment.TabIndex = 2;
            // 
            // PropertyTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.spContainter);
            this.Name = "PropertyTable";
            this.Size = new System.Drawing.Size(1503, 400);
            this.Load += new System.EventHandler(this.PropertyTable_Load);
            this.spContainter.Panel1.ResumeLayout(false);
            this.spContainter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spContainter)).EndInit();
            this.spContainter.ResumeLayout(false);
            this.pExtendedDoc.ResumeLayout(false);
            this.gbTitle.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer spContainter;
        private System.Windows.Forms.Panel pExtendedDoc;
        private System.Windows.Forms.GroupBox gbTitle;
        private System.Windows.Forms.Label lComment;
        private System.Windows.Forms.FlowLayoutPanel pGrid;
    }
}
