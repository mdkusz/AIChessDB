namespace AIChessDatabase.Dialogs
{
    partial class DlgAppConfiguration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgAppConfiguration));
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
            this.panel1.Location = new System.Drawing.Point(0, 2050);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(971, 80);
            this.panel1.TabIndex = 0;
            // 
            // bOK
            // 
            this.bOK.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bOK_Description;
            this.bOK.AccessibleName = global::AIChessDatabase.Properties.UIElements.bOK_Name;
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Location = new System.Drawing.Point(692, 22);
            this.bOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(112, 35);
            this.bOK.TabIndex = 4;
            this.bOK.Text = global::AIChessDatabase.Properties.UIResources.BTN_OK;
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // bCancel
            // 
            this.bCancel.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bCancel_Description;
            this.bCancel.AccessibleName = global::AIChessDatabase.Properties.UIElements.bCancel_Name;
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(834, 22);
            this.bCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(112, 35);
            this.bCancel.TabIndex = 3;
            this.bCancel.Text = global::AIChessDatabase.Properties.UIResources.BTN_CANCEL;
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
            this.flpSettings.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.flpSettings.Name = "flpSettings";
            this.flpSettings.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.flpSettings.Size = new System.Drawing.Size(971, 2050);
            this.flpSettings.TabIndex = 2;
            this.flpSettings.WrapContents = false;
            this.flpSettings.ResizeParent += new System.EventHandler(this.flpSettings_ResizeParent);
            // 
            // DlgAppConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(971, 2130);
            this.Controls.Add(this.flpSettings);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "DlgAppConfiguration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DlgAppConfiguration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DlgAppConfiguration_FormClosing);
            this.Shown += new System.EventHandler(this.DlgAppConfiguration_Shown);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Button bCancel;
        private DesktopControls.Controls.InputEditors.InputEditorFlowContainer flpSettings;
    }
}