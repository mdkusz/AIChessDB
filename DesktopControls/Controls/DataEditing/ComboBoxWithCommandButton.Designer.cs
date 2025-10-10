namespace DesktopControls.Controls.DataEditing
{
    partial class ComboBoxWithCommandButton
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComboBoxWithCommandButton));
            this.bCommand = new System.Windows.Forms.Button();
            this.cbValues = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // bCommand
            // 
            this.bCommand.Image = ((System.Drawing.Image)(resources.GetObject("bCommand.Image")));
            this.bCommand.Location = new System.Drawing.Point(1, 0);
            this.bCommand.Margin = new System.Windows.Forms.Padding(0);
            this.bCommand.Name = "bCommand";
            this.bCommand.Size = new System.Drawing.Size(32, 30);
            this.bCommand.TabIndex = 0;
            this.bCommand.UseVisualStyleBackColor = true;
            this.bCommand.Click += new System.EventHandler(this.bCommand_Click);
            // 
            // cbValues
            // 
            this.cbValues.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbValues.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbValues.FormattingEnabled = true;
            this.cbValues.Location = new System.Drawing.Point(40, 3);
            this.cbValues.Name = "cbValues";
            this.cbValues.Size = new System.Drawing.Size(355, 24);
            this.cbValues.TabIndex = 1;
            // 
            // ComboBoxWithCommandButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbValues);
            this.Controls.Add(this.bCommand);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ComboBoxWithCommandButton";
            this.Size = new System.Drawing.Size(398, 30);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bCommand;
        private System.Windows.Forms.ComboBox cbValues;
    }
}
