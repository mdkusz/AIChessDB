namespace DesktopControls.Dialogs
{
    partial class DlgInputText
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgInputText));
            this.lPrompt = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.bOK = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lPrompt
            // 
            this.lPrompt.AutoSize = true;
            this.lPrompt.Location = new System.Drawing.Point(24, 20);
            this.lPrompt.Name = "lPrompt";
            this.lPrompt.Size = new System.Drawing.Size(70, 16);
            this.lPrompt.TabIndex = 0;
            this.lPrompt.Text = "Enter Text:";
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(27, 39);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(746, 22);
            this.txtInput.TabIndex = 1;
            // 
            // bOK
            // 
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Image = ((System.Drawing.Image)(resources.GetObject("bOK.Image")));
            this.bOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bOK.Location = new System.Drawing.Point(528, 80);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(105, 37);
            this.bOK.TabIndex = 2;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Image = ((System.Drawing.Image)(resources.GetObject("bCancel.Image")));
            this.bCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bCancel.Location = new System.Drawing.Point(668, 80);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(105, 37);
            this.bCancel.TabIndex = 3;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // DlgInputText
            // 
            this.AcceptButton = this.bOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(800, 134);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.lPrompt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgInputText";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lPrompt;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Button bCancel;
    }
}