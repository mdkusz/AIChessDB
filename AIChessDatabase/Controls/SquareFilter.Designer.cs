namespace AIChessDatabase.Controls
{
    partial class SquareFilter
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lFilter = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lSquare = new System.Windows.Forms.Label();
            this.cbOperators = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.pbBoard = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBoard)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.lFilter);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(377, 32);
            this.panel1.TabIndex = 0;
            // 
            // lFilter
            // 
            this.lFilter.AutoSize = true;
            this.lFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lFilter.Location = new System.Drawing.Point(10, 6);
            this.lFilter.Name = "lFilter";
            this.lFilter.Size = new System.Drawing.Size(57, 20);
            this.lFilter.TabIndex = 0;
            this.lFilter.Text = "label1";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Honeydew;
            this.panel2.Controls.Add(this.lSquare);
            this.panel2.Controls.Add(this.cbOperators);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 32);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(377, 44);
            this.panel2.TabIndex = 1;
            // 
            // lSquare
            // 
            this.lSquare.AutoSize = true;
            this.lSquare.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lSquare.Location = new System.Drawing.Point(11, 15);
            this.lSquare.Name = "lSquare";
            this.lSquare.Size = new System.Drawing.Size(57, 20);
            this.lSquare.TabIndex = 1;
            this.lSquare.Text = "label1";
            // 
            // cbOperators
            // 
            this.cbOperators.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbOperators_Description;
            this.cbOperators.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbOperators_Name;
            this.cbOperators.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.cbOperators.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOperators.FormattingEnabled = true;
            this.cbOperators.Location = new System.Drawing.Point(89, 8);
            this.cbOperators.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbOperators.Name = "cbOperators";
            this.cbOperators.Size = new System.Drawing.Size(136, 28);
            this.cbOperators.TabIndex = 0;
            this.cbOperators.SelectedIndexChanged += new System.EventHandler(this.cbOperators_SelectedIndexChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.bCancel);
            this.panel3.Controls.Add(this.bOK);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 336);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(377, 44);
            this.panel3.TabIndex = 3;
            // 
            // bCancel
            // 
            this.bCancel.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bCancel_Description;
            this.bCancel.AccessibleName = global::AIChessDatabase.Properties.UIElements.bCancel_Name;
            this.bCancel.Location = new System.Drawing.Point(280, 8);
            this.bCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(84, 29);
            this.bCancel.TabIndex = 1;
            this.bCancel.Text = "button2";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bOK
            // 
            this.bOK.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bOK_Description;
            this.bOK.AccessibleName = global::AIChessDatabase.Properties.UIElements.bOK_Name;
            this.bOK.Enabled = false;
            this.bOK.Location = new System.Drawing.Point(177, 8);
            this.bOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(84, 29);
            this.bOK.TabIndex = 0;
            this.bOK.Text = "button1";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // pbBoard
            // 
            this.pbBoard.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.pbBoard_Description;
            this.pbBoard.AccessibleName = global::AIChessDatabase.Properties.UIElements.pbBoard_Name;
            this.pbBoard.AccessibleRole = System.Windows.Forms.AccessibleRole.Graphic;
            this.pbBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbBoard.Location = new System.Drawing.Point(0, 76);
            this.pbBoard.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbBoard.Name = "pbBoard";
            this.pbBoard.Size = new System.Drawing.Size(377, 260);
            this.pbBoard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbBoard.TabIndex = 4;
            this.pbBoard.TabStop = false;
            this.pbBoard.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbBoard_MouseClick);
            // 
            // SquareFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbBoard);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(377, 380);
            this.Name = "SquareFilter";
            this.Size = new System.Drawing.Size(377, 380);
            this.Load += new System.EventHandler(this.SquareSelectionBoard_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbBoard)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cbOperators;
        private System.Windows.Forms.Label lSquare;
        private System.Windows.Forms.Label lFilter;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pbBoard;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
    }
}
