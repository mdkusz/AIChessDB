namespace AIChessDatabase
{
    partial class AIAssistantConsole
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
            this.SuspendLayout();
            // 
            // AIAssistantConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(999, 720);
            this.Location = new System.Drawing.Point(3000, 3000);
            this.Name = "AIAssistantConsole";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "AIAssistantConsole";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AIAssistantConsole_FormClosing);
            this.Load += new System.EventHandler(this.AIAssistantConsole_Load);
            this.Shown += new System.EventHandler(this.AIAssistantConsole_Shown);
            this.ResumeLayout(false);

        }

        #endregion
    }
}