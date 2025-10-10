namespace AIChessDatabase.Dialogs
{
    partial class DlgKeywordConsolidation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgKeywordConsolidation));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cbKeyword = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbValues = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.bFilter = new System.Windows.Forms.Button();
            this.bFilterEq = new System.Windows.Forms.Button();
            this.txtFilterEq = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lbEquivalences = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.bConsolidate = new System.Windows.Forms.Button();
            this.bClose = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCanonical = new System.Windows.Forms.TextBox();
            this.bRefresh = new System.Windows.Forms.Button();
            this.bRefreshEq = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Keyword";
            // 
            // cbKeyword
            // 
            this.cbKeyword.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbKeyword_Description;
            this.cbKeyword.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbKeyword_Name;
            this.cbKeyword.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.cbKeyword.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKeyword.FormattingEnabled = true;
            this.cbKeyword.Location = new System.Drawing.Point(16, 32);
            this.cbKeyword.Margin = new System.Windows.Forms.Padding(4);
            this.cbKeyword.Name = "cbKeyword";
            this.cbKeyword.Size = new System.Drawing.Size(333, 24);
            this.cbKeyword.Sorted = true;
            this.cbKeyword.TabIndex = 1;
            this.cbKeyword.SelectedIndexChanged += new System.EventHandler(this.cbKeyword_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 81);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Values";
            // 
            // lbValues
            // 
            this.lbValues.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.lbValues_Description;
            this.lbValues.AccessibleName = global::AIChessDatabase.Properties.UIElements.lbValues_Name;
            this.lbValues.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
            this.lbValues.FormattingEnabled = true;
            this.lbValues.ItemHeight = 16;
            this.lbValues.Location = new System.Drawing.Point(16, 101);
            this.lbValues.Margin = new System.Windows.Forms.Padding(4);
            this.lbValues.Name = "lbValues";
            this.lbValues.Size = new System.Drawing.Size(333, 388);
            this.lbValues.Sorted = true;
            this.lbValues.TabIndex = 3;
            this.lbValues.SelectedIndexChanged += new System.EventHandler(this.lbValues_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 502);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "FIlter";
            // 
            // txtFilter
            // 
            this.txtFilter.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.txtFilter_Description;
            this.txtFilter.AccessibleName = global::AIChessDatabase.Properties.UIElements.txtFilter_Name;
            this.txtFilter.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
            this.txtFilter.Location = new System.Drawing.Point(59, 498);
            this.txtFilter.Margin = new System.Windows.Forms.Padding(4);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(251, 22);
            this.txtFilter.TabIndex = 5;
            // 
            // bFilter
            // 
            this.bFilter.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bFilter_Description;
            this.bFilter.AccessibleName = global::AIChessDatabase.Properties.UIElements.bFilter_Name;
            this.bFilter.Image = ((System.Drawing.Image)(resources.GetObject("bFilter.Image")));
            this.bFilter.Location = new System.Drawing.Point(319, 496);
            this.bFilter.Margin = new System.Windows.Forms.Padding(4);
            this.bFilter.Name = "bFilter";
            this.bFilter.Size = new System.Drawing.Size(32, 30);
            this.bFilter.TabIndex = 6;
            this.bFilter.UseVisualStyleBackColor = true;
            this.bFilter.Click += new System.EventHandler(this.bFilter_Click);
            // 
            // bFilterEq
            // 
            this.bFilterEq.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bFilterEq_Description;
            this.bFilterEq.AccessibleName = global::AIChessDatabase.Properties.UIElements.bFilter_Name;
            this.bFilterEq.Image = ((System.Drawing.Image)(resources.GetObject("bFilterEq.Image")));
            this.bFilterEq.Location = new System.Drawing.Point(703, 496);
            this.bFilterEq.Margin = new System.Windows.Forms.Padding(4);
            this.bFilterEq.Name = "bFilterEq";
            this.bFilterEq.Size = new System.Drawing.Size(32, 30);
            this.bFilterEq.TabIndex = 11;
            this.bFilterEq.UseVisualStyleBackColor = true;
            this.bFilterEq.Click += new System.EventHandler(this.bFilterEq_Click);
            // 
            // txtFilterEq
            // 
            this.txtFilterEq.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.txtFilterEq_Description;
            this.txtFilterEq.AccessibleName = global::AIChessDatabase.Properties.UIElements.txtFilterEq_Name;
            this.txtFilterEq.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtFilterEq.Location = new System.Drawing.Point(443, 498);
            this.txtFilterEq.Margin = new System.Windows.Forms.Padding(4);
            this.txtFilterEq.Name = "txtFilterEq";
            this.txtFilterEq.Size = new System.Drawing.Size(251, 22);
            this.txtFilterEq.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(396, 503);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "FIlter";
            // 
            // lbEquivalences
            // 
            this.lbEquivalences.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.lbEquivalences_Description;
            this.lbEquivalences.AccessibleName = global::AIChessDatabase.Properties.UIElements.lbEquivalences_Name;
            this.lbEquivalences.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
            this.lbEquivalences.FormattingEnabled = true;
            this.lbEquivalences.ItemHeight = 16;
            this.lbEquivalences.Location = new System.Drawing.Point(400, 101);
            this.lbEquivalences.Margin = new System.Windows.Forms.Padding(4);
            this.lbEquivalences.Name = "lbEquivalences";
            this.lbEquivalences.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbEquivalences.Size = new System.Drawing.Size(333, 388);
            this.lbEquivalences.Sorted = true;
            this.lbEquivalences.TabIndex = 8;
            this.lbEquivalences.SelectedIndexChanged += new System.EventHandler(this.lbEquivalences_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(396, 81);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 16);
            this.label5.TabIndex = 7;
            this.label5.Text = "Equivalences";
            // 
            // bConsolidate
            // 
            this.bConsolidate.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bConsolidate_Description;
            this.bConsolidate.AccessibleName = global::AIChessDatabase.Properties.UIElements.bConsolidate_Name;
            this.bConsolidate.Enabled = false;
            this.bConsolidate.Location = new System.Drawing.Point(513, 561);
            this.bConsolidate.Margin = new System.Windows.Forms.Padding(4);
            this.bConsolidate.Name = "bConsolidate";
            this.bConsolidate.Size = new System.Drawing.Size(100, 28);
            this.bConsolidate.TabIndex = 12;
            this.bConsolidate.Text = global::AIChessDatabase.Properties.UIResources.BTN_CONSOLIDATE;
            this.bConsolidate.UseVisualStyleBackColor = true;
            this.bConsolidate.Click += new System.EventHandler(this.bConsolidate_Click);
            // 
            // bClose
            // 
            this.bClose.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bClose_Description;
            this.bClose.AccessibleName = global::AIChessDatabase.Properties.UIElements.bClose_Name;
            this.bClose.Location = new System.Drawing.Point(637, 561);
            this.bClose.Margin = new System.Windows.Forms.Padding(4);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(100, 28);
            this.bClose.TabIndex = 13;
            this.bClose.Text = global::AIChessDatabase.Properties.UIResources.BTN_CLOSE;
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(396, 11);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 16);
            this.label6.TabIndex = 14;
            this.label6.Text = "Canonical Value";
            // 
            // txtCanonical
            // 
            this.txtCanonical.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.txtCanonical_Description;
            this.txtCanonical.AccessibleName = global::AIChessDatabase.Properties.UIElements.txtCanonical_Name;
            this.txtCanonical.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtCanonical.Location = new System.Drawing.Point(400, 33);
            this.txtCanonical.Margin = new System.Windows.Forms.Padding(4);
            this.txtCanonical.Name = "txtCanonical";
            this.txtCanonical.Size = new System.Drawing.Size(333, 22);
            this.txtCanonical.TabIndex = 15;
            // 
            // bRefresh
            // 
            this.bRefresh.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bRefresh_Description;
            this.bRefresh.AccessibleName = global::AIChessDatabase.Properties.UIElements.bRefresh_Name;
            this.bRefresh.Image = ((System.Drawing.Image)(resources.GetObject("bRefresh.Image")));
            this.bRefresh.Location = new System.Drawing.Point(319, 68);
            this.bRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(32, 30);
            this.bRefresh.TabIndex = 16;
            this.bRefresh.UseVisualStyleBackColor = true;
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // bRefreshEq
            // 
            this.bRefreshEq.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bRefreshEq_Description;
            this.bRefreshEq.AccessibleName = global::AIChessDatabase.Properties.UIElements.bRefresh_Name;
            this.bRefreshEq.Image = ((System.Drawing.Image)(resources.GetObject("bRefreshEq.Image")));
            this.bRefreshEq.Location = new System.Drawing.Point(703, 68);
            this.bRefreshEq.Margin = new System.Windows.Forms.Padding(4);
            this.bRefreshEq.Name = "bRefreshEq";
            this.bRefreshEq.Size = new System.Drawing.Size(32, 30);
            this.bRefreshEq.TabIndex = 17;
            this.bRefreshEq.UseVisualStyleBackColor = true;
            this.bRefreshEq.Click += new System.EventHandler(this.bRefreshEq_Click);
            //
            // panel1
            //
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Size = new System.Drawing.Size(753, 606);
            this.panel1.TabIndex = 18;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.panel1.Name = "panel1";
            this.panel1.Controls.Add(this.bRefreshEq);
            this.panel1.Controls.Add(this.bRefresh);
            this.panel1.Controls.Add(this.txtCanonical);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.bClose);
            this.panel1.Controls.Add(this.bConsolidate);
            this.panel1.Controls.Add(this.bFilterEq);
            this.panel1.Controls.Add(this.txtFilterEq);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lbEquivalences);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.bFilter);
            this.panel1.Controls.Add(this.txtFilter);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lbValues);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbKeyword);
            this.panel1.Controls.Add(this.label1);
            // 
            // DlgKeywordConsolidation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 606);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgKeywordConsolidation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Consolidate Keywords";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DlgKeywordConsolidation_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DlgKeywordConsolidation_FormClosed);
            this.Shown += new System.EventHandler(this.DlgKeywordConsolidation_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbKeyword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lbValues;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Button bFilter;
        private System.Windows.Forms.Button bFilterEq;
        private System.Windows.Forms.TextBox txtFilterEq;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox lbEquivalences;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button bConsolidate;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCanonical;
        private System.Windows.Forms.Button bRefresh;
        private System.Windows.Forms.Button bRefreshEq;
    }
}