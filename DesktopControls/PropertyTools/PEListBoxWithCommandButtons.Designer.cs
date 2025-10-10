namespace DesktopControls.PropertyTools
{
    partial class PEListBoxWithCommandButtons
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PEListBoxWithCommandButtons));
            this.panel1 = new System.Windows.Forms.Panel();
            this.peListBox = new DesktopControls.PropertyTools.PropertyEditorListBox();
            this.bAdd = new System.Windows.Forms.Button();
            this.bDelete = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bDelete);
            this.panel1.Controls.Add(this.bAdd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(518, 38);
            this.panel1.TabIndex = 0;
            // 
            // peListBox
            // 
            this.peListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.peListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.peListBox.FormattingEnabled = true;
            this.peListBox.ItemHeight = 16;
            this.peListBox.Location = new System.Drawing.Point(0, 38);
            this.peListBox.Name = "peListBox";
            this.peListBox.Selection = null;
            this.peListBox.Size = new System.Drawing.Size(518, 265);
            this.peListBox.TabIndex = 1;
            this.peListBox.WInSrv = null;
            // 
            // bAdd
            // 
            this.bAdd.Image = ((System.Drawing.Image)(resources.GetObject("bAdd.Image")));
            this.bAdd.Location = new System.Drawing.Point(3, 3);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new System.Drawing.Size(32, 32);
            this.bAdd.TabIndex = 0;
            this.bAdd.UseVisualStyleBackColor = true;
            this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // bDelete
            // 
            this.bDelete.Image = ((System.Drawing.Image)(resources.GetObject("bDelete.Image")));
            this.bDelete.Location = new System.Drawing.Point(41, 3);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(32, 32);
            this.bDelete.TabIndex = 1;
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // PEListBoxWithCommandButtons
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.peListBox);
            this.Controls.Add(this.panel1);
            this.Name = "PEListBoxWithCommandButtons";
            this.Size = new System.Drawing.Size(518, 303);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private PropertyEditorListBox peListBox;
        private System.Windows.Forms.Button bAdd;
        private System.Windows.Forms.Button bDelete;
    }
}
