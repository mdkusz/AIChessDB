using System;
using System.Diagnostics;
using System.Windows.Forms;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Dialogs
{
    /// <summary>
    /// Show application credits and check for proper installation and configuration
    /// </summary>
    public partial class DlgCredits : Form
    {
        public DlgCredits()
        {
            InitializeComponent();
            Text = TTL_Credits;
            lAuthor.Text = LAB_AUTHOR;
            lIcon.Text = LAB_APPICON;
            lPieces.Text = LAB_PIECESET;
        }
        private void llPieces_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = @"https://commons.wikimedia.org/wiki/File:ChessPiecesArray.png";
                p.Start();
            }
            catch
            {
            }
        }

        private void llLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = @"https://creativecommons.org/licenses/by-sa/3.0";
                p.Start();
            }
            catch
            {
            }
        }

        private void llIcon_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = @"https://iconos8.es/icons/set/chessboard";
                p.Start();
            }
            catch
            {
            }
        }

        private void llIcons8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = @"https://iconos8.es";
                p.Start();
            }
            catch
            {
            }
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            if (Modal)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                Close();
            }
        }

        private void DlgCredits_Load(object sender, EventArgs e)
        {

        }

        private void llAuthor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = URL_STL;
                p.Start();
            }
            catch
            {
            }
        }
    }
}
