using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DesktopControls.Controls
{
    /// <summary>
    /// Collector for relevant UI controls in a Windows Forms application.
    /// </summary>
    /// <remarks>
    /// Only elements with accesibility objects are collected.
    /// </remarks>
    public class RelevantControlCollector : IUIRelevantElementCollector
    {
        /// <summary>
        /// IUIRelevantElementCollector: Top-level UI elements container.
        /// </summary>
        public IUIRemoteControlElement BaseInstance { get; set; }
        /// <summary>
        /// IUIRelevantElementCollector: Get first-level UI elements.
        /// </summary>
        /// <param name="instance">
        /// Instance of the UI elements container. Null to use BaseInstance.
        /// </param>
        /// <returns>
        /// List of UI elements. Can be empty or null if there are no UI elements.
        /// </returns>
        public List<UIRelevantElement> GetUIElements(IUIRemoteControlElement instance)
        {
            Control cinstance = (instance ?? BaseInstance) as Control;
            List<UIRelevantElement> elements = null;
            if (cinstance != null)
            {
                IUIRemoteControlElement uiinstance = cinstance as IUIRemoteControlElement;
                foreach (Control ctl in cinstance.Controls)
                {
                    if (ctl.Visible)
                    {
                        if (!string.IsNullOrEmpty(ctl.AccessibleName))
                        {
                            AccessibleObject ao = ctl.AccessibilityObject;
                            if (elements == null)
                            {
                                elements = new List<UIRelevantElement>();
                            }
                            elements.Add(new UIRelevantElement
                            {
                                Path = string.Join(".", uiinstance.UID, ctl.Name),
                                FriendlyName = ao.Name,
                                Description = ao.Description,
                                Role = ao.Role.ToString(),
                                Bounds = new CtlBounds()
                                {
                                    X = ao.Bounds.X,
                                    Y = ao.Bounds.Y,
                                    Width = ao.Bounds.Width,
                                    Height = ao.Bounds.Height
                                },
                                State = ao.State.ToString(),
                                Value = ao.Value,
                                HasChildren = ctl.HasChildren,
                                ElementClass = ctl is UserControl ? RelevantElementClass.UserControl : RelevantElementClass.Control
                            });
                        }
                        else
                        {
                            List<UIRelevantElement> fchilds = GetUIElementChildren(uiinstance, string.Join(".", uiinstance.UID, ctl.Name));
                            if (fchilds != null && fchilds.Count > 0)
                            {
                                if (elements == null)
                                {
                                    elements = new List<UIRelevantElement>();
                                }
                                elements.AddRange(fchilds);
                            }
                        }
                    }
                }
            }
            return elements;
        }
        /// <summary>
        /// IUIRelevantElementCollector: Get all-level UI elements.
        /// </summary>
        /// <param name="instance">
        /// Instance of the UI elements container. Null to use BaseInstance.
        /// </param>
        /// <returns>
        /// List of UI elements with all children included. Can be empty or null if there are no UI elements.
        /// </returns>
        public List<UIRelevantElement> GetAllUIElements(IUIRemoteControlElement instance)
        {
            List<UIRelevantElement> result = new List<UIRelevantElement>();
            List<UIRelevantElement> elements = GetUIElements(instance);
            if (elements != null)
            {
                foreach (UIRelevantElement element in elements)
                {
                    element.State = null;
                    result.Add(element);
                    if (element.HasChildren)
                    {
                        List<UIRelevantElement> childlist = GetAllUIElementChildren(instance, element.Path);
                        if (childlist != null && childlist.Count > 0)
                        {
                            result.AddRange(childlist);
                        }
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// IUIRelevantElementCollector: Get the child UI elements of that on a specific path.
        /// </summary>
        /// <param name="instance">
        /// Instance of the UI elements container. Null to use BaseInstance.
        /// </param>
        /// <param name="path">
        /// Path to the parent UI element.
        /// </param>
        /// <returns>
        /// List of child UI elements. Can be empty or null if there are no child elements.
        /// </returns>
        public List<UIRelevantElement> GetUIElementChildren(IUIRemoteControlElement instance, string path)
        {
            IUIRemoteControlElement uiinstance = instance ?? BaseInstance;
            List<UIRelevantElement> elements = null;
            if (uiinstance != null)
            {
                string[] pathParts = path.Split('.');
                if (pathParts[0] == uiinstance.UID)
                {
                    object element = GetUIElementByPath(uiinstance, path);
                    if (element == null)
                    {
                        return null; // Path not found
                    }
                    if (element is ToolStrip toolStrip)
                    {
                        List<UIRelevantElement> litems = CollectToolStripItems(toolStrip.Items, path);
                        if (litems != null)
                        {
                            if (elements == null)
                            {
                                elements = new List<UIRelevantElement>(litems);
                            }
                        }
                    }
                    else if (element is MenuStrip menuStrip)
                    {
                        List<UIRelevantElement> litems = CollectToolStripItems(menuStrip.Items, path);
                        if (litems != null)
                        {
                            if (elements == null)
                            {
                                elements = new List<UIRelevantElement>(litems);
                            }
                        }
                    }
                    else if (element is ContextMenuStrip contextMenuStrip)
                    {
                        List<UIRelevantElement> litems = CollectToolStripItems(contextMenuStrip.Items, path);
                        if (litems != null)
                        {
                            if (elements == null)
                            {
                                elements = new List<UIRelevantElement>(litems);
                            }
                        }
                    }
                    else if (element is SplitContainer sc)
                    {
                        foreach (Control ctl in sc.Panel1.Controls)
                        {
                            if (ctl.Visible)
                            {
                                if (!string.IsNullOrEmpty(ctl.AccessibleName))
                                {
                                    AccessibleObject ao = ctl.AccessibilityObject;
                                    if (elements == null)
                                    {
                                        elements = new List<UIRelevantElement>();
                                    }
                                    elements.Add(new UIRelevantElement
                                    {
                                        Path = string.Join(".", path, "1", ctl.Name),
                                        FriendlyName = ao.Name,
                                        Description = ao.Description,
                                        Role = ao.Role.ToString(),
                                        Bounds = new CtlBounds()
                                        {
                                            X = ao.Bounds.X,
                                            Y = ao.Bounds.Y,
                                            Width = ao.Bounds.Width,
                                            Height = ao.Bounds.Height
                                        },
                                        State = ao.State.ToString(),
                                        Value = ao.Value,
                                        HasChildren = ctl.HasChildren,
                                        ElementClass = ctl is UserControl ? RelevantElementClass.UserControl : RelevantElementClass.Control
                                    });
                                }
                                else
                                {
                                    List<UIRelevantElement> fchilds = GetUIElementChildren(uiinstance, string.Join(".", path, "1", ctl.Name));
                                    if (fchilds != null && fchilds.Count > 0)
                                    {
                                        if (elements == null)
                                        {
                                            elements = new List<UIRelevantElement>();
                                        }
                                        elements.AddRange(fchilds);
                                    }
                                }
                            }
                        }
                        foreach (Control ctl in sc.Panel2.Controls)
                        {
                            if (ctl.Visible)
                            {
                                if (!string.IsNullOrEmpty(ctl.AccessibleName))
                                {
                                    AccessibleObject ao = ctl.AccessibilityObject;
                                    if (elements == null)
                                    {
                                        elements = new List<UIRelevantElement>();
                                    }
                                    elements.Add(new UIRelevantElement
                                    {
                                        Path = string.Join(".", path, "2", ctl.Name),
                                        FriendlyName = ao.Name,
                                        Description = ao.Description,
                                        Role = ao.Role.ToString(),
                                        Bounds = new CtlBounds()
                                        {
                                            X = ao.Bounds.X,
                                            Y = ao.Bounds.Y,
                                            Width = ao.Bounds.Width,
                                            Height = ao.Bounds.Height,
                                        },
                                        State = ao.State.ToString(),
                                        Value = ao.Value,
                                        HasChildren = ctl.HasChildren,
                                        ElementClass = ctl is UserControl ? RelevantElementClass.UserControl : RelevantElementClass.Control
                                    });
                                }
                                else
                                {
                                    List<UIRelevantElement> fchilds = GetUIElementChildren(uiinstance, string.Join(".", path, "2", ctl.Name));
                                    if (fchilds != null && fchilds.Count > 0)
                                    {
                                        if (elements == null)
                                        {
                                            elements = new List<UIRelevantElement>();
                                        }
                                        elements.AddRange(fchilds);
                                    }
                                }
                            }
                        }
                    }
                    else if (element is Control control)
                    {
                        foreach (Control ctl in control.Controls)
                        {
                            if (ctl.Visible)
                            {
                                if (!string.IsNullOrEmpty(ctl.AccessibleName))
                                {
                                    AccessibleObject ao = ctl.AccessibilityObject;
                                    if (elements == null)
                                    {
                                        elements = new List<UIRelevantElement>();
                                    }
                                    elements.Add(new UIRelevantElement
                                    {
                                        Path = string.Join(".", path, ctl.Name),
                                        FriendlyName = ao.Name,
                                        Description = ao.Description,
                                        Role = ao.Role.ToString(),
                                        Bounds = new CtlBounds()
                                        {
                                            X = ao.Bounds.X,
                                            Y = ao.Bounds.Y,
                                            Width = ao.Bounds.Width,
                                            Height = ao.Bounds.Height
                                        },
                                        State = ao.State.ToString(),
                                        Value = ao.Value,
                                        HasChildren = ctl.HasChildren,
                                        ElementClass = ctl is UserControl ? RelevantElementClass.UserControl : RelevantElementClass.Control
                                    });
                                }
                                else
                                {
                                    List<UIRelevantElement> fchilds = GetUIElementChildren(uiinstance, string.Join(".", path, ctl.Name));
                                    if (fchilds != null && fchilds.Count > 0)
                                    {
                                        if (elements == null)
                                        {
                                            elements = new List<UIRelevantElement>();
                                        }
                                        elements.AddRange(fchilds);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // Unsupported type, return null
                        return null;
                    }
                }
            }
            return elements;
        }
        /// <summary>
        /// IUIRelevantElementCollector: Get the child UI elements of that on a specific path and all its children.
        /// </summary>
        /// <param name="instance">
        /// Instance of the UI elements container. Null to use BaseInstance.
        /// </param>
        /// <param name="path">
        /// Path to the parent UI element.
        /// </param>
        /// <returns>
        /// List of child UI elements. Can be empty or null if there are no child elements.
        /// </returns>
        public List<UIRelevantElement> GetAllUIElementChildren(IUIRemoteControlElement instance, string path)
        {
            List<UIRelevantElement> result = new List<UIRelevantElement>();
            List<UIRelevantElement> elements = GetUIElementChildren(instance, path);
            if (elements != null)
            {
                foreach (UIRelevantElement element in elements)
                {
                    element.State = null;
                    result.Add(element);
                    if (element.HasChildren)
                    {
                        List<UIRelevantElement> childlist = GetAllUIElementChildren(instance, element.Path);
                        if (childlist != null && childlist.Count > 0)
                        {
                            result.AddRange(childlist);
                        }
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// IUIRelevantElementCollector: Retrieve a UI element by its path.
        /// </summary>
        /// <param name="instance">
        /// Root instance to start searching from.
        /// </param>
        /// <param name="path">
        /// Dot-separated path to the control, starting with the root control's name.
        /// </param>
        /// <returns>
        /// Control or ToolStripItem if found, otherwise null.
        /// </returns>
        public object GetUIElementByPath(IUIRemoteControlElement instance, string path)
        {
            Control cinstance = (instance ?? BaseInstance) as Control;
            if (cinstance == null)
            {
                return null;
            }
            IUIRemoteControlElement uiinstance = cinstance as IUIRemoteControlElement;
            var parts = path.Split('.');
            if (parts.Length == 0 || parts[0] != uiinstance.UID)
            {
                return null;
            }

            object current = cinstance;

            for (int i = 1; i < parts.Length; i++)
            {
                string key = parts[i];
                object next = null;

                switch (current)
                {
                    // 1) (ToolStripMenuItem, ToolStripDropDownButton...)
                    case ToolStripDropDownItem dropDownItem:
                        next = dropDownItem.DropDownItems[key];
                        break;

                    // 2) (MenuStrip, ToolStrip, ContextMenuStrip)
                    case ToolStrip toolStrip:
                        next = toolStrip.Items[key];
                        break;
                    // 3) SplitContainer
                    case SplitContainer splitContainer:
                        if (key == "1")
                        {
                            next = splitContainer.Panel1;
                        }
                        else if (key == "2")
                        {
                            next = splitContainer.Panel2;
                        }
                        else
                        {
                            next = null; // Invalid panel key
                        }
                        break;
                    // 4) Regular Controls
                    case Control ctrl:
                        next = ctrl.Controls[key];
                        break;

                    default:
                        return null;  // unsupported type
                }

                if (next == null)
                {
                    return null;    // Does not exist
                }
                current = next;
            }

            return current;
        }
        /// <summary>
        /// Collect ToolStrip items into a list of UIRelevantElement.
        /// </summary>
        /// <param name="items">
        /// List of ToolStrip items to collect.
        /// </param>
        /// <param name="pname">
        /// Path name for the root container of the ToolStrip items.
        /// </param>
        /// <returns>
        /// List of UIRelevantElement representing the ToolStrip items or null.
        /// </returns>
        protected List<UIRelevantElement> CollectToolStripItems(ToolStripItemCollection items, string pname)
        {
            List<UIRelevantElement> elements = null;
            foreach (ToolStripItem item in items)
            {
                if (item.Visible)
                {
                    if (!string.IsNullOrEmpty(item.AccessibleName))
                    {
                        AccessibleObject ao = item.AccessibilityObject;
                        if (elements == null)
                        {
                            elements = new List<UIRelevantElement>();
                        }
                        elements.Add(new UIRelevantElement
                        {
                            Path = string.Join(".", pname, item.Name),
                            FriendlyName = ao.Name,
                            Description = ao.Description,
                            Role = ao.Role.ToString(),
                            Bounds = new CtlBounds()
                            {
                                X = ao.Bounds.X,
                                Y = ao.Bounds.Y,
                                Width = ao.Bounds.Width,
                                Height = ao.Bounds.Height
                            },
                            State = ao.State.ToString(),
                            Value = ao.Value,
                            HasChildren = ao.GetChildCount() > 0,
                            ElementClass = item is ToolStripDropDownItem ? RelevantElementClass.ToolStripDropDownItem : RelevantElementClass.ToolStripItem
                        });
                    }
                    else if (item is ToolStripDropDownItem dditem)
                    {
                        List<UIRelevantElement> fchilds = CollectToolStripItems(dditem.DropDownItems, string.Join(".", pname, item.Name));
                        if (fchilds != null && fchilds.Count > 0)
                        {
                            if (elements == null)
                            {
                                elements = new List<UIRelevantElement>();
                            }
                            elements.AddRange(fchilds);
                        }
                    }
                }
            }
            return elements;
        }
    }
}
