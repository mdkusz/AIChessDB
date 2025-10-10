namespace DesktopControls.Dialogs
{
    partial class DataSheetDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataSheetDialog));
            this.panel1 = new System.Windows.Forms.Panel();
            this.bOK = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.flpSettings = new DesktopControls.Controls.InputEditors.InputEditorFlowContainer();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bOK);
            this.panel1.Controls.Add(this.bCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 676);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(683, 80);
            this.panel1.TabIndex = 3;
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Location = new System.Drawing.Point(404, 22);
            this.bOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(112, 35);
            this.bOK.TabIndex = 4;
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(546, 22);
            this.bCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(112, 35);
            this.bCancel.TabIndex = 3;
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // flpSettings
            // 
            this.flpSettings.AutoScroll = true;
            this.flpSettings.BackColor = System.Drawing.SystemColors.Desktop;
            this.flpSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpSettings.EditorFactory = null;
            this.flpSettings.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flpSettings.Location = new System.Drawing.Point(0, 0);
            this.flpSettings.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.flpSettings.Name = "flpSettings";
            this.flpSettings.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.flpSettings.Size = new System.Drawing.Size(683, 676);
            this.flpSettings.TabIndex = 4;
            this.flpSettings.WrapContents = false;
            this.flpSettings.ResizeParent += new System.EventHandler(this.flpSettings_ResizeParent);
            // 
            // DataSheetDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 756);
            this.Controls.Add(this.flpSettings);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "DataSheetDialog";
            this.Text = "DataSheetDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DataSheetDialog_FormClosing);
            this.Shown += new System.EventHandler(this.DataSheetDialog_Shown);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DesktopControls.Controls.InputEditors.InputEditorFlowContainer flpSettings;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Button bCancel;
    }
}