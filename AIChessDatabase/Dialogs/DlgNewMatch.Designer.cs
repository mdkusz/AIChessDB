namespace AIChessDatabase.Dialogs
{
    partial class DlgNewMatch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgNewMatch));
            this.txtBlack = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtWhite = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMatch = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.cbResult = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txtBlack
            // 
            this.txtBlack.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.txtBlack_Description;
            this.txtBlack.AccessibleName = global::AIChessDatabase.Properties.UIElements.txtBlack_Name;
            this.txtBlack.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtBlack.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtBlack.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtBlack.Location = new System.Drawing.Point(16, 146);
            this.txtBlack.Margin = new System.Windows.Forms.Padding(4);
            this.txtBlack.Name = "txtBlack";
            this.txtBlack.Size = new System.Drawing.Size(340, 22);
            this.txtBlack.TabIndex = 28;
            this.txtBlack.TextChanged += new System.EventHandler(this.txtWhite_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 127);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 16);
            this.label7.TabIndex = 27;
            this.label7.Text = "Black";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 245);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 16);
            this.label6.TabIndex = 25;
            this.label6.Text = "Result";
            // 
            // txtDate
            // 
            this.txtDate.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.txtDate_Description;
            this.txtDate.AccessibleName = global::AIChessDatabase.Properties.UIElements.txtDate_Name;
            this.txtDate.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtDate.Location = new System.Drawing.Point(16, 207);
            this.txtDate.Margin = new System.Windows.Forms.Padding(4);
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(132, 22);
            this.txtDate.TabIndex = 24;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 187);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 16);
            this.label5.TabIndex = 23;
            this.label5.Text = "Date";
            // 
            // txtWhite
            // 
            this.txtWhite.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.txtWhite_Description;
            this.txtWhite.AccessibleName = global::AIChessDatabase.Properties.UIElements.txtWhite_Name;
            this.txtWhite.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtWhite.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtWhite.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtWhite.Location = new System.Drawing.Point(16, 87);
            this.txtWhite.Margin = new System.Windows.Forms.Padding(4);
            this.txtWhite.Name = "txtWhite";
            this.txtWhite.Size = new System.Drawing.Size(340, 22);
            this.txtWhite.TabIndex = 22;
            this.txtWhite.TextChanged += new System.EventHandler(this.txtWhite_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 68);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 16);
            this.label4.TabIndex = 21;
            this.label4.Text = "White";
            // 
            // txtMatch
            // 
            this.txtMatch.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.txtMatch_Description;
            this.txtMatch.AccessibleName = global::AIChessDatabase.Properties.UIElements.txtMatch_Name;
            this.txtMatch.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtMatch.Location = new System.Drawing.Point(16, 30);
            this.txtMatch.Margin = new System.Windows.Forms.Padding(4);
            this.txtMatch.Name = "txtMatch";
            this.txtMatch.Size = new System.Drawing.Size(340, 22);
            this.txtMatch.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 10);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 16);
            this.label3.TabIndex = 19;
            this.label3.Text = "Description";
            // 
            // bCancel
            // 
            this.bCancel.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bCancel_Description;
            this.bCancel.AccessibleName = global::AIChessDatabase.Properties.UIElements.bCancel_Name;
            this.bCancel.Location = new System.Drawing.Point(257, 309);
            this.bCancel.Margin = new System.Windows.Forms.Padding(4);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(100, 28);
            this.bCancel.TabIndex = 29;
            this.bCancel.Text = global::AIChessDatabase.Properties.UIResources.BTN_CANCEL;
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bOK
            // 
            this.bOK.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bOK_Description;
            this.bOK.AccessibleName = global::AIChessDatabase.Properties.UIElements.bOK_Name;
            this.bOK.Enabled = false;
            this.bOK.Location = new System.Drawing.Point(135, 309);
            this.bOK.Margin = new System.Windows.Forms.Padding(4);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(100, 28);
            this.bOK.TabIndex = 30;
            this.bOK.Text = global::AIChessDatabase.Properties.UIResources.BTN_OK;
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // cbResult
            // 
            this.cbResult.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbResult_Description;
            this.cbResult.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbResult_Name;
            this.cbResult.AccessibleRole = System.Windows.Forms.AccessibleRole.DropList;
            this.cbResult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbResult.FormattingEnabled = true;
            this.cbResult.Items.AddRange(new object[] {
            "*",
            "1-0",
            "0-1",
            "1/2-1/2"});
            this.cbResult.Location = new System.Drawing.Point(16, 265);
            this.cbResult.Margin = new System.Windows.Forms.Padding(4);
            this.cbResult.Name = "cbResult";
            this.cbResult.Size = new System.Drawing.Size(132, 24);
            this.cbResult.TabIndex = 31;
            this.cbResult.SelectedIndexChanged += new System.EventHandler(this.txtWhite_TextChanged);
            // 
            // DlgNewMatch
            // 
            this.AcceptButton = this.bOK;
            this.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.DlgNewMatch_Description;
            this.AccessibleName = global::AIChessDatabase.Properties.UIElements.DlgNewMatch_Name;
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Dialog;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 352);
            this.Controls.Add(this.cbResult);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.txtBlack);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtDate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtWhite);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtMatch);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgNewMatch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Match Data";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DlgNewMatch_FormClosed);
            this.Shown += new System.EventHandler(this.DlgNewMatch_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBlack;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtWhite;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMatch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.ComboBox cbResult;
    }
}