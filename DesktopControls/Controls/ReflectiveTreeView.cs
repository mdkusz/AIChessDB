using GlobalCommonEntities.Interfaces;
using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using static DesktopControls.Properties.UIResources;

namespace DesktopControls.Controls
{
    /// <summary>
    /// Represent any object in a tree view
    /// </summary>
    /// <remarks>
    /// Most classes in ths solution represent complex Json objects. 
    /// This control is used to show them in a tree view using Reflection to show all properties and values in a friendly way.
    /// Use attributes like DisplayName to force the name of the nodes and Description to show ToolTips for the nodes.
    /// </remarks>
    public class ReflectiveTreeView : TreeView
    {
        private object _treeObject = null;

        public ReflectiveTreeView() : base()
        {
            ShowNodeToolTips = true;
        }
        /// <summary>
        /// Object to convert into a tree
        /// </summary>
        [Browsable(false)]
        public object TreeObject
        {
            get
            {
                return _treeObject;
            }
            set
            {
                try
                {
                    _treeObject = value;
                    AccessibleRole = AccessibleRole.Outline;
                    AccessibleDescription = "";
                    Nodes.Clear();
                    if (_treeObject != null)
                    {
                        TreeNode rootNode = CreateNodeForObject(_treeObject);
                        AccessibleDescription = CAP_TreeNode + rootNode.ToolTipText ?? rootNode.Text;
                        Nodes.Add(rootNode);
                    }
                }
                catch { }
            }
        }
        /// <summary>
        /// Create a node and its descendants
        /// </summary>
        /// <param name="obj">
        /// Root node object
        /// </param>
        /// <param name="name">
        /// Force node name or null to use object type name or description attribute
        /// </param>
        /// <returns>
        /// New node for the given object
        /// </returns>
        public TreeNode CreateNodeForObject(object obj, string name = null)
        {
            string tooltip = null;
            // Use Description attribute to add tooltips to nodes
            DescriptionAttribute descattr = obj.GetType().GetCustomAttribute<DescriptionAttribute>();
            if (descattr != null)
            {
                tooltip = descattr.Description;
            }
            if (string.IsNullOrEmpty(name))
            {
                // Use STDName for IStandardObjects
                IStandardObject stdobj = obj as IStandardObject;
                if (stdobj != null)
                {
                    name = stdobj.StdName;
                }
                else
                {
                    // Use DisplayName attribute in classes to force node name
                    DisplayNameAttribute dnattr = obj.GetType().GetCustomAttribute<DisplayNameAttribute>();
                    name = dnattr != null ? dnattr.DisplayName : obj.GetType().Name;
                }
            }
            TreeNode node = new TreeNode(name)
            {
                // Use Tag to store each node object
                Tag = obj
            };
            if (!string.IsNullOrEmpty(tooltip))
            {
                node.ToolTipText = tooltip;
            }
            // Process all object properties
            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                tooltip = null;
                // Use Description attribute to add tooltips to nodes
                descattr = property.GetCustomAttribute<DescriptionAttribute>();
                if (descattr != null)
                {
                    tooltip = descattr.Description;
                }
                object value = null;
                // Discard null or non-browsable properties
                if (property.IsBrowsable() && ((value = property.GetValue(obj)) != null))
                {
                    if (value is Type)
                    {
                        TreeNode propertyNode = new TreeNode($"{property.UIName()}")
                        {
                            Tag = obj
                        };
                        if (!string.IsNullOrEmpty(tooltip))
                        {
                            propertyNode.ToolTipText = tooltip;
                        }
                        propertyNode.Nodes.Add(new TreeNode($"{((Type)value).Name}")
                        {
                            Tag = value
                        });
                        node.Nodes.Add(propertyNode);
                    }
                    else if (property.PropertyType == typeof(JsonElement))
                    {
                        // Json Objects are converted into text
                        TreeNode propertyNode = new TreeNode($"{property.UIName()}")
                        {
                            Tag = obj
                        };
                        if (!string.IsNullOrEmpty(tooltip))
                        {
                            propertyNode.ToolTipText = tooltip;
                        }
                        string vjson = JsonSerializer.Serialize(value);
                        propertyNode.Nodes.Add(new TreeNode($"{vjson}")
                        {
                            Tag = value
                        });
                        node.Nodes.Add(propertyNode);
                    }
                    else if (IsSimpleType(value.GetType()))
                    {
                        // Simple types are added as a node with a child with its value
                        TreeNode propertyNode = new TreeNode($"{property.UIName()}")
                        {
                            Tag = obj
                        };
                        if (!string.IsNullOrEmpty(tooltip))
                        {
                            propertyNode.ToolTipText = tooltip;
                        }
                        propertyNode.Nodes.Add(new TreeNode($"{value}")
                        {
                            Tag = value
                        });
                        node.Nodes.Add(propertyNode);
                    }
                    else if (typeof(IDictionary).IsAssignableFrom(value.GetType()))
                    {
                        // Handle dictionaries
                        TreeNode propertyNode = new TreeNode(property.UIName())
                        {
                            Tag = value
                        };
                        if (!string.IsNullOrEmpty(tooltip))
                        {
                            propertyNode.ToolTipText = tooltip;
                        }
                        foreach (DictionaryEntry entry in (IDictionary)value)
                        {
                            TreeNode keyNode = new TreeNode(entry.Key.ToString())
                            {
                                Tag = entry.Key
                            };
                            if (IsSimpleType(entry.Value.GetType()))
                            {
                                keyNode.Nodes.Add(new TreeNode(entry.Value.ToString())
                                {
                                    Tag = entry.Value
                                });
                            }
                            else
                            {
                                // Directly add properties of the value object under the key node
                                foreach (PropertyInfo entryProperty in entry.Value.GetType().GetProperties())
                                {
                                    tooltip = null;
                                    // Use Description attribute to add tooltips to nodes
                                    descattr = entryProperty.GetCustomAttribute<DescriptionAttribute>();
                                    if (descattr != null)
                                    {
                                        tooltip = descattr.Description;
                                    }
                                    object entryValue = entryProperty.GetValue(entry.Value);
                                    if (entryValue != null)
                                    {
                                        TreeNode entryPropertyNode = new TreeNode($"{entryProperty.UIName()}")
                                        {
                                            Tag = entryProperty
                                        };
                                        if (!string.IsNullOrEmpty(tooltip))
                                        {
                                            entryPropertyNode.ToolTipText = tooltip;
                                        }
                                        entryPropertyNode.Nodes.Add(new TreeNode($"{entryValue}")
                                        {
                                            Tag = entryValue
                                        });
                                        keyNode.Nodes.Add(entryPropertyNode);
                                    }
                                }
                            }
                            propertyNode.Nodes.Add(keyNode);
                        }
                        node.Nodes.Add(propertyNode);
                    }
                    else if (typeof(IEnumerable).IsAssignableFrom(value.GetType()) && value.GetType() != typeof(string))
                    {
                        // Collections are added as a node with its elements as children
                        TreeNode propertyNode = new TreeNode(property.UIName())
                        {
                            Tag = value
                        };
                        if (!string.IsNullOrEmpty(tooltip))
                        {
                            propertyNode.ToolTipText = tooltip;
                        }
                        foreach (var item in (IEnumerable)value)
                        {
                            if (IsSimpleType(item.GetType()))
                            {
                                propertyNode.Nodes.Add(new TreeNode($"{item}")
                                {
                                    Tag = item
                                });
                            }
                            else
                            {
                                TreeNode itemNode = CreateNodeForObject(item);
                                propertyNode.Nodes.Add(itemNode);
                            }
                        }
                        node.Nodes.Add(propertyNode);
                    }
                    else
                    {
                        TreeNode propertyNode = CreateNodeForObject(value, property.UIName());
                        node.Nodes.Add(propertyNode);
                    }
                }
            }

