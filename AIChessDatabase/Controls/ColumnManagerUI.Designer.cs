namespace AIChessDatabase.Controls
{
    partial class ColumnManagerUI
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lvColumns = new System.Windows.Forms.ListView();
            this.colQueryColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colQueryColumn.Text = global::AIChessDatabase.Properties.UIElements.DetailFiltersHeader;
            this.ctxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(567, 26);
            this.panel1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(567, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // lvColumns
            // 
            this.lvColumns.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.ColumnManagerUI_lvColumns_Description;
            this.lvColumns.AccessibleName = global::AIChessDatabase.Properties.UIElements.ColumnManagerUI_lvColumns_Name;
            this.lvColumns.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
            this.lvColumns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colQueryColumn});
            this.lvColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvColumns.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvColumns.HideSelection = false;
            this.lvColumns.Location = new System.Drawing.Point(0, 26);
            this.lvColumns.MultiSelect = false;
            this.lvColumns.Name = "lvColumns";
            this.lvColumns.Size = new System.Drawing.Size(567, 659);
            this.lvColumns.TabIndex = 1;
            this.lvColumns.UseCompatibleStateImageBehavior = false;
            this.lvColumns.View = System.Windows.Forms.View.Details;
            this.lvColumns.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvColumns_MouseClick);
            // 
            // colQueryColumn
            // 
            this.colQueryColumn.Width = 400;
            // 
            // ctxMenu
            // 
            this.ctxMenu.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.ColumnManagerUI_ctxMenu_Description;
            this.ctxMenu.AccessibleName = global::AIChessDatabase.Properties.UIElements.ColumnManagerUI_ctxMenu_Name;
            this.ctxMenu.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuPopup;
            this.ctxMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxMenu.Name = "ctxMenu";
            this.ctxMenu.Size = new System.Drawing.Size(61, 4);
            this.ctxMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenu_Opening);
            // 
            // ColumnManagerUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvColumns);
            this.Controls.Add(this.panel1);
            this.Name = "ColumnManagerUI";
            this.Size = new System.Drawing.Size(567, 685);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ListView lvColumns;
        private System.Windows.Forms.ContextMenuStrip ctxMenu;
        private System.Windows.Forms.ColumnHeader colQueryColumn;
    }
}
