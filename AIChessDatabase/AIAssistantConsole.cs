using AIAssistants.Interfaces;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase
{
    /// <summary>
    /// MDI Child form to display an AI Assistant console.
    /// </summary>
    public partial class AIAssistantConsole : Form, IUIRemoteControlElement
    {
        private IHelpConsole _console;
        private IUIRemoteControlElement _rcconsole;
        public AIAssistantConsole()
        {
            InitializeComponent();
            Icon = ICO_Console;
            Text = TTL_Console;
        }
        /// <summary>
        /// Assistant console control.
        /// </summary>
        [Browsable(false)]
        public IHelpConsole Console
        {
            get
            {
                if (Controls.Count > 0)
                {
                    return Controls[0] as IHelpConsole;
                }
                return _console;
            }
            set
            {
                _console = value;
                _rcconsole = value as IUIRemoteControlElement;
                Controls.Clear();
                if (value is Control)
                {
                    Controls.Add(value as Control);
                }
            }
        }
        /// <summary>
        /// Flag to allow or disallow closing the console.
        /// </summary>
        public bool AllowClose { get; set; }
        /// <summary>
        /// IUIRemoteControlElement: Unique identifier for the UI container.
        /// </summary>
        public string UID { get { return _rcconsole.UID; } }
        /// <summary>
        /// IUIRemoteControlElement: Form friendly name.
        /// </summary>
        public string FriendlyName { get { return Text; } }
        /// <summary>
        /// IUIRemoteControlElement: Get first-level UI elements.
        /// </summary>
        /// <returns>
        /// List of first-level UI elements. Can be empty or null if there are no elements.
        /// </returns>
        public List<UIRelevantElement> GetUIElements()
        {
            return _rcconsole.GetUIElements();
        }
        /// <summary>
        /// IUIRemoteControlElement: Get all-level UI elements.
        /// </summary>
        /// <returns>
        /// List of all UI elements. Can be empty or null if there are no elements.
        /// </returns>
        public List<UIRelevantElement> GetAllUIElements()
        {
            return _rcconsole.GetAllUIElements();
        }
        /// <summary>
        /// IUIRemoteControlElement: Get the child UI elements of that on a specific path.
        /// </summary>
        /// <param name="path">
        /// Path to the parent UI element.
        /// </param>
        /// <returns>
        /// List of child UI elements. Can be empty or null if there are no child elements.
        /// </returns>
        public List<UIRelevantElement> GetUIElementChildren(string path)
        {
            return _rcconsole.GetUIElementChildren(path);
        }
        /// <summary>
        /// IUIRemoteControlElement: Highlight a UI element for a specified number of seconds.
        /// </summary>
        /// <param name="path">
        /// Path to the UI element to highlight.
        /// </param>
        /// <param name="seconds">
        /// Duration in seconds to highlight the element.
        /// </param>
        /// <param name="mode">
        /// Implementation-dependant mode of highlighting.
        /// </param>
        public void HighlightUIElement(string path, int seconds, string mode)
        {
            _rcconsole.HighlightUIElement(path, seconds, mode);
        }
        /// <summary>
        /// IUIRemoteControlElement: Show a tooltip for a UI element with a comment for a specified number of seconds.
        /// </summary>
        /// <param name="path">
        /// Path to the UI element to comment.
        /// </param>
        /// <param name="title">
        /// The title of the notification balloon.
        /// </param>
        /// <param name="comment">
        /// Comment to display in the tooltip.
        /// </param>
        /// <param name="mode">
        /// Implementation-dependant mode of highlighting.
        /// </param>
        /// <param name="seconds">
        /// Seconds to display the tooltip.
        /// </param>
        public void CommentUIElement(string path, string title, string comment, string mode, int seconds)
        {
            _rcconsole.CommentUIElement(path, title, comment, mode, seconds);
        }
        /// <summary>
        /// IUIRemoteControlElement: Invoke an action on a UI element at the specified path.
        /// </summary>
        /// <param name="path">
        /// Path to the UI element to invoke the action on.
        /// </param>
        /// <param name="action">
        /// Action name to invoke on the UI element, such as "click", "double-click", etc.
        /// </param>
        /// <returns>
        /// String value or null
        /// </returns>
        /// <remarks>
        /// The format for action is: action[:param]
        /// </remarks>
        public object InvokeElementAction(string path, string action)
        {
            return _rcconsole.InvokeElementAction(path, action);
        }
        private void AIAssistantConsole_Load(object sender, EventArgs e)
        {
            Icon = Icon.Clone() as Icon;
        }

        private void AIAssistantConsole_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !AllowClose &&
                e.CloseReason != CloseReason.MdiFormClosing &&
                e.CloseReason != CloseReason.WindowsShutDown &&
                e.CloseReason != CloseReason.ApplicationExitCall;
        }

        private void AIAssistantConsole_Shown(object sender, EventArgs e)
        {
            BringToFront();
        }
    }
}
