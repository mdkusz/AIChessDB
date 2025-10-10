namespace AIChessDatabase.Dialogs
{
    partial class DlgLabelMatch
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgLabelMatch));
            this.txtNewKey = new System.Windows.Forms.TextBox();
            this.bNewKey = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbKeywords = new System.Windows.Forms.ComboBox();
            this.txtKeyValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bOK = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtNewKey
            // 
            this.txtNewKey.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtNewKey.AccessibleName = global::AIChessDatabase.Properties.UIElements.txtNewKey_Name;
            this.txtNewKey.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.txtNewKey_Description;
            this.txtNewKey.Location = new System.Drawing.Point(177, 15);
            this.txtNewKey.Margin = new System.Windows.Forms.Padding(4);
            this.txtNewKey.Name = "txtNewKey";
            this.txtNewKey.Size = new System.Drawing.Size(173, 22);
            this.txtNewKey.TabIndex = 14;
            this.txtNewKey.TextChanged += new System.EventHandler(this.txtNewKey_TextChanged);
            // 
            // bNewKey
            // 
            this.bNewKey.AccessibleRole = System.Windows.Forms.AccessibleRole.Default;
            this.bNewKey.AccessibleName = global::AIChessDatabase.Properties.UIElements.bNewKey_Name;
            this.bNewKey.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bNewKey_Description;
            this.bNewKey.Enabled = false;
            this.bNewKey.Location = new System.Drawing.Point(109, 12);
            this.bNewKey.Margin = new System.Windows.Forms.Padding(4);
            this.bNewKey.Name = "bNewKey";
            this.bNewKey.Size = new System.Drawing.Size(60, 28);
            this.bNewKey.TabIndex = 13;
            this.bNewKey.Text = global::AIChessDatabase.Properties.UIResources.BTN_ADD;
            this.bNewKey.UseVisualStyleBackColor = true;
            this.bNewKey.Click += new System.EventHandler(this.bNewKey_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 100);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "Value";
            // 
            // cbKeywords
            // 
            this.cbKeywords.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.cbKeywords.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbKeywords_Name;
            this.cbKeywords.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbKeywords_Description;
            this.cbKeywords.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKeywords.FormattingEnabled = true;
            this.cbKeywords.Location = new System.Drawing.Point(16, 55);
            this.cbKeywords.Margin = new System.Windows.Forms.Padding(4);
            this.cbKeywords.Name = "cbKeywords";
            this.cbKeywords.Size = new System.Drawing.Size(335, 24);
            this.cbKeywords.Sorted = true;
            this.cbKeywords.TabIndex = 11;
            this.cbKeywords.SelectedIndexChanged += new System.EventHandler(this.cbKeywords_SelectedIndexChanged);
            // 
            // txtKeyValue
            // 
            this.txtKeyValue.Location = new System.Drawing.Point(16, 119);
            this.txtKeyValue.Margin = new System.Windows.Forms.Padding(4);
            this.txtKeyValue.Name = "txtKeyValue";
            this.txtKeyValue.Size = new System.Drawing.Size(335, 22);
            this.txtKeyValue.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 36);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "Keyword";
            // 
            // bOK
            // 
            this.bOK.AccessibleRole = System.Windows.Forms.AccessibleRole.Default;
            this.bOK.AccessibleName = global::AIChessDatabase.Properties.UIElements.bOK_Name;
            this.bOK.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bOK_Description;
            this.bOK.Enabled = false;
            this.bOK.Location = new System.Drawing.Point(123, 176);
            this.bOK.Margin = new System.Windows.Forms.Padding(4);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(100, 28);
            this.bOK.TabIndex = 16;
            this.bOK.Text = global::AIChessDatabase.Properties.UIResources.BTN_OK;
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // bCancel
            // 
            this.bCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.Default;
            this.bCancel.AccessibleName = global::AIChessDatabase.Properties.UIElements.bCancel_Name;
            this.bCancel.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bCancel_Description;
            this.bCancel.Location = new System.Drawing.Point(249, 176);
            this.bCancel.Margin = new System.Windows.Forms.Padding(4);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(100, 28);
            this.bCancel.TabIndex = 15;
            this.bCancel.Text = global::AIChessDatabase.Properties.UIResources.BTN_CANCEL;
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // DlgLabelMatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 217);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.txtNewKey);
            this.Controls.Add(this.bNewKey);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbKeywords);
            this.Controls.Add(this.txtKeyValue);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgLabelMatch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DlgLabelMatch";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DlgLabelMatch_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNewKey;
        private System.Windows.Forms.Button bNewKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbKeywords;
        private System.Windows.Forms.TextBox txtKeyValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Button bCancel;
    }
}