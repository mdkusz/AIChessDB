using GlobalCommonEntities.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace DesktopControls.Controls
{
    /// <summary>
    /// Implementation of IUIElementInteractor for Windows Forms controls.
    /// </summary>
    public class ControlInteractor : IUIElementInteractor
    {
        private static fHighlight _wHL = new fHighlight();
        /// <summary>
        /// IUIElementInteractor: Element collector to resolve path to elements.
        /// </summary>
        public IUIRelevantElementCollector ElementCollector { get; set; }
        /// <summary>
        /// IUIElementInteractor: Highlight the element at the specified path for a given number of seconds.
        /// </summary>
        /// <param name="path">
        /// Path to the UI element to highlight.
        /// </param>
        /// <param name="seconds">
        /// Seconds to highlight the element.
        /// </param>
        /// <param name="mode">
        /// Implementation-dependant mode of highlighting.
        /// </param>
        public void HighLight(string path, int seconds, string mode)
        {
            object element = ElementCollector.GetUIElementByPath(null, path);
            if (element != null)
            {
                if (!Enum.TryParse(mode, out fHighlight.HighlightMode hmode))
                {
                    hmode = fHighlight.HighlightMode.Flash;
                }
                _wHL.AddHighlightRequest(new HighlightRequest()
                {
                    CloseInMs = seconds * 1000,
                    Mode = hmode,
                    CtlToHighlight = element
                });
            }
        }
        /// <summary>
        /// IUIElementInteractor: Displays a notification balloon with the specified title and message for a given duration.
        /// </summary>
        /// <param name="path">
        /// Path to the UI element to comment.
        /// </param>
        /// <param name="title">
        /// The title of the notification balloon.
        /// </param>
        /// <param name="message">
        /// The message content of the notification balloon.
        /// </param>
        /// <param name="mode">
        /// Implementation-dependant mode of highlighting.
        /// </param>
        /// <param name="seconds">
        /// The duration, in seconds, for which the notification balloon is displayed.
        /// </param>
        public void ShowBalloon(string path, string title, string message, string mode, int seconds)
        {
            object element = ElementCollector.GetUIElementByPath(null, path);
            if (element != null)
            {
                if (!Enum.TryParse(mode, out fHighlight.HighlightMode hmode))
                {
                    hmode = fHighlight.HighlightMode.Tooltip;
                }
                _wHL.AddHighlightRequest(new HighlightRequest()
                {
                    CloseInMs = seconds * 1000,
                    Mode = fHighlight.HighlightMode.Tooltip | hmode,
                    ToolTipCaption = title,
                    ToolTipMessage = message,
                    CtlToHighlight = element
                });
            }
        }
        /// <summary>
        /// Invoke an action on a UI element at the specified path.
        /// </summary>
        /// <param name="path">
        /// Path to the UI element to invoke the action on.
        /// </param>
        /// <param name="action">
        /// Action name to invoke on the UI element, such as "click", "double-click", etc.
        /// </param>
        /// <returns>
        /// Value or null
        /// </returns>
        /// <remarks>
        /// The format for action is: action[:param]
        /// </remarks>
        public object Invoke(string path, string action)
        {
            try
            {
                object element = ElementCollector.GetUIElementByPath(null, path);
                if (element != null)
                {
                    PropertyInfo pi = null;
                    string[] actparts = action.Split(':');
                    switch (actparts[0])
                    {
                        case "click":
                            MethodInfo mi = element.GetType().GetMethod("PerformClick");
                            if (mi != null)
                            {
                                mi.Invoke(element, null);
                            }
                            else if ((mi = element.GetType().GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)) != null)
                            {
                                mi.Invoke(element, new object[] { EventArgs.Empty });
                            }
                            break;
                        case "write":
                            if ((actparts.Length > 1) &&
                                ((pi = element.GetType().GetProperty("Text")) != null) &&
                                pi.CanWrite)
                            {
                                List<string> text = new List<string>(actparts);
                                text.RemoveAt(0);
                                pi.SetValue(element, string.Join(":", text));
                            }
                            break;
                        case "read":
                            if (((pi = element.GetType().GetProperty("Text")) != null) &&
                                pi.CanRead)
                            {
                                return pi.GetValue(element)?.ToString();
                            }
                            break;
                        case "expand":
                            if ((pi = element.GetType().GetProperty("DroppedDown")) != null)
                            {
                                pi.SetValue(element, true);
                            }
                            else if (element is ToolStripDropDownItem tsddi)
                            {
                                tsddi.ShowDropDown();
                            }
                            else if (element is TreeView tv)
                            {
                                tv.SelectedNode?.Expand();
                            }
                            break;
                        case "items":
                            if ((pi = element.GetType().GetProperty("Items")) != null)
                            {
                                var itemsObj = pi.GetValue(element);
                                if (itemsObj is IEnumerable enumerable)
                                {
                                    List<string> result = new List<string>();
                                    foreach (var item in enumerable)
                                    {
                                        switch (item)
                                        {
                                            case ListViewItem lvi:
                                                result.Add(lvi.Text);
                                                break;
                                            case ToolStripItem tsi:
                                                result.Add(tsi.Text);
                                                break;
                                            default:
                                                // If the control exposes GetItemText, then use it
                                                mi = element.GetType().GetMethod("GetItemText");
                                                if (mi != null)
                                                    result.Add(mi.Invoke(element, new object[] { item })?.ToString() ?? "");
                                                else
                                                    result.Add(item?.ToString() ?? "");
                                                break;
                                        }
                                    }
                                    return result;
                                }
                            }
                            else if (element is ToolStripDropDownItem tsddi)
                            {
                                List<string> result = new List<string>();
                                foreach (ToolStripItem subItem in tsddi.DropDownItems)
                                {
                                    result.Add(subItem.Text);
                                }
                                return result;
                            }
                            else if (element is TreeView tv)
                            {
                                if (tv.SelectedNode != null)
                                {
                                    List<string> result = new List<string>();
                                    result = new List<string>();
                                    foreach (TreeNode child in tv.SelectedNode.Nodes)
                                    {
                                        result.Add(child.Text);
                                    }
                                    return result;
                                }
                            }
                            break;
                        case "select":
                            if (actparts.Length > 1)
                            {
                                int ix = int.Parse(actparts[1].Trim());
                                if ((pi = element.GetType().GetProperty("SelectedIndex")) != null)
                                {
                                    pi.SetValue(element, ix);
                                }
                                else if (element is ToolStripDropDownItem tsddi)
                                {
                                    var item = tsddi.DropDownItems[ix];
                                    if (item is ToolStripDropDownItem sub)
                                    {
                                        sub.ShowDropDown();
                                    }
                                }
                                else if (element is TreeView tv)
                                {
                                    tv.SelectedNode = tv.SelectedNode?.Nodes[ix];
                                }
                            }
                            break;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
