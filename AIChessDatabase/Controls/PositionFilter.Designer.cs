namespace AIChessDatabase.Controls
{
    partial class PositionFilter
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
            this.txtCustom = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.bSave = new System.Windows.Forms.Button();
            this.bUse = new System.Windows.Forms.Button();
            this.cbBoards = new System.Windows.Forms.ComboBox();
            this.lStored = new System.Windows.Forms.Label();
            this.bReset = new System.Windows.Forms.Button();
            this.bClear = new System.Windows.Forms.Button();
            this.cbSimple = new System.Windows.Forms.CheckBox();
            this.cbSimpleBoard = new AIChessDatabase.Controls.SimpleConfigurableBoard();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.bOK = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.cbBoard = new AIChessDatabase.Controls.ConfigurableBoard();
            this.SuspendLayout();
            // 
            // txtCustom
            // 
            this.txtCustom.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.txtCustom_Description;
            this.txtCustom.AccessibleName = global::AIChessDatabase.Properties.UIElements.txtCustom_Name;
            this.txtCustom.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtCustom.Location = new System.Drawing.Point(25, 985);
            this.txtCustom.Margin = new System.Windows.Forms.Padding(4);
            this.txtCustom.Name = "txtCustom";
            this.txtCustom.Size = new System.Drawing.Size(691, 22);
            this.txtCustom.TabIndex = 29;
            this.txtCustom.TextChanged += new System.EventHandler(this.txtCustom_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 965);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 16);
            this.label2.TabIndex = 28;
            this.label2.Text = "Custom Expression";
            // 
            // bSave
            // 
            this.bSave.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bSave_Description;
            this.bSave.AccessibleName = global::AIChessDatabase.Properties.UIElements.bSave_Name;
            this.bSave.Enabled = false;
            this.bSave.Location = new System.Drawing.Point(615, 923);
            this.bSave.Margin = new System.Windows.Forms.Padding(4);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(103, 28);
            this.bSave.TabIndex = 27;
            this.bSave.Text = global::AIChessDatabase.Properties.UIResources.BTN_SAVE;
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bUse
            // 
            this.bUse.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bUse_Description;
            this.bUse.AccessibleName = global::AIChessDatabase.Properties.UIElements.bUse_Name;
            this.bUse.Enabled = false;
            this.bUse.Location = new System.Drawing.Point(511, 923);
            this.bUse.Margin = new System.Windows.Forms.Padding(4);
            this.bUse.Name = "bUse";
            this.bUse.Size = new System.Drawing.Size(96, 28);
            this.bUse.TabIndex = 26;
            this.bUse.Text = global::AIChessDatabase.Properties.UIResources.BTN_USE;
            this.bUse.UseVisualStyleBackColor = true;
            this.bUse.Click += new System.EventHandler(this.bUse_Click);
            // 
            // cbBoards
            // 
            this.cbBoards.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbBoards_Description;
            this.cbBoards.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbBoards_Name;
            this.cbBoards.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.cbBoards.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBoards.FormattingEnabled = true;
            this.cbBoards.Location = new System.Drawing.Point(204, 923);
            this.cbBoards.Margin = new System.Windows.Forms.Padding(4);
            this.cbBoards.Name = "cbBoards";
            this.cbBoards.Size = new System.Drawing.Size(297, 24);
            this.cbBoards.Sorted = true;
            this.cbBoards.TabIndex = 25;
            this.cbBoards.SelectedIndexChanged += new System.EventHandler(this.cbBoards_SelectedIndexChanged);
            // 
            // lStored
            // 
            this.lStored.AutoSize = true;
            this.lStored.Location = new System.Drawing.Point(200, 898);
            this.lStored.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lStored.Name = "lStored";
            this.lStored.Size = new System.Drawing.Size(94, 16);
            this.lStored.TabIndex = 24;
            this.lStored.Text = "Stored Boards";
            // 
            // bReset
            // 
            this.bReset.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.PositionFilter_bReset_Description;
            this.bReset.AccessibleName = global::AIChessDatabase.Properties.UIElements.PositionFilter_bReset_Name;
            this.bReset.Location = new System.Drawing.Point(112, 921);
            this.bReset.Margin = new System.Windows.Forms.Padding(4);
            this.bReset.Name = "bReset";
            this.bReset.Size = new System.Drawing.Size(84, 28);
            this.bReset.TabIndex = 23;
            this.bReset.Text = global::AIChessDatabase.Properties.UIResources.BTN_RESET;
            this.bReset.UseVisualStyleBackColor = true;
            this.bReset.Click += new System.EventHandler(this.bReset_Click);
            // 
            // bClear
            // 
            this.bClear.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bClear_Description;
            this.bClear.AccessibleName = global::AIChessDatabase.Properties.UIElements.bClear_Name;
            this.bClear.Location = new System.Drawing.Point(24, 921);
            this.bClear.Margin = new System.Windows.Forms.Padding(4);
            this.bClear.Name = "bClear";
            this.bClear.Size = new System.Drawing.Size(80, 28);
            this.bClear.TabIndex = 22;
            this.bClear.Text = global::AIChessDatabase.Properties.UIResources.BTN_CLEAR;
            this.bClear.UseVisualStyleBackColor = true;
            this.bClear.Click += new System.EventHandler(this.bClear_Click);
            // 
            // cbSimple
            // 
            this.cbSimple.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbSimple_Description;
            this.cbSimple.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbSimple_Name;
            this.cbSimple.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbSimple.AutoSize = true;
            this.cbSimple.Location = new System.Drawing.Point(24, 893);
            this.cbSimple.Margin = new System.Windows.Forms.Padding(4);
            this.cbSimple.Name = "cbSimple";
            this.cbSimple.Size = new System.Drawing.Size(109, 20);
            this.cbSimple.TabIndex = 21;
            this.cbSimple.Text = global::AIChessDatabase.Properties.UIResources.CB_SIMPLEBOARDED;
            this.cbSimple.UseVisualStyleBackColor = true;
            this.cbSimple.CheckedChanged += new System.EventHandler(this.cbSimple_CheckedChanged);
            // 
            // cbSimpleBoard
            // 
            this.cbSimpleBoard.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbSimpleBoard_Description;
            this.cbSimpleBoard.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbSimpleBoard_Name;
            this.cbSimpleBoard.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
            this.cbSimpleBoard.Board = "0000000000000000000000000000000000000000000000000000000000000000";
            this.cbSimpleBoard.FromTo = new System.Drawing.Point(-1, -1);
            this.cbSimpleBoard.Location = new System.Drawing.Point(24, 24);
            this.cbSimpleBoard.Margin = new System.Windows.Forms.Padding(5);
            this.cbSimpleBoard.Name = "cbSimpleBoard";
            this.cbSimpleBoard.Size = new System.Drawing.Size(693, 788);
            this.cbSimpleBoard.TabIndex = 20;
            this.cbSimpleBoard.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 1016);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 16);
            this.label1.TabIndex = 19;
            this.label1.Text = "Description";
            // 
            // txtDescription
            // 
            this.txtDescription.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.txtDescription_Description;
            this.txtDescription.AccessibleName = global::AIChessDatabase.Properties.UIElements.txtDescription_Name;
            this.txtDescription.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(24, 1035);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(4);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(433, 22);
            this.txtDescription.TabIndex = 18;
            this.txtDescription.TextChanged += new System.EventHandler(this.txtDescription_TextChanged);
            // 
            // bOK
            // 
            this.bOK.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bOK_Description;
            this.bOK.AccessibleName = global::AIChessDatabase.Properties.UIElements.bOK_Name;
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.Location = new System.Drawing.Point(489, 1033);
            this.bOK.Margin = new System.Windows.Forms.Padding(4);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(100, 28);
            this.bOK.TabIndex = 17;
            this.bOK.Text = global::AIChessDatabase.Properties.UIResources.BTN_OK;
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // bCancel
            // 
            this.bCancel.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bCancel_Description;
            this.bCancel.AccessibleName = global::AIChessDatabase.Properties.UIElements.bCancel_Name;
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.Location = new System.Drawing.Point(617, 1033);
            this.bCancel.Margin = new System.Windows.Forms.Padding(4);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(100, 28);
            this.bCancel.TabIndex = 16;
            this.bCancel.Text = global::AIChessDatabase.Properties.UIResources.BTN_CANCEL;
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // cbBoard
            // 
            this.cbBoard.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbBoard_Description;
            this.cbBoard.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbBoard_Name;
            this.cbBoard.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
            this.cbBoard.AllowDrop = true;
            this.cbBoard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cbBoard.Expression = "";
            this.cbBoard.Location = new System.Drawing.Point(24, 24);
            this.cbBoard.Margin = new System.Windows.Forms.Padding(5);
            this.cbBoard.Name = "cbBoard";
            this.cbBoard.Selection = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.cbBoard.Size = new System.Drawing.Size(693, 861);
            this.cbBoard.TabIndex = 15;
            // 
            // PositionFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtCustom);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.bUse);
            this.Controls.Add(this.cbBoards);
            this.Controls.Add(this.lStored);
            this.Controls.Add(this.bReset);
            this.Controls.Add(this.bClear);
            this.Controls.Add(this.cbSimple);
            this.Controls.Add(this.cbSimpleBoard);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.cbBoard);
            this.MinimumSize = new System.Drawing.Size(742, 1080);
            this.Name = "PositionFilter";
            this.Size = new System.Drawing.Size(742, 1084);
            this.Load += new System.EventHandler(this.PositionFilter_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCustom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bUse;
        private System.Windows.Forms.ComboBox cbBoards;
        private System.Windows.Forms.Label lStored;
        private System.Windows.Forms.Button bReset;
        private System.Windows.Forms.Button bClear;
        private System.Windows.Forms.CheckBox cbSimple;
        private SimpleConfigurableBoard cbSimpleBoard;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Button bCancel;
        private ConfigurableBoard cbBoard;
    }
}
