namespace AIChessDatabase.Controls
{
    partial class PositionViewer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PositionViewer));
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.bFind = new System.Windows.Forms.ToolStripButton();
            this.lMoves = new System.Windows.Forms.ToolStripLabel();
            this.txtMoves = new System.Windows.Forms.ToolStripTextBox();
            this.lResult = new System.Windows.Forms.ToolStripLabel();
            this.cbResult = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bShow = new System.Windows.Forms.ToolStripButton();
            this.pBoards = new System.Windows.Forms.FlowLayoutPanel();
            this.dgMatches = new QueryDesktopControls.Controls.QueryDataGridPanel();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMatches.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(642, 25);
            this.panel1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.toolStrip1.AccessibleName = global::AIChessDatabase.Properties.UIElements.PositionViewer_toolStrip1_Name;
            this.toolStrip1.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.PositionViewer_toolStrip1_Description;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bFind,
            this.lMoves,
            this.txtMoves,
            this.lResult,
            this.cbResult,
            this.toolStripSeparator1,
            this.bShow});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(642, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // bFind
            // 
            this.bFind.AccessibleRole = System.Windows.Forms.AccessibleRole.Default;
            this.bFind.AccessibleName = global::AIChessDatabase.Properties.UIElements.bFind_Name;
            this.bFind.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bFind_Description;
            this.bFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bFind.Enabled = false;
            this.bFind.Image = ((System.Drawing.Image)(resources.GetObject("bFind.Image")));
            this.bFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bFind.Name = "bFind";
            this.bFind.Size = new System.Drawing.Size(23, 22);
            this.bFind.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_FINDPOS;
            this.bFind.Click += new System.EventHandler(this.bFind_Click);
            // 
            // lMoves
            // 
            this.lMoves.Name = "lMoves";
            this.lMoves.Size = new System.Drawing.Size(42, 22);
            this.lMoves.Text = "Moves";
            // 
            // txtMoves
            // 
            this.txtMoves.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtMoves.AccessibleName = global::AIChessDatabase.Properties.UIElements.txtMoves_Name;
            this.txtMoves.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.txtMoves_Description;
            this.txtMoves.Name = "txtMoves";
            this.txtMoves.Size = new System.Drawing.Size(50, 25);
            this.txtMoves.Text = "4";
            this.txtMoves.TextChanged += new System.EventHandler(this.txtMoves_TextChanged);
            // 
            // lResult
            // 
            this.lResult.Name = "lResult";
            this.lResult.Size = new System.Drawing.Size(39, 22);
            this.lResult.Text = "Result";
            // 
            // cbResult
            // 
            this.cbResult.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.cbResult.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbResult_Name;
            this.cbResult.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbResult_Description;
            this.cbResult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbResult.Items.AddRange(new object[] {
            "all",
            "*",
            "1-0",
            "0-1",
            "1/2-1/2"});
            this.cbResult.Name = "cbResult";
            this.cbResult.Size = new System.Drawing.Size(90, 25);
            this.cbResult.SelectedIndexChanged += new System.EventHandler(this.txtMoves_TextChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bShow
            // 
            this.bShow.AccessibleRole = System.Windows.Forms.AccessibleRole.Default;
            this.bShow.AccessibleName = global::AIChessDatabase.Properties.UIElements.bShow_Name;
            this.bShow.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bShow_Description;
            this.bShow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bShow.Enabled = false;
            this.bShow.Image = ((System.Drawing.Image)(resources.GetObject("bShow.Image")));
            this.bShow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bShow.Name = "bShow";
            this.bShow.Size = new System.Drawing.Size(23, 22);
            this.bShow.ToolTipText = global::AIChessDatabase.Properties.UIResources.TTP_VIEWMATCH;
            this.bShow.Click += new System.EventHandler(this.bShow_Click);
            // 
            // pBoards
            // 
            this.pBoards.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
            this.pBoards.AccessibleName = global::AIChessDatabase.Properties.UIElements.pBoards_Name;
            this.pBoards.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.pBoards_Description;
            this.pBoards.AutoScroll = true;
            this.pBoards.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pBoards.Location = new System.Drawing.Point(0, 345);
            this.pBoards.Name = "pBoards";
            this.pBoards.Size = new System.Drawing.Size(642, 202);
            this.pBoards.TabIndex = 1;
            this.pBoards.WrapContents = false;
            // 
            // dgMatches
            // 
            this.dgMatches.AccessibleRole = System.Windows.Forms.AccessibleRole.Table;
            this.dgMatches.AccessibleName = global::AIChessDatabase.Properties.UIElements.dgMatches_Name;
            this.dgMatches.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.dgMatches_Description;
            this.dgMatches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgMatches.FOManager = null;
            this.dgMatches.Location = new System.Drawing.Point(0, 25);
            this.dgMatches.Name = "dgMatches";
            this.dgMatches.Size = new System.Drawing.Size(642, 320);
            this.dgMatches.Style = ((QueryDesktopControls.Controls.QueryDataGridPanel.GridPanelStyle)(((QueryDesktopControls.Controls.QueryDataGridPanel.GridPanelStyle.ManageQuery))));
            this.dgMatches.TabIndex = 4;
            this.dgMatches.Title = "";
            this.dgMatches.SelectionChanged += new System.EventHandler(this.dgMatches_SelectionChanged);
            this.dgMatches.QueryChanged += new System.EventHandler(this.dgMatches_QueryChanged);
            // 
            // PositionViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgMatches);
            this.Controls.Add(this.pBoards);
            this.Controls.Add(this.panel1);
            this.Name = "PositionViewer";
            this.Size = new System.Drawing.Size(642, 547);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMatches.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.FlowLayoutPanel pBoards;
        private QueryDesktopControls.Controls.QueryDataGridPanel dgMatches;
        private System.Windows.Forms.ToolStripButton bFind;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton bShow;
        private System.Windows.Forms.ToolStripTextBox txtMoves;
        private System.Windows.Forms.ToolStripLabel lMoves;
        private System.Windows.Forms.ToolStripLabel lResult;
        private System.Windows.Forms.ToolStripComboBox cbResult;
    }
}
