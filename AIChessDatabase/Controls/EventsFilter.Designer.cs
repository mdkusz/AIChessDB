namespace AIChessDatabase.Controls
{
    partial class EventsFilter
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
            this.bOK = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.cbDraw = new System.Windows.Forms.CheckBox();
            this.cbCastling = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbCheck = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbPromoted = new System.Windows.Forms.GroupBox();
            this.cbBBishopP = new System.Windows.Forms.CheckBox();
            this.cbKnightP = new System.Windows.Forms.CheckBox();
            this.cbRookP = new System.Windows.Forms.CheckBox();
            this.cbQueenP = new System.Windows.Forms.CheckBox();
            this.gbCaptured = new System.Windows.Forms.GroupBox();
            this.cbBBishopC = new System.Windows.Forms.CheckBox();
            this.cbEnPassant = new System.Windows.Forms.CheckBox();
            this.cbKnightC = new System.Windows.Forms.CheckBox();
            this.cbQueenC = new System.Windows.Forms.CheckBox();
            this.cbRookC = new System.Windows.Forms.CheckBox();
            this.cbWBishopC = new System.Windows.Forms.CheckBox();
            this.cbPawnC = new System.Windows.Forms.CheckBox();
            this.gbMove = new System.Windows.Forms.GroupBox();
            this.cbBBishopM = new System.Windows.Forms.CheckBox();
            this.cbKingM = new System.Windows.Forms.CheckBox();
            this.cbKnightM = new System.Windows.Forms.CheckBox();
            this.cbQueenM = new System.Windows.Forms.CheckBox();
            this.cbRookM = new System.Windows.Forms.CheckBox();
            this.cbWBishopM = new System.Windows.Forms.CheckBox();
            this.cbPawnM = new System.Windows.Forms.CheckBox();
            this.cbNever = new System.Windows.Forms.CheckBox();
            this.cbAllTogether = new System.Windows.Forms.CheckBox();
            this.gbPromoted.SuspendLayout();
            this.gbCaptured.SuspendLayout();
            this.gbMove.SuspendLayout();
            this.SuspendLayout();
            // 
            // bOK
            // 
            this.bOK.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bOK_Description;
            this.bOK.AccessibleName = global::AIChessDatabase.Properties.UIElements.bOK_Name;
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.Location = new System.Drawing.Point(162, 800);
            this.bOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(112, 35);
            this.bOK.TabIndex = 21;
            this.bOK.Text = global::AIChessDatabase.Properties.UIResources.BTN_OK;
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // bCancel
            // 
            this.bCancel.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.bCancel_Description;
            this.bCancel.AccessibleName = global::AIChessDatabase.Properties.UIElements.bCancel_Name;
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.Location = new System.Drawing.Point(306, 800);
            this.bCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(112, 35);
            this.bCancel.TabIndex = 20;
            this.bCancel.Text = global::AIChessDatabase.Properties.UIResources.BTN_CANCEL;
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // cbDraw
            // 
            this.cbDraw.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbDraw_Description;
            this.cbDraw.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbDraw_Name;
            this.cbDraw.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbDraw.AutoSize = true;
            this.cbDraw.Location = new System.Drawing.Point(22, 742);
            this.cbDraw.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbDraw.Name = "cbDraw";
            this.cbDraw.Size = new System.Drawing.Size(109, 24);
            this.cbDraw.TabIndex = 19;
            this.cbDraw.Text = global::AIChessDatabase.Properties.UIResources.EVENT_DRAW_OFFER;
            this.cbDraw.UseVisualStyleBackColor = true;
            // 
            // cbCastling
            // 
            this.cbCastling.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbCastling_Description;
            this.cbCastling.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbCastling_Name;
            this.cbCastling.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.cbCastling.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCastling.FormattingEnabled = true;
            this.cbCastling.Items.AddRange(new object[] {
            global::AIChessDatabase.Properties.UIResources.EVENT_CASTLE,
            global::AIChessDatabase.Properties.UIResources.EVENT_CASTLEK,
            global::AIChessDatabase.Properties.UIResources.EVENT_CASTLEQ,
            ""});
            this.cbCastling.Location = new System.Drawing.Point(22, 686);
            this.cbCastling.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbCastling.Name = "cbCastling";
            this.cbCastling.Size = new System.Drawing.Size(250, 28);
            this.cbCastling.TabIndex = 18;
            this.cbCastling.SelectedIndexChanged += new System.EventHandler(this.cbCastling_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 662);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 20);
            this.label2.TabIndex = 17;
            this.label2.Text = "Castling";
            // 
            // cbCheck
            // 
            this.cbCheck.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbCheck_Description;
            this.cbCheck.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbCheck_Name;
            this.cbCheck.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.cbCheck.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCheck.FormattingEnabled = true;
            this.cbCheck.Items.AddRange(new object[] {
            global::AIChessDatabase.Properties.UIResources.EVENT_CHECK,
            global::AIChessDatabase.Properties.UIResources.EVENT_CHECK_MATE,
            global::AIChessDatabase.Properties.UIResources.EVENT_DISCOVERED_CHECK,
            global::AIChessDatabase.Properties.UIResources.EVENT_MULTIPLE_CHECK,
            ""});
            this.cbCheck.Location = new System.Drawing.Point(22, 605);
            this.cbCheck.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbCheck.Name = "cbCheck";
            this.cbCheck.Size = new System.Drawing.Size(250, 28);
            this.cbCheck.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 581);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 20);
            this.label1.TabIndex = 15;
            this.label1.Text = "Check";
            // 
            // gbPromoted
            // 
            this.gbPromoted.Controls.Add(this.cbBBishopP);
            this.gbPromoted.Controls.Add(this.cbKnightP);
            this.gbPromoted.Controls.Add(this.cbRookP);
            this.gbPromoted.Controls.Add(this.cbQueenP);
            this.gbPromoted.Location = new System.Drawing.Point(22, 401);
            this.gbPromoted.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbPromoted.Name = "gbPromoted";
            this.gbPromoted.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbPromoted.Size = new System.Drawing.Size(396, 159);
            this.gbPromoted.TabIndex = 14;
            this.gbPromoted.TabStop = false;
            this.gbPromoted.Text = "Pawn Promoted to";
            // 
            // cbBBishopP
            // 
            this.cbBBishopP.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbBBishopP_Description;
            this.cbBBishopP.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbBBishopP_Name;
            this.cbBBishopP.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbBBishopP.AutoSize = true;
            this.cbBBishopP.Location = new System.Drawing.Point(118, 48);
            this.cbBBishopP.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbBBishopP.Name = "cbBBishopP";
            this.cbBBishopP.Size = new System.Drawing.Size(84, 24);
            this.cbBBishopP.TabIndex = 5;
            this.cbBBishopP.Text = global::AIChessDatabase.Properties.UIResources.EVENT_BISHOP;
            this.cbBBishopP.UseVisualStyleBackColor = true;
            this.cbBBishopP.CheckedChanged += new System.EventHandler(this.cbBBishopP_CheckedChanged);
            // 
            // cbKnightP
            // 
            this.cbKnightP.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbKnightP_Description;
            this.cbKnightP.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbKnightP_Name;
            this.cbKnightP.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbKnightP.AutoSize = true;
            this.cbKnightP.Location = new System.Drawing.Point(267, 48);
            this.cbKnightP.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbKnightP.Name = "cbKnightP";
            this.cbKnightP.Size = new System.Drawing.Size(80, 24);
            this.cbKnightP.TabIndex = 4;
            this.cbKnightP.Text = global::AIChessDatabase.Properties.UIResources.EVENT_KNIGHT1;
            this.cbKnightP.UseVisualStyleBackColor = true;
            this.cbKnightP.CheckedChanged += new System.EventHandler(this.cbKnightP_CheckedChanged);
            // 
            // cbRookP
            // 
            this.cbRookP.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbRookP_Description;
            this.cbRookP.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbRookP_Name;
            this.cbRookP.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbRookP.AutoSize = true;
            this.cbRookP.Location = new System.Drawing.Point(9, 102);
            this.cbRookP.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbRookP.Name = "cbRookP";
            this.cbRookP.Size = new System.Drawing.Size(73, 24);
            this.cbRookP.TabIndex = 2;
            this.cbRookP.Text = global::AIChessDatabase.Properties.UIResources.EVENT_ROOK1;
            this.cbRookP.UseVisualStyleBackColor = true;
            this.cbRookP.CheckedChanged += new System.EventHandler(this.cbRookP_CheckedChanged);
            // 
            // cbQueenP
            // 
            this.cbQueenP.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbQueenP_Description;
            this.cbQueenP.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbQueenP_Name;
            this.cbQueenP.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbQueenP.AutoSize = true;
            this.cbQueenP.Location = new System.Drawing.Point(9, 48);
            this.cbQueenP.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbQueenP.Name = "cbQueenP";
            this.cbQueenP.Size = new System.Drawing.Size(83, 24);
            this.cbQueenP.TabIndex = 0;
            this.cbQueenP.Text = global::AIChessDatabase.Properties.UIResources.EVENT_QUEEN1;
            this.cbQueenP.UseVisualStyleBackColor = true;
            this.cbQueenP.CheckedChanged += new System.EventHandler(this.cbQueenP_CheckedChanged);
            // 
            // gbCaptured
            // 
            this.gbCaptured.Controls.Add(this.cbBBishopC);
            this.gbCaptured.Controls.Add(this.cbEnPassant);
            this.gbCaptured.Controls.Add(this.cbKnightC);
            this.gbCaptured.Controls.Add(this.cbQueenC);
            this.gbCaptured.Controls.Add(this.cbRookC);
            this.gbCaptured.Controls.Add(this.cbWBishopC);
            this.gbCaptured.Controls.Add(this.cbPawnC);
            this.gbCaptured.Location = new System.Drawing.Point(22, 209);
            this.gbCaptured.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbCaptured.Name = "gbCaptured";
            this.gbCaptured.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbCaptured.Size = new System.Drawing.Size(396, 159);
            this.gbCaptured.TabIndex = 13;
            this.gbCaptured.TabStop = false;
            this.gbCaptured.Text = "Piece Captured";
            // 
            // cbBBishopC
            // 
            this.cbBBishopC.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbBBishopC_Description;
            this.cbBBishopC.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbBBishopC_Name;
            this.cbBBishopC.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbBBishopC.AutoSize = true;
            this.cbBBishopC.Location = new System.Drawing.Point(118, 65);
            this.cbBBishopC.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbBBishopC.Name = "cbBBishopC";
            this.cbBBishopC.Size = new System.Drawing.Size(122, 24);
            this.cbBBishopC.TabIndex = 6;
            this.cbBBishopC.Text = global::AIChessDatabase.Properties.UIResources.EVENT_BBISHOP2;
            this.cbBBishopC.UseVisualStyleBackColor = true;
            this.cbBBishopC.CheckedChanged += new System.EventHandler(this.cbBBishopC_CheckedChanged);
            // 
            // cbEnPassant
            // 
            this.cbEnPassant.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbEnPassant_Description;
            this.cbEnPassant.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbEnPassant_Name;
            this.cbEnPassant.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbEnPassant.AutoSize = true;
            this.cbEnPassant.Enabled = false;
            this.cbEnPassant.Location = new System.Drawing.Point(267, 102);
            this.cbEnPassant.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbEnPassant.Name = "cbEnPassant";
            this.cbEnPassant.Size = new System.Drawing.Size(116, 24);
            this.cbEnPassant.TabIndex = 5;
            this.cbEnPassant.Text = global::AIChessDatabase.Properties.UIResources.EVENT_PAWN_PASSANT;
            this.cbEnPassant.UseVisualStyleBackColor = true;
            // 
            // cbKnightC
            // 
            this.cbKnightC.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbKnightC_Description;
            this.cbKnightC.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbKnightC_Name;
            this.cbKnightC.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbKnightC.AutoSize = true;
            this.cbKnightC.Location = new System.Drawing.Point(267, 48);
            this.cbKnightC.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbKnightC.Name = "cbKnightC";
            this.cbKnightC.Size = new System.Drawing.Size(80, 24);
            this.cbKnightC.TabIndex = 4;
            this.cbKnightC.Text = global::AIChessDatabase.Properties.UIResources.EVENT_KNIGHT2;
            this.cbKnightC.UseVisualStyleBackColor = true;
            this.cbKnightC.CheckedChanged += new System.EventHandler(this.cbKnightC_CheckedChanged);
            // 
            // cbQueenC
            // 
            this.cbQueenC.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbQueenC_Description;
            this.cbQueenC.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbQueenC_Name;
            this.cbQueenC.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbQueenC.AutoSize = true;
            this.cbQueenC.Location = new System.Drawing.Point(138, 102);
            this.cbQueenC.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbQueenC.Name = "cbQueenC";
            this.cbQueenC.Size = new System.Drawing.Size(83, 24);
            this.cbQueenC.TabIndex = 3;
            this.cbQueenC.Text = global::AIChessDatabase.Properties.UIResources.EVENT_QUEEN2;
            this.cbQueenC.UseVisualStyleBackColor = true;
            this.cbQueenC.CheckedChanged += new System.EventHandler(this.cbQueenC_CheckedChanged);
            // 
            // cbRookC
            // 
            this.cbRookC.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbRookC_Description;
            this.cbRookC.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbRookC_Name;
            this.cbRookC.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbRookC.AutoSize = true;
            this.cbRookC.Location = new System.Drawing.Point(9, 102);
            this.cbRookC.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbRookC.Name = "cbRookC";
            this.cbRookC.Size = new System.Drawing.Size(73, 24);
            this.cbRookC.TabIndex = 2;
            this.cbRookC.Text = global::AIChessDatabase.Properties.UIResources.EVENT_ROOK2;
            this.cbRookC.UseVisualStyleBackColor = true;
            this.cbRookC.CheckedChanged += new System.EventHandler(this.cbRookC_CheckedChanged);
            // 
            // cbWBishopC
            // 
            this.cbWBishopC.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbWBishopC_Description;
            this.cbWBishopC.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbWBishopC_Name;
            this.cbWBishopC.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbWBishopC.AutoSize = true;
            this.cbWBishopC.Location = new System.Drawing.Point(118, 29);
            this.cbWBishopC.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbWBishopC.Name = "cbWBishopC";
            this.cbWBishopC.Size = new System.Drawing.Size(123, 24);
            this.cbWBishopC.TabIndex = 1;
            this.cbWBishopC.Text = global::AIChessDatabase.Properties.UIResources.EVENT_WBISHOP2;
            this.cbWBishopC.UseVisualStyleBackColor = true;
            this.cbWBishopC.CheckedChanged += new System.EventHandler(this.cbWBishopC_CheckedChanged);
            // 
            // cbPawnC
            // 
            this.cbPawnC.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbPawnC_Description;
            this.cbPawnC.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbPawnC_Name;
            this.cbPawnC.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbPawnC.AutoSize = true;
            this.cbPawnC.Location = new System.Drawing.Point(9, 48);
            this.cbPawnC.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbPawnC.Name = "cbPawnC";
            this.cbPawnC.Size = new System.Drawing.Size(74, 24);
            this.cbPawnC.TabIndex = 0;
            this.cbPawnC.Text = global::AIChessDatabase.Properties.UIResources.EVENT_PAWN2;
            this.cbPawnC.UseVisualStyleBackColor = true;
            this.cbPawnC.CheckedChanged += new System.EventHandler(this.cbPawnC_CheckedChanged);
            // 
            // gbMove
            // 
            this.gbMove.Controls.Add(this.cbBBishopM);
            this.gbMove.Controls.Add(this.cbKingM);
            this.gbMove.Controls.Add(this.cbKnightM);
            this.gbMove.Controls.Add(this.cbQueenM);
            this.gbMove.Controls.Add(this.cbRookM);
            this.gbMove.Controls.Add(this.cbWBishopM);
            this.gbMove.Controls.Add(this.cbPawnM);
            this.gbMove.Location = new System.Drawing.Point(24, 21);
            this.gbMove.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbMove.Name = "gbMove";
            this.gbMove.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbMove.Size = new System.Drawing.Size(396, 159);
            this.gbMove.TabIndex = 12;
            this.gbMove.TabStop = false;
            this.gbMove.Text = "Piece to Move";
            // 
            // cbBBishopM
            // 
            this.cbBBishopM.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbBBishopM_Description;
            this.cbBBishopM.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbBBishopM_Name;
            this.cbBBishopM.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbBBishopM.AutoSize = true;
            this.cbBBishopM.Location = new System.Drawing.Point(117, 65);
            this.cbBBishopM.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbBBishopM.Name = "cbBBishopM";
            this.cbBBishopM.Size = new System.Drawing.Size(122, 24);
            this.cbBBishopM.TabIndex = 6;
            this.cbBBishopM.Text = global::AIChessDatabase.Properties.UIResources.EVENT_BBISHOP1;
            this.cbBBishopM.UseVisualStyleBackColor = true;
            this.cbBBishopM.CheckedChanged += new System.EventHandler(this.cbBBishopM_CheckedChanged);
            // 
            // cbKingM
            // 
            this.cbKingM.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbKingM_Description;
            this.cbKingM.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbKingM_Name;
            this.cbKingM.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbKingM.AutoSize = true;
            this.cbKingM.Location = new System.Drawing.Point(267, 102);
            this.cbKingM.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbKingM.Name = "cbKingM";
            this.cbKingM.Size = new System.Drawing.Size(66, 24);
            this.cbKingM.TabIndex = 5;
            this.cbKingM.Text = global::AIChessDatabase.Properties.UIResources.EVENT_KING1;
            this.cbKingM.UseVisualStyleBackColor = true;
            this.cbKingM.CheckedChanged += new System.EventHandler(this.cbKingM_CheckedChanged);
            // 
            // cbKnightM
            // 
            this.cbKnightM.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbKnightM_Description;
            this.cbKnightM.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbKnightM_Name;
            this.cbKnightM.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbKnightM.AutoSize = true;
            this.cbKnightM.Location = new System.Drawing.Point(267, 48);
            this.cbKnightM.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbKnightM.Name = "cbKnightM";
            this.cbKnightM.Size = new System.Drawing.Size(80, 24);
            this.cbKnightM.TabIndex = 4;
            this.cbKnightM.Text = global::AIChessDatabase.Properties.UIResources.EVENT_KNIGHT1;
            this.cbKnightM.UseVisualStyleBackColor = true;
            this.cbKnightM.CheckedChanged += new System.EventHandler(this.cbKnightM_CheckedChanged);
            // 
            // cbQueenM
            // 
            this.cbQueenM.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbQueenM_Description;
            this.cbQueenM.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbQueenM_Name;
            this.cbQueenM.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbQueenM.AutoSize = true;
            this.cbQueenM.Location = new System.Drawing.Point(138, 102);
            this.cbQueenM.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbQueenM.Name = "cbQueenM";
            this.cbQueenM.Size = new System.Drawing.Size(83, 24);
            this.cbQueenM.TabIndex = 3;
            this.cbQueenM.Text = global::AIChessDatabase.Properties.UIResources.EVENT_QUEEN1;
            this.cbQueenM.UseVisualStyleBackColor = true;
            this.cbQueenM.CheckedChanged += new System.EventHandler(this.cbQueenM_CheckedChanged);
            // 
            // cbRookM
            // 
            this.cbRookM.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbRookM_Description;
            this.cbRookM.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbRookM_Name;
            this.cbRookM.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbRookM.AutoSize = true;
            this.cbRookM.Location = new System.Drawing.Point(9, 102);
            this.cbRookM.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbRookM.Name = "cbRookM";
            this.cbRookM.Size = new System.Drawing.Size(73, 24);
            this.cbRookM.TabIndex = 2;
            this.cbRookM.Text = global::AIChessDatabase.Properties.UIResources.EVENT_ROOK1;
            this.cbRookM.UseVisualStyleBackColor = true;
            this.cbRookM.CheckedChanged += new System.EventHandler(this.cbRookM_CheckedChanged);
            // 
            // cbWBishopM
            // 
            this.cbWBishopM.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbWBishopM_Description;
            this.cbWBishopM.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbWBishopM_Name;
            this.cbWBishopM.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbWBishopM.AutoSize = true;
            this.cbWBishopM.Location = new System.Drawing.Point(117, 29);
            this.cbWBishopM.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbWBishopM.Name = "cbWBishopM";
            this.cbWBishopM.Size = new System.Drawing.Size(123, 24);
            this.cbWBishopM.TabIndex = 1;
            this.cbWBishopM.Text = global::AIChessDatabase.Properties.UIResources.EVENT_WBISHOP1;
            this.cbWBishopM.UseVisualStyleBackColor = true;
            this.cbWBishopM.CheckedChanged += new System.EventHandler(this.cbWBishopM_CheckedChanged);
            // 
            // cbPawnM
            // 
            this.cbPawnM.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbPawnM_Description;
            this.cbPawnM.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbPawnM_Name;
            this.cbPawnM.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbPawnM.AutoSize = true;
            this.cbPawnM.Location = new System.Drawing.Point(9, 48);
            this.cbPawnM.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbPawnM.Name = "cbPawnM";
            this.cbPawnM.Size = new System.Drawing.Size(74, 24);
            this.cbPawnM.TabIndex = 0;
            this.cbPawnM.Text = global::AIChessDatabase.Properties.UIResources.EVENT_PAWN1;
            this.cbPawnM.UseVisualStyleBackColor = true;
            this.cbPawnM.CheckedChanged += new System.EventHandler(this.cbPawnM_CheckedChanged);
            // 
            // cbNever
            // 
            this.cbNever.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbNever_Description;
            this.cbNever.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbNever_Name;
            this.cbNever.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbNever.AutoSize = true;
            this.cbNever.Location = new System.Drawing.Point(289, 742);
            this.cbNever.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbNever.Name = "cbNever";
            this.cbNever.Size = new System.Drawing.Size(130, 24);
            this.cbNever.TabIndex = 22;
            this.cbNever.Text = global::AIChessDatabase.Properties.UIResources.CB_NEVER;
            this.cbNever.UseVisualStyleBackColor = true;
            this.cbNever.Visible = false;
            // 
            // cbAllTogether
            // 
            this.cbAllTogether.AccessibleDescription = global::AIChessDatabase.Properties.UIElements.cbAllTogether_Description;
            this.cbAllTogether.AccessibleName = global::AIChessDatabase.Properties.UIElements.cbAllTogether_Name;
            this.cbAllTogether.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbAllTogether.AutoSize = true;
            this.cbAllTogether.Location = new System.Drawing.Point(162, 742);
            this.cbAllTogether.Name = "cbAllTogether";
            this.cbAllTogether.Size = new System.Drawing.Size(120, 24);
            this.cbAllTogether.TabIndex = 23;
            this.cbAllTogether.Text = "All Together";
            this.cbAllTogether.UseVisualStyleBackColor = true;
            this.cbAllTogether.CheckedChanged += new System.EventHandler(this.cbAllTogether_CheckedChanged);
            // 
            // EventsFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbAllTogether);
            this.Controls.Add(this.cbNever);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.cbDraw);
            this.Controls.Add(this.cbCastling);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbCheck);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gbPromoted);
            this.Controls.Add(this.gbCaptured);
            this.Controls.Add(this.gbMove);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(448, 868);
            this.Name = "EventsFilter";
            this.Size = new System.Drawing.Size(448, 868);
            this.gbPromoted.ResumeLayout(false);
            this.gbPromoted.PerformLayout();
            this.gbCaptured.ResumeLayout(false);
            this.gbCaptured.PerformLayout();
            this.gbMove.ResumeLayout(false);
            this.gbMove.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.CheckBox cbDraw;
        private System.Windows.Forms.ComboBox cbCastling;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbCheck;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbPromoted;
        private System.Windows.Forms.CheckBox cbBBishopP;
        private System.Windows.Forms.CheckBox cbKnightP;
        private System.Windows.Forms.CheckBox cbRookP;
        private System.Windows.Forms.CheckBox cbQueenP;
        private System.Windows.Forms.GroupBox gbCaptured;
        private System.Windows.Forms.CheckBox cbBBishopC;
        private System.Windows.Forms.CheckBox cbEnPassant;
        private System.Windows.Forms.CheckBox cbKnightC;
        private System.Windows.Forms.CheckBox cbQueenC;
        private System.Windows.Forms.CheckBox cbRookC;
        private System.Windows.Forms.CheckBox cbWBishopC;
        private System.Windows.Forms.CheckBox cbPawnC;
        private System.Windows.Forms.GroupBox gbMove;
        private System.Windows.Forms.CheckBox cbBBishopM;
        private System.Windows.Forms.CheckBox cbKingM;
        private System.Windows.Forms.CheckBox cbKnightM;
        private System.Windows.Forms.CheckBox cbQueenM;
        private System.Windows.Forms.CheckBox cbRookM;
        private System.Windows.Forms.CheckBox cbWBishopM;
        private System.Windows.Forms.CheckBox cbPawnM;
        private System.Windows.Forms.CheckBox cbNever;
        private System.Windows.Forms.CheckBox cbAllTogether;
    }
}
