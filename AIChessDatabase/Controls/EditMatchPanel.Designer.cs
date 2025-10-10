namespace AIChessDatabase.Controls
{
    partial class EditMatchPanel
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
            this.bAddCom = new System.Windows.Forms.Button();
            this.bCancelChg = new System.Windows.Forms.Button();
            this.bUpdate = new System.Windows.Forms.Button();
            this.bDeleteCom = new System.Windows.Forms.Button();
            this.bChangeCom = new System.Windows.Forms.Button();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.lbComments = new System.Windows.Forms.ListBox();
            this.lComments = new System.Windows.Forms.Label();
            this.txtBlack = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtWhite = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMatch = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNewKey = new System.Windows.Forms.TextBox();
            this.bNewKey = new System.Windows.Forms.Button();
            this.bDelete = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.bChangeValue = new System.Windows.Forms.Button();
            this.cbKeywords = new System.Windows.Forms.ComboBox();
            this.txtKeyValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbResult = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbResult);
            this.panel1.Controls.Add(this.bAddCom);
            this.panel1.Controls.Add(this.bCancelChg);
            this.panel1.Controls.Add(this.bUpdate);
            this.panel1.Controls.Add(this.bDeleteCom);
            this.panel1.Controls.Add(this.bChangeCom);
            this.panel1.Controls.Add(this.txtComment);
            this.panel1.Controls.Add(this.lbComments);
            this.panel1.Controls.Add(this.lComments);
            this.panel1.Controls.Add(this.txtBlack);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtDate);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtWhite);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtMatch);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtNewKey);
            this.panel1.Controls.Add(this.bNewKey);
            this.panel1.Controls.Add(this.bDelete);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.bChangeValue);
            this.panel1.Controls.Add(this.cbKeywords);
            this.panel1.Controls.Add(this.txtKeyValue);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(724, 523);
            this.panel1.TabIndex = 1;
            // 
            // bAddCom
            // 
            this.bAddCom.AccessibleRole = System.Windows.Forms.AccessibleRole.Default;
            this.bAddCom.AccessibleName = global::AIChessDatabase.Properties.UIElements.bAddCom_Name;
            this.bAddCom.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bAddCom_Description;
            this.bAddCom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bAddCom.Enabled = false;
            this.bAddCom.Location = new System.Drawing.Point(389, 442);
            this.bAddCom.Name = "bAddCom";
            this.bAddCom.Size = new System.Drawing.Size(75, 23);
            this.bAddCom.TabIndex = 26;
            this.bAddCom.Text = global::AIChessDatabase.Properties.UIResources.BTN_ADD;
            this.bAddCom.UseVisualStyleBackColor = true;
            this.bAddCom.Click += new System.EventHandler(this.bAddCom_Click);
            // 
            // bCancelChg
            // 
            this.bCancelChg.AccessibleRole = System.Windows.Forms.AccessibleRole.Default;
            this.bCancelChg.AccessibleName = global::AIChessDatabase.Properties.UIElements.bCancelChg_Name;
            this.bCancelChg.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bCancelChg_Description;
            this.bCancelChg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bCancelChg.Enabled = false;
            this.bCancelChg.Location = new System.Drawing.Point(159, 488);
            this.bCancelChg.Name = "bCancelChg";
            this.bCancelChg.Size = new System.Drawing.Size(121, 23);
            this.bCancelChg.TabIndex = 25;
            this.bCancelChg.Text = global::AIChessDatabase.Properties.UIResources.BTN_CANCELCHG;
            this.bCancelChg.UseVisualStyleBackColor = true;
            this.bCancelChg.Click += new System.EventHandler(this.bCancelChg_Click);
            // 
            // bUpdate
            // 
            this.bUpdate.AccessibleRole = System.Windows.Forms.AccessibleRole.Default;
            this.bUpdate.AccessibleName = global::AIChessDatabase.Properties.UIElements.bUpdate_Name;
            this.bUpdate.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bUpdate_Description;
            this.bUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bUpdate.Enabled = false;
            this.bUpdate.Location = new System.Drawing.Point(26, 488);
            this.bUpdate.Name = "bUpdate";
            this.bUpdate.Size = new System.Drawing.Size(112, 23);
            this.bUpdate.TabIndex = 24;
            this.bUpdate.Text = global::AIChessDatabase.Properties.UIResources.BTN_UPDATE;
            this.bUpdate.UseVisualStyleBackColor = true;
            this.bUpdate.Click += new System.EventHandler(this.bUpdate_Click);
            // 
            // bDeleteCom
            // 
            this.bDeleteCom.AccessibleRole = System.Windows.Forms.AccessibleRole.Default;
            this.bDeleteCom.AccessibleName = global::AIChessDatabase.Properties.UIElements.bDeleteCom_Name;
            this.bDeleteCom.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bDeleteCom_Description;
            this.bDeleteCom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bDeleteCom.Enabled = false;
            this.bDeleteCom.Location = new System.Drawing.Point(552, 442);
            this.bDeleteCom.Name = "bDeleteCom";
            this.bDeleteCom.Size = new System.Drawing.Size(75, 23);
            this.bDeleteCom.TabIndex = 23;
            this.bDeleteCom.Text = global::AIChessDatabase.Properties.UIResources.BTN_DELETE;
            this.bDeleteCom.UseVisualStyleBackColor = true;
            this.bDeleteCom.Click += new System.EventHandler(this.bDeleteCom_Click);
            // 
            // bChangeCom
            // 
            this.bChangeCom.AccessibleRole = System.Windows.Forms.AccessibleRole.Default;
            this.bChangeCom.AccessibleName = global::AIChessDatabase.Properties.UIElements.bChangeCom_Name;
            this.bChangeCom.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bChangeCom_Description;
            this.bChangeCom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bChangeCom.Enabled = false;
            this.bChangeCom.Location = new System.Drawing.Point(470, 442);
            this.bChangeCom.Name = "bChangeCom";
            this.bChangeCom.Size = new System.Drawing.Size(75, 23);
            this.bChangeCom.TabIndex = 22;
            this.bChangeCom.Text = global::AIChessDatabase.Properties.UIResources.BTN_CHANGE;
            this.bChangeCom.UseVisualStyleBackColor = true;
            this.bChangeCom.Click += new System.EventHandler(this.bChangeCom_Click);
            // 
            // txtComment
            // 
            this.txtComment.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtComment.AccessibleName = global::AIChessDatabase.Properties.UIElements.txtComment_Name;
            this.txtComment.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.txtComment_Description;
            this.txtComment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtComment.Location = new System.Drawing.Point(26, 416);
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(601, 20);
            this.txtComment.TabIndex = 21;
            this.txtComment.TextChanged += new System.EventHandler(this.txtComment_TextChanged);
            // 
            // lbComments
            // 
            this.lbComments.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
            this.lbComments.AccessibleName = global::AIChessDatabase.Properties.UIElements.lbComments_Name;
            this.lbComments.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.lbComments_Description;
            this.lbComments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbComments.FormattingEnabled = true;
            this.lbComments.Location = new System.Drawing.Point(26, 239);
            this.lbComments.Name = "lbComments";
            this.lbComments.Size = new System.Drawing.Size(601, 160);
            this.lbComments.TabIndex = 20;
            this.lbComments.SelectedIndexChanged += new System.EventHandler(this.lbComments_SelectedIndexChanged);
            // 
            // lComments
            // 
            this.lComments.AutoSize = true;
            this.lComments.Location = new System.Drawing.Point(23, 223);
            this.lComments.Name = "lComments";
            this.lComments.Size = new System.Drawing.Size(56, 13);
            this.lComments.TabIndex = 19;
            this.lComments.Text = "Comments";
            // 
            // txtBlack
            // 
            this.txtBlack.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtBlack.AccessibleName = global::AIChessDatabase.Properties.UIElements.txtBlack_Name;
            this.txtBlack.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.txtBlack_Description;
            this.txtBlack.Location = new System.Drawing.Point(298, 170);
            this.txtBlack.Name = "txtBlack";
            this.txtBlack.Size = new System.Drawing.Size(247, 20);
            this.txtBlack.TabIndex = 18;
            this.txtBlack.TextChanged += new System.EventHandler(this.txtBlack_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(295, 154);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Black";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(135, 154);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Result";
            // 
            // txtDate
            // 
            this.txtDate.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtDate.AccessibleName = global::AIChessDatabase.Properties.UIElements.txtDate_Name;
            this.txtDate.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.txtDate_Description;
            this.txtDate.Location = new System.Drawing.Point(24, 170);
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(100, 20);
            this.txtDate.TabIndex = 14;
            this.txtDate.TextChanged += new System.EventHandler(this.txtDate_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Date";
            // 
            // txtWhite
            // 
            this.txtWhite.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtWhite.AccessibleName = global::AIChessDatabase.Properties.UIElements.txtWhite_Name;
            this.txtWhite.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.txtWhite_Description;
            this.txtWhite.Location = new System.Drawing.Point(298, 122);
            this.txtWhite.Name = "txtWhite";
            this.txtWhite.Size = new System.Drawing.Size(247, 20);
            this.txtWhite.TabIndex = 12;
            this.txtWhite.TextChanged += new System.EventHandler(this.txtWhite_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(295, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "White";
            // 
            // txtMatch
            // 
            this.txtMatch.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtMatch.AccessibleName = global::AIChessDatabase.Properties.UIElements.txtMatch_Name;
            this.txtMatch.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.txtMatch_Description;
            this.txtMatch.Location = new System.Drawing.Point(24, 122);
            this.txtMatch.Name = "txtMatch";
            this.txtMatch.Size = new System.Drawing.Size(256, 20);
            this.txtMatch.TabIndex = 10;
            this.txtMatch.TextChanged += new System.EventHandler(this.txtMatch_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Match";
            // 
            // txtNewKey
            // 
            this.txtNewKey.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtNewKey.AccessibleName = global::AIChessDatabase.Properties.UIElements.txtNewKey_Name;
            this.txtNewKey.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.txtNewKey_Description;
            this.txtNewKey.Location = new System.Drawing.Point(75, 65);
            this.txtNewKey.Name = "txtNewKey";
            this.txtNewKey.Size = new System.Drawing.Size(131, 20);
            this.txtNewKey.TabIndex = 8;
            this.txtNewKey.TextChanged += new System.EventHandler(this.txtNewKey_TextChanged);
            // 
            // bNewKey
            // 
            this.bNewKey.AccessibleRole = System.Windows.Forms.AccessibleRole.Default;
            this.bNewKey.AccessibleName = global::AIChessDatabase.Properties.UIElements.bNewKey_Name;
            this.bNewKey.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bMatchNewKey_Description;
            this.bNewKey.Enabled = false;
            this.bNewKey.Location = new System.Drawing.Point(24, 63);
            this.bNewKey.Name = "bNewKey";
            this.bNewKey.Size = new System.Drawing.Size(45, 23);
            this.bNewKey.TabIndex = 7;
            this.bNewKey.Text = global::AIChessDatabase.Properties.UIResources.BTN_ADD;
            this.bNewKey.UseVisualStyleBackColor = true;
            this.bNewKey.Click += new System.EventHandler(this.bNewKey_Click);
            // 
            // bDelete
            // 
            this.bDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.Default;
            this.bDelete.AccessibleName = global::AIChessDatabase.Properties.UIElements.bDelete_Name;
            this.bDelete.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bDelete_Description;
            this.bDelete.Enabled = false;
            this.bDelete.Location = new System.Drawing.Point(552, 34);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(75, 23);
            this.bDelete.TabIndex = 6;
            this.bDelete.Text = global::AIChessDatabase.Properties.UIResources.BTN_DELETE;
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(212, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Value";
            // 
            // bChangeValue
            // 
            this.bChangeValue.AccessibleRole = System.Windows.Forms.AccessibleRole.Default;
            this.bChangeValue.AccessibleName = global::AIChessDatabase.Properties.UIElements.bChangeValue_Name;
            this.bChangeValue.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bChangeValue_Description;
            this.bChangeValue.Enabled = false;
            this.bChangeValue.Location = new System.Drawing.Point(470, 34);
            this.bChangeValue.Name = "bChangeValue";
            this.bChangeValue.Size = new System.Drawing.Size(75, 23);
            this.bChangeValue.TabIndex = 4;
            this.bChangeValue.Text = global::AIChessDatabase.Properties.UIResources.BTN_CHANGE;
            this.bChangeValue.UseVisualStyleBackColor = true;
            this.bChangeValue.Click += new System.EventHandler(this.bChangeValue_Click);
            // 
            // cbKeywords
            // 
            this.cbKeywords.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.cbKeywords.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbKeywords_Name;
            this.cbKeywords.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbKeywords_Description;
            this.cbKeywords.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKeywords.FormattingEnabled = true;
            this.cbKeywords.Location = new System.Drawing.Point(24, 36);
            this.cbKeywords.Name = "cbKeywords";
            this.cbKeywords.Size = new System.Drawing.Size(182, 21);
            this.cbKeywords.Sorted = true;
            this.cbKeywords.TabIndex = 3;
            this.cbKeywords.SelectedIndexChanged += new System.EventHandler(this.cbKeywords_SelectedIndexChanged);
            // 
            // txtKeyValue
            // 
            this.txtKeyValue.AccessibleRole = System.Windows.Forms.AccessibleRole.Default;
            this.txtKeyValue.AccessibleName = global::AIChessDatabase.Properties.UIElements.txtKeyValue_Name;
            this.txtKeyValue.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.txtKeyValue_Description;
            this.txtKeyValue.Location = new System.Drawing.Point(212, 36);
            this.txtKeyValue.Name = "txtKeyValue";
            this.txtKeyValue.Size = new System.Drawing.Size(252, 20);
            this.txtKeyValue.TabIndex = 2;
            this.txtKeyValue.TextChanged += new System.EventHandler(this.txtKeyValue_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Keyword";
            // 
            // cbResult
            // 
            this.cbResult.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.cbResult.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbResult_Name;
            this.cbResult.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbResult_Description;
            this.cbResult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbResult.FormattingEnabled = true;
            this.cbResult.Items.AddRange(new object[] {
            "*",
            "1-0",
            "0-1",
            "1/2-1/2"});
            this.cbResult.Location = new System.Drawing.Point(138, 170);
            this.cbResult.Name = "cbResult";
            this.cbResult.Size = new System.Drawing.Size(142, 21);
            this.cbResult.TabIndex = 27;
            this.cbResult.SelectedIndexChanged += new System.EventHandler(this.cbResult_SelectedIndexChanged);
            // 
            // EditMatchPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "EditMatchPanel";
            this.Size = new System.Drawing.Size(724, 523);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bAddCom;
        private System.Windows.Forms.Button bCancelChg;
        private System.Windows.Forms.Button bUpdate;
        private System.Windows.Forms.Button bDeleteCom;
        private System.Windows.Forms.Button bChangeCom;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.ListBox lbComments;
        private System.Windows.Forms.Label lComments;
        private System.Windows.Forms.TextBox txtBlack;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtWhite;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMatch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtNewKey;
        private System.Windows.Forms.Button bNewKey;
        private System.Windows.Forms.Button bDelete;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bChangeValue;
        private System.Windows.Forms.ComboBox cbKeywords;
        private System.Windows.Forms.TextBox txtKeyValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbResult;
    }
}
