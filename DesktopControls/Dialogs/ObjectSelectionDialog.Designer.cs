namespace DesktopControls.Dialogs
{
    partial class ObjectSelectionDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjectSelectionDialog));
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.bRefresh = new System.Windows.Forms.ToolStripButton();
            this.bAddMode = new System.Windows.Forms.ToolStripButton();
            this.bDeleteMode = new System.Windows.Forms.ToolStripButton();
            this.bProceed = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bRemoveObject = new System.Windows.Forms.ToolStripButton();
            this.lvSelection = new System.Windows.Forms.ListView();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 32);
            this.panel1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.toolStrip1.AccessibleName = global::DesktopControls.Properties.UIElements.ObjectSelector_toolbar_Name;
            this.toolStrip1.AccessibleDescription = global::DesktopControls.Properties.UIElements.ObjectSelector_toolbar_Description;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bRefresh,
            this.bAddMode,
            this.bDeleteMode,
            this.bProceed,
            this.toolStripSeparator1,
            this.bRemoveObject});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // bRefresh
            // 
            this.bRefresh.AccessibleRole = System.Windows.Forms.AccessibleRole.Default;
            this.bRefresh.AccessibleName = global::DesktopControls.Properties.UIElements.ObjectSelector_bRefresh_Name;
            this.bRefresh.AccessibleDescription = global::DesktopControls.Properties.UIElements.ObjectSelector_bRefresh_Description;
            this.bRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bRefresh.Image = ((System.Drawing.Image)(resources.GetObject("bRefresh.Image")));
            this.bRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(29, 24);
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // bAddMode
            // 
            this.bAddMode.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.bAddMode.AccessibleName = global::DesktopControls.Properties.UIElements.ObjectSelector_bAddMode_Name;
            this.bAddMode.AccessibleDescription = global::DesktopControls.Properties.UIElements.ObjectSelector_bAddMode_Description;
            this.bAddMode.Checked = true;
            this.bAddMode.CheckOnClick = true;
            this.bAddMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bAddMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bAddMode.Image = ((System.Drawing.Image)(resources.GetObject("bAddMode.Image")));
            this.bAddMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bAddMode.Name = "bAddMode";
            this.bAddMode.Size = new System.Drawing.Size(29, 24);
            this.bAddMode.Click += new System.EventHandler(this.bAddMode_Click);
            // 
            // bDeleteMode
            // 
            this.bDeleteMode.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.bDeleteMode.AccessibleName = global::DesktopControls.Properties.UIElements.ObjectSelector_bDeleteMode_Name;
            this.bDeleteMode.AccessibleDescription = global::DesktopControls.Properties.UIElements.ObjectSelector_bDeleteMode_Description;
            this.bDeleteMode.CheckOnClick = true;
            this.bDeleteMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bDeleteMode.Image = ((System.Drawing.Image)(resources.GetObject("bDeleteMode.Image")));
            this.bDeleteMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bDeleteMode.Name = "bDeleteMode";
            this.bDeleteMode.Size = new System.Drawing.Size(29, 24);
            this.bDeleteMode.Click += new System.EventHandler(this.bDeleteMode_Click);
            // 
            // bProceed
            // 
            this.bProceed.AccessibleRole = System.Windows.Forms.AccessibleRole.Default;
            this.bProceed.AccessibleName = global::DesktopControls.Properties.UIElements.ObjectSelector_bProceed_Name;
            this.bProceed.AccessibleDescription = global::DesktopControls.Properties.UIElements.ObjectSelector_bProceed_Description;
            this.bProceed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bProceed.Enabled = false;
            this.bProceed.Image = ((System.Drawing.Image)(resources.GetObject("bProceed.Image")));
            this.bProceed.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bProceed.Name = "bProceed";
            this.bProceed.Size = new System.Drawing.Size(29, 24);
            this.bProceed.Click += new System.EventHandler(this.bProceed_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // bRemoveObject
            // 
            this.bRemoveObject.AccessibleRole = System.Windows.Forms.AccessibleRole.Default;
            this.bRemoveObject.AccessibleName = global::DesktopControls.Properties.UIElements.ObjectSelector_bRemoveObject_Name;
            this.bRemoveObject.AccessibleDescription = global::DesktopControls.Properties.UIElements.ObjectSelector_bRemoveObject_Description;
            this.bRemoveObject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bRemoveObject.Image = ((System.Drawing.Image)(resources.GetObject("bRemoveObject.Image")));
            this.bRemoveObject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bRemoveObject.Name = "bRemoveObject";
            this.bRemoveObject.Size = new System.Drawing.Size(29, 24);
            this.bRemoveObject.Click += new System.EventHandler(this.bRemoveObject_Click);
            // 
            // lvSelection
            // 
            this.lvSelection.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
            this.lvSelection.AccessibleName = global::DesktopControls.Properties.UIElements.ObjectSelector_lvSelection_Name;
            this.lvSelection.AccessibleDescription = global::DesktopControls.Properties.UIElements.ObjectSelector_lvSelection_Description;
            this.lvSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSelection.HideSelection = false;
            this.lvSelection.Location = new System.Drawing.Point(0, 32);
            this.lvSelection.MultiSelect = false;
            this.lvSelection.Name = "lvSelection";
            this.lvSelection.Size = new System.Drawing.Size(800, 418);
            this.lvSelection.TabIndex = 1;
            this.lvSelection.UseCompatibleStateImageBehavior = false;
            this.lvSelection.View = System.Windows.Forms.View.Details;
            this.lvSelection.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvSelection_ItemSelectionChanged);
            // 
            // ObjectSelectionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lvSelection);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ObjectSelectionDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ObjectSelectionDialog";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ListView lvSelection;
        private System.Windows.Forms.ToolStripButton bRefresh;
        private System.Windows.Forms.ToolStripButton bAddMode;
        private System.Windows.Forms.ToolStripButton bDeleteMode;
        private System.Windows.Forms.ToolStripButton bProceed;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton bRemoveObject;
    }
}