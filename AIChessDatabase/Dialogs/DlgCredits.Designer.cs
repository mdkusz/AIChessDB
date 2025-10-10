namespace AIChessDatabase.Dialogs
{
    partial class DlgCredits
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgCredits));
            this.lAuthor = new System.Windows.Forms.Label();
            this.lPieces = new System.Windows.Forms.Label();
            this.llPieces = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.llLicense = new System.Windows.Forms.LinkLabel();
            this.lIcon = new System.Windows.Forms.Label();
            this.llIcon = new System.Windows.Forms.LinkLabel();
            this.llIcons8 = new System.Windows.Forms.LinkLabel();
            this.bOK = new System.Windows.Forms.Button();
            this.llAuthor = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lAuthor
            // 
            this.lAuthor.AutoSize = true;
            this.lAuthor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lAuthor.Location = new System.Drawing.Point(17, 16);
            this.lAuthor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lAuthor.Name = "lAuthor";
            this.lAuthor.Size = new System.Drawing.Size(61, 17);
            this.lAuthor.TabIndex = 0;
            this.lAuthor.Text = "Author:";
            // 
            // lPieces
            // 
            this.lPieces.AutoSize = true;
            this.lPieces.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lPieces.Location = new System.Drawing.Point(17, 55);
            this.lPieces.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lPieces.Name = "lPieces";
            this.lPieces.Size = new System.Drawing.Size(82, 17);
            this.lPieces.TabIndex = 2;
            this.lPieces.Text = "Piece Set:";
            // 
            // llPieces
            // 
            this.llPieces.AutoSize = true;
            this.llPieces.Location = new System.Drawing.Point(113, 55);
            this.llPieces.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.llPieces.Name = "llPieces";
            this.llPieces.Size = new System.Drawing.Size(152, 16);
            this.llPieces.TabIndex = 3;
            this.llPieces.TabStop = true;
            this.llPieces.Text = "Wikipedia user: Cburnett";
            this.llPieces.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llPieces_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(276, 55);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "/";
            // 
            // llLicense
            // 
            this.llLicense.AutoSize = true;
            this.llLicense.Location = new System.Drawing.Point(296, 55);
            this.llLicense.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.llLicense.Name = "llLicense";
            this.llLicense.Size = new System.Drawing.Size(68, 16);
            this.llLicense.TabIndex = 5;
            this.llLicense.TabStop = true;
            this.llLicense.Text = "CC BY-SA";
            this.llLicense.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llLicense_LinkClicked);
            // 
            // lIcon
            // 
            this.lIcon.AutoSize = true;
            this.lIcon.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lIcon.Location = new System.Drawing.Point(17, 95);
            this.lIcon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lIcon.Name = "lIcon";
            this.lIcon.Size = new System.Drawing.Size(128, 17);
            this.lIcon.TabIndex = 6;
            this.lIcon.Text = "Application Icon:";
            // 
            // llIcon
            // 
            this.llIcon.AutoSize = true;
            this.llIcon.Location = new System.Drawing.Point(164, 95);
            this.llIcon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.llIcon.Name = "llIcon";
            this.llIcon.Size = new System.Drawing.Size(109, 16);
            this.llIcon.TabIndex = 7;
            this.llIcon.TabStop = true;
            this.llIcon.Text = "Chessboard icon";
            this.llIcon.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llIcon_LinkClicked);
            // 
            // llIcons8
            // 
            this.llIcons8.AutoSize = true;
            this.llIcons8.Location = new System.Drawing.Point(276, 95);
            this.llIcons8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.llIcons8.Name = "llIcons8";
            this.llIcons8.Size = new System.Drawing.Size(92, 16);
            this.llIcons8.TabIndex = 8;
            this.llIcons8.TabStop = true;
            this.llIcons8.Text = "icon by Icons8";
            this.llIcons8.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llIcons8_LinkClicked);
            // 
            // bOK
            // 
            this.bOK.Location = new System.Drawing.Point(365, 144);
            this.bOK.Margin = new System.Windows.Forms.Padding(4);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(100, 28);
            this.bOK.TabIndex = 9;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // llAuthor
            // 
            this.llAuthor.AutoSize = true;
            this.llAuthor.Location = new System.Drawing.Point(85, 17);
            this.llAuthor.Name = "llAuthor";
            this.llAuthor.Size = new System.Drawing.Size(171, 16);
            this.llAuthor.TabIndex = 10;
            this.llAuthor.TabStop = true;
            this.llAuthor.Text = "Miguel Díaz Kusztrich (2025)";
            this.llAuthor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llAuthor_LinkClicked);
            // 
            // DlgCredits
            // 
            this.AcceptButton = this.bOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 187);
            this.Controls.Add(this.llAuthor);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.llIcons8);
            this.Controls.Add(this.llIcon);
            this.Controls.Add(this.lIcon);
            this.Controls.Add(this.llLicense);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.llPieces);
            this.Controls.Add(this.lPieces);
            this.Controls.Add(this.lAuthor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgCredits";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "DlgCredits";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lAuthor;
        private System.Windows.Forms.Label lPieces;
        private System.Windows.Forms.LinkLabel llPieces;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel llLicense;
        private System.Windows.Forms.Label lIcon;
        private System.Windows.Forms.LinkLabel llIcon;
        private System.Windows.Forms.LinkLabel llIcons8;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.LinkLabel llAuthor;
    }
}