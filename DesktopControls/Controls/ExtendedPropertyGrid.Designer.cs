namespace DesktopControls.Controls
{
    partial class ExtendedPropertyGrid
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
            this.pExtendedDoc = new System.Windows.Forms.Panel();
            this.lComment = new System.Windows.Forms.Label();
            this.lTitle = new System.Windows.Forms.Label();
            this.pgProperties = new DesktopControls.Controls.TabPropertyGrid();
            this.pExtendedDoc.SuspendLayout();
            this.SuspendLayout();
            // 
            // pExtendedDoc
            // 
            this.pExtendedDoc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pExtendedDoc.Controls.Add(this.lComment);
            this.pExtendedDoc.Controls.Add(this.lTitle);
            this.pExtendedDoc.Location = new System.Drawing.Point(43, 314);
            this.pExtendedDoc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pExtendedDoc.Name = "pExtendedDoc";
            this.pExtendedDoc.Size = new System.Drawing.Size(392, 107);
            this.pExtendedDoc.TabIndex = 1;
            this.pExtendedDoc.Visible = false;
            // 
            // lComment
            // 
            this.lComment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lComment.Location = new System.Drawing.Point(9, 25);
            this.lComment.Name = "lComment";
            this.lComment.Size = new System.Drawing.Size(372, 71);
            this.lComment.TabIndex = 1;
            this.lComment.Text = "label1";
            // 
            // lTitle
            // 
            this.lTitle.AutoSize = true;
            this.lTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTitle.Location = new System.Drawing.Point(7, 6);
            this.lTitle.Name = "lTitle";
            this.lTitle.Size = new System.Drawing.Size(52, 17);
            this.lTitle.TabIndex = 0;
            this.lTitle.Text = "label1";
            // 
            // pgProperties
            // 
            this.pgProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgProperties.EditorDropDownKey = System.Windows.Forms.Keys.F1;
            this.pgProperties.Location = new System.Drawing.Point(0, 0);
            this.pgProperties.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pgProperties.Name = "pgProperties";
            this.pgProperties.RefreshKey = System.Windows.Forms.Keys.Return;
            this.pgProperties.Size = new System.Drawing.Size(490, 442);
            this.pgProperties.TabIndex = 0;
            this.pgProperties.CommentSizeChanged += new System.EventHandler(this.pgProperties_CommentSizeChanged);
            this.pgProperties.EditorSizeChanged += new DesktopControls.PropertyTools.PropertyEditorChangedEventHandler(this.pgProperties_EditorSizeChanged);
            this.pgProperties.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgProperties_PropertyValueChanged);
            this.pgProperties.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.pgProperties_SelectedGridItemChanged);
            this.pgProperties.SelectedObjectsChanged += new System.EventHandler(this.pgProperties_SelectedObjectsChanged);
            this.pgProperties.SizeChanged += new System.EventHandler(this.pgProperties_SizeChanged);
            this.pgProperties.Layout += new System.Windows.Forms.LayoutEventHandler(this.pgProperties_Layout);
            // 
            // ExtendedPropertyGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pExtendedDoc);
            this.Controls.Add(this.pgProperties);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ExtendedPropertyGrid";
            this.Size = new System.Drawing.Size(490, 442);
            this.pExtendedDoc.ResumeLayout(false);
            this.pExtendedDoc.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TabPropertyGrid pgProperties;
        private System.Windows.Forms.Panel pExtendedDoc;
        private System.Windows.Forms.Label lComment;
        private System.Windows.Forms.Label lTitle;
    }
}