            return node;
        }
        /// <summary>
        /// Check for types that don't need to be expanded
        /// </summary>
        /// <param name="type">
        /// GenericType to check
        /// </param>
        /// <returns>
        /// True if type its simple
        /// </returns>
        private bool IsSimpleType(Type type)
        {
            return type.IsPrimitive || type.IsEnum || type.IsValueType || type == typeof(string) || type == typeof(decimal);
        }
    }
    /// <summary>
    /// Tool for property attribute management
    /// </summary>
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// Check for Browsable attribute
        /// </summary>
        /// <param name="property">
        /// Property to check
        /// </param>
        /// <returns>
        /// True if property is browsable
        /// </returns>
        public static bool IsBrowsable(this PropertyInfo property)
        {
            BrowsableAttribute browsableAttribute = property.GetCustomAttribute<BrowsableAttribute>();
            return browsableAttribute == null || browsableAttribute.Browsable;
        }
        /// <summary>
        /// Get the UI name for a property. 
        /// </summary>
        /// <param name="property">
        /// Porperty to check
        /// </param>
        /// <returns>
        /// Name from attributes, or property name if no attribute is found
        /// </returns>
        public static string UIName(this PropertyInfo property)
        {
            // Check DisplayName attribute
            DisplayNameAttribute dnattr = property.GetCustomAttribute<DisplayNameAttribute>();
            if (dnattr == null)
            {
                // Check JsonPropertyName attribute
                JsonPropertyNameAttribute jpattr = property.GetCustomAttribute<JsonPropertyNameAttribute>();
                if (jpattr == null)
                {
                    return property.Name;
                }
                return jpattr.Name;
            }
            return dnattr.DisplayName;
        }
    }
}
