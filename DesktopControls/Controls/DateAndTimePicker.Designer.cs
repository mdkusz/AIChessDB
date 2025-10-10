namespace DesktopControls.Controls
{
    partial class DateAndTimePicker
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
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.dtpTime = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // dtpDate
            // 
            this.dtpDate.Dock = System.Windows.Forms.DockStyle.Left;
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(0, 0);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.ShowCheckBox = true;
            this.dtpDate.Size = new System.Drawing.Size(120, 22);
            this.dtpDate.TabIndex = 0;
            this.dtpDate.FormatChanged += new System.EventHandler(this.dtpDate_FormatChanged);
            this.dtpDate.ValueChanged += new System.EventHandler(this.dtpDate_ValueChanged);
            this.dtpDate.FontChanged += new System.EventHandler(this.dtpDate_FontChanged);
            this.dtpDate.SizeChanged += new System.EventHandler(this.dtpDate_SizeChanged);
            // 
            // dtpTime
            // 
            this.dtpTime.Dock = System.Windows.Forms.DockStyle.Left;
            this.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpTime.Location = new System.Drawing.Point(120, 0);
            this.dtpTime.Name = "dtpTime";
            this.dtpTime.ShowCheckBox = true;
            this.dtpTime.ShowUpDown = true;
            this.dtpTime.Size = new System.Drawing.Size(110, 22);
            this.dtpTime.TabIndex = 1;
            this.dtpTime.ValueChanged += new System.EventHandler(this.dtpTime_ValueChanged);
            this.dtpTime.FontChanged += new System.EventHandler(this.dtpTime_FontChanged);
            this.dtpTime.SizeChanged += new System.EventHandler(this.dtpTime_SizeChanged);
            // 
            // DateAndTimePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dtpTime);
            this.Controls.Add(this.dtpDate);
            this.Name = "DateAndTimePicker";
            this.Size = new System.Drawing.Size(230, 22);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.DateTimePicker dtpTime;
    }
}
