namespace AIChessDatabase.Controls
{
    partial class ChessPlayer
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup(global::AIChessDatabase.Properties.UIResources.GRP_MATCHKEYS, System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup(global::AIChessDatabase.Properties.UIResources.GRP_MATCHSTS, System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup(global::AIChessDatabase.Properties.UIResources.GRP_POSSTS, System.Windows.Forms.HorizontalAlignment.Left);
            this.txtComments = new System.Windows.Forms.TextBox();
            this.lvKeys = new System.Windows.Forms.ListView();
            this.Keyword = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Value = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.board = new AIChessDatabase.Controls.Board();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.bNew = new System.Windows.Forms.Button();
            this.tmQueue = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // txtComments
            // 
            this.txtComments.AcceptsReturn = true;
            this.txtComments.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.txtComments_Description;
            this.txtComments.AccessibleName = global::AIChessDatabase.Properties.UIElements.txtComments_Name;
            this.txtComments.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtComments.BackColor = System.Drawing.SystemColors.Window;
            this.txtComments.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtComments.Location = new System.Drawing.Point(24, 891);
            this.txtComments.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtComments.Multiline = true;
            this.txtComments.Name = "txtComments";
            this.txtComments.ReadOnly = true;
            this.txtComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtComments.Size = new System.Drawing.Size(473, 199);
            this.txtComments.TabIndex = 1;
            // 
            // lvKeys
            // 
            this.lvKeys.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.lvKeys_Description;
            this.lvKeys.AccessibleName = global::AIChessDatabase.Properties.UIElements.lvKeys_Name;
            this.lvKeys.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
            this.lvKeys.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Keyword,
            this.Value});
            this.lvKeys.FullRowSelect = true;
            this.lvKeys.GridLines = true;
            listViewGroup1.Header = global::AIChessDatabase.Properties.UIResources.GRP_MATCHKEYS;
            listViewGroup1.Name = "lgKeywords";
            listViewGroup2.Header = global::AIChessDatabase.Properties.UIResources.GRP_MATCHSTS;
            listViewGroup2.Name = "lgStatistics";
            listViewGroup3.Header = global::AIChessDatabase.Properties.UIResources.GRP_POSSTS;
            listViewGroup3.Name = "lgPositionsSts";
            this.lvKeys.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this.lvKeys.HideSelection = false;
            this.lvKeys.Location = new System.Drawing.Point(510, 891);
            this.lvKeys.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lvKeys.MultiSelect = false;
            this.lvKeys.Name = "lvKeys";
            this.lvKeys.Size = new System.Drawing.Size(292, 244);
            this.lvKeys.TabIndex = 2;
            this.lvKeys.UseCompatibleStateImageBehavior = false;
            this.lvKeys.View = System.Windows.Forms.View.Details;
            // 
            // Keyword
            // 
            this.Keyword.Text = global::AIChessDatabase.Properties.UIResources.LAB_KEYWORD;
            this.Keyword.Width = 91;
            // 
            // Value
            // 
            this.Value.Text = global::AIChessDatabase.Properties.UIResources.LAB_VALUE;
            this.Value.Width = 106;
            // 
            // board
            // 
            this.board.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.board_Description;
            this.board.AccessibleName = global::AIChessDatabase.Properties.UIElements.board_Name;
            this.board.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
            this.board.AddComments = false;
            this.board.AllowSetPosition = false;
            this.board.BoardPosition = "rnbqkbnrpppppppp00000000000000000000000000000000PPPPPPPPRNBQKBNR";
            this.board.FromTo = new System.Drawing.Point(-1, -1);
            this.board.Location = new System.Drawing.Point(24, 26);
            this.board.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.board.MoveEvents = ((ulong)(0ul));
            this.board.Name = "board";
            this.board.SideView = true;
            this.board.Size = new System.Drawing.Size(585, 681);
            this.board.TabIndex = 0;
            this.board.ResetMatch += new System.EventHandler(this.board_ResetMatch);
            this.board.MoveBack += new System.EventHandler(this.board_MoveBack);
            this.board.MoveForward += new System.EventHandler(this.board_MoveForward);
            this.board.BoardChanged += new System.EventHandler(this.board_BoardChanged);
            this.board.SideViewChanged += new System.EventHandler(this.board_SideViewChanged);
            // 
            // bCancel
            // 
            this.bCancel.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.ChessPlayer_bCancel_Description;
            this.bCancel.AccessibleName = global::AIChessDatabase.Properties.UIElements.bCancel_Name;
            this.bCancel.Enabled = false;
            this.bCancel.Location = new System.Drawing.Point(387, 1101);
            this.bCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(112, 35);
            this.bCancel.TabIndex = 3;
            this.bCancel.Text = global::AIChessDatabase.Properties.UIResources.BTN_CANCEL;
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bOK
            // 
            this.bOK.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.ChessPlayer_bOK_Description;
            this.bOK.AccessibleName = global::AIChessDatabase.Properties.UIElements.bOK_Name;
            this.bOK.Enabled = false;
            this.bOK.Location = new System.Drawing.Point(266, 1101);
            this.bOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(112, 35);
            this.bOK.TabIndex = 4;
            this.bOK.Text = global::AIChessDatabase.Properties.UIResources.BTN_OK;
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // bNew
            // 
            this.bNew.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.ChessPlayer_bNew_Description;
            this.bNew.AccessibleName = global::AIChessDatabase.Properties.UIElements.ChessPlayer_bNew_Name;
            this.bNew.Location = new System.Drawing.Point(24, 1101);
            this.bNew.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bNew.Name = "bNew";
            this.bNew.Size = new System.Drawing.Size(176, 35);
            this.bNew.TabIndex = 5;
            this.bNew.Text = global::AIChessDatabase.Properties.UIResources.BTN_NEWCOMMENT;
            this.bNew.UseVisualStyleBackColor = true;
            this.bNew.Click += new System.EventHandler(this.bNew_Click);
            // 
            // tmQueue
            // 
            this.tmQueue.Interval = 10000;
            this.tmQueue.Tick += new System.EventHandler(this.tmQueue_Tick);
            // 
            // ChessPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SaddleBrown;
            this.Controls.Add(this.bNew);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.lvKeys);
            this.Controls.Add(this.txtComments);
            this.Controls.Add(this.board);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ChessPlayer";
            this.Size = new System.Drawing.Size(838, 1162);
            this.Load += new System.EventHandler(this.ChessPlayer_Load);
            this.SizeChanged += new System.EventHandler(this.ChessPlayer_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtComments;
        private System.Windows.Forms.ListView lvKeys;
        private System.Windows.Forms.ColumnHeader Keyword;
        private System.Windows.Forms.ColumnHeader Value;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Button bNew;
        private System.Windows.Forms.Timer tmQueue;
        public Board board;
    }
}
