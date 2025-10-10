using DesktopControls.Controls;
using DesktopControls.Tools;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using static DesktopControls.Properties.UIResources;

namespace DesktopControls.Dialogs
{
    /// <summary>
    /// Generic dialog box to select objects using the ISelectionObjectProvider interface.
    /// </summary>
    /// <remarks>
    /// This dialog box shows a list of objects that can be selected or removed from the selection.
    /// It can be used with the ObjectSelectorInputEditor to select objects from a list.
    /// The SelectionTarget propety contains the interface with the consumer / provider object lits.
    /// The ISelectionObjectProvider object will provide a list of objects. 
    /// Decorate the object properties you want to show in the list columns with the Browsable(true) attribute.
    /// Decorate the shown object properties with the DisplayName attribute to provide a friendly name instead of the property name for the list columns.
    /// The dialog box has a toolbar with the following buttons:
    /// Refresh: Refresh the object list.
    /// Add Mode: Select this mode to add the selected objects.
    /// Delete Mode: Select this mode to remove the selected objects.
    /// Proceed: Add or remove the selected objects.
    /// RemoveObject: Remove the object container. This is useful to set empty list properties to null.
    /// </remarks>
    /// <seealso cref="ISelectionObjectProvider"/>
    public partial class ObjectSelectionDialog : Form, IUIRemoteControlElement
    {
        private RelevantControlCollector _collector = null;
        private ControlInteractor _interactor = null;
        private PropertyEditorInfo _pInfo = null;
        private ISelectionObjectProvider _selector = null;
        private int _maxSelection = 1;

        public ObjectSelectionDialog()
        {
            InitializeComponent();
            bRefresh.ToolTipText = TTP_Refresh;
            bAddMode.ToolTipText = TTP_AddMode;
            bDeleteMode.ToolTipText = TTP_RemoveMode;
            bProceed.ToolTipText = TTP_Proceed;
            bRemoveObject.ToolTipText = TTP_RemoveContainer;
            _collector = new RelevantControlCollector() { BaseInstance = this };
            _interactor = new ControlInteractor() { ElementCollector = _collector };
        }
        /// <summary>
        /// Allow user to remove the object container.
        /// </summary>
        public bool AllowRemoveContainer
        {
            get
            {
                return bRemoveObject.Visible;
            }
            set
            {
                bRemoveObject.Visible = value;
            }
        }
        /// <summary>
        /// Interface to interact with the object container.
        /// </summary>
        [Browsable(false)]
        public ISelectionObjectProvider SelectionTarget
        {
            get
            {
                return _selector;
            }
            set
            {
                _selector = value;
                Text = _selector?.Title ?? "?";
            }
        }
        /// <summary>
        /// Information about the property editor.
        /// </summary>
        [Browsable(false)]
        public PropertyEditorInfo EditorInfo
        {
            get
            {
                return _pInfo;
            }
            set
            {
                _pInfo = value;
                if (_pInfo != null)
                {
                    if (_pInfo.Values != null)
                    {
                        _maxSelection = Convert.ToInt32(_pInfo.Values[0]);
                        lvSelection.MultiSelect = _maxSelection > 1;
                    }
                    else
                    {
                        lvSelection.MultiSelect = false;
                    }
                }
            }
        }
        /// <summary>
        /// IUIRemoteControlElement: Unique identifier for the UI container.
        /// </summary>
        public string UID { get; } = Guid.NewGuid().ToString("N");
        /// <summary>
        /// IUIRemoteControlElement: Form friendly name.
        /// </summary>
        public string FriendlyName { get { return FNAME_ObjectSelector; } }
        /// <summary>
        /// IUIRemoteControlElement: Get first-level UI elements.
        /// </summary>
        /// <returns>
        /// List of first-level UI elements. Can be empty or null if there are no elements.
        /// </returns>
        public List<UIRelevantElement> GetUIElements()
        {
            return _collector.GetUIElements(null);
        }
        /// <summary>
        /// IUIRemoteControlElement: Get all-level UI elements.
        /// </summary>
        /// <returns>
        /// List of all UI elements. Can be empty or null if there are no elements.
        /// </returns>
        public List<UIRelevantElement> GetAllUIElements()
        {
            return _collector.GetAllUIElements(null);
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
            return _collector.GetUIElementChildren(null, path);
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
            _interactor.HighLight(path, seconds, mode);
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
            _interactor.ShowBalloon(path, title, comment, mode, seconds);
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
            return _interactor.Invoke(path, action);
        }
        private async void bRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                List<object> list = null;
                if (bAddMode.Checked)
                {
                    list = await SelectionTarget.GetSelectionObjects(EditorInfo);
                }
                else if (bDeleteMode.Checked)
                {
                    list = await SelectionTarget.GetSelectedObjects(EditorInfo);
                }
                ControlHelpers.PopulateListView(lvSelection, list);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CAP_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bAddMode_Click(object sender, EventArgs e)
        {
            bDeleteMode.Checked = !bAddMode.Checked;
            lvSelection.Items.Clear();
        }

        private void bDeleteMode_Click(object sender, EventArgs e)
        {
            bAddMode.Checked = !bDeleteMode.Checked;
            lvSelection.Items.Clear();
        }

        private async void bProceed_Click(object sender, EventArgs e)
        {
            try
            {
                List<object> selection = new List<object>();
                foreach (ListViewItem item in lvSelection.SelectedItems)
                {
                    selection.Add(item.Tag);
                }
                List<string> errors = null;
                if (bAddMode.Checked)
                {
                    errors = await SelectionTarget.SetSelection(selection, EditorInfo);
                }
                else if (bDeleteMode.Checked)
                {
                    errors = await SelectionTarget.RemoveSelection(selection, EditorInfo);
                }
                lvSelection.Items.Clear();
                if (errors != null && errors.Count > 0)
                {
                    throw new Exception(string.Join("\n", errors));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CAP_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lvSelection_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                if (lvSelection.SelectedItems.Count > _maxSelection)
                {
                    e.Item.Selected = false;
                }
            }
            bProceed.Enabled = lvSelection.SelectedItems.Count > 0;
        }

        private async void bRemoveObject_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(string.Format(MSG_RemovePropertyObject, SelectionTarget.Title), CAP_Warning, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    string error = await SelectionTarget.RemoveContainer(EditorInfo);
                    if (!string.IsNullOrEmpty(error))
                    {
                        throw new Exception(error);
                    }
                    lvSelection.Items.Clear();
                    if (Modal)
                    {
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CAP_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
