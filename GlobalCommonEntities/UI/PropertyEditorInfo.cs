using GlobalCommonEntities.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace GlobalCommonEntities.UI
{
    /// <summary>
    /// Information to edit a property
    /// </summary>
    /// <remarks>
    /// This implementation is platform independent. User interface editors can be Windows Forms, WPF, Web, etc. 
    /// This is a simple and flexible way to define the properties that can be edited in a class.
    /// </remarks>
    public class PropertyEditorInfo : IEquatable<PropertyEditorInfo>
    {
        /// <summary>
        /// Show the propery in the editor
        /// </summary>
        public bool Visible { get; set; } = true;
        /// <summary>
        /// Property is read only
        /// </summary>
        public bool ReadOnly { get; set; } = false;
        /// <summary>
        /// GenericType of editor to use
        /// </summary>
        public InputEditorType EditorType { get; set; }
        /// <summary>
        /// Range of values; value list; file filter
        /// </summary>
        public List<object> Values { get; set; }
        /// <summary>
        /// Default value
        /// </summary>
        public object InitialValue { get; set; }
        /// <summary>
        /// Service to provide extra information and features to the editor
        /// </summary>
        public object Service { get; set; }
        /// <summary>
        /// Property name
        /// Procedure name for CommandButton
        /// Title for BlockTitle
        /// </summary>
        /// <remarks>
        /// PropertyName is not just the name of a property. It can be a path to a child object property, a dictionary key, a property of an object in a dictonary entry, or a property of an object in a given list position.
        /// It can be also the name of a method.
        /// Property editors must be defined having this in mind.
        /// </remarks>
        public string PropertyName { get; set; }
        /// <summary>
        /// Define the array of parameter types when PropertyName is a method name.
        /// </summary>
        public Type[] MethodParameters { get; set; } = Type.EmptyTypes;
        /// <summary>
        /// Parameter values to invoke the method
        /// </summary>
        public object[] MethodParameterValues { get; set; }
        /// <summary>
        /// Property value is required
        /// </summary>
        public bool Required { get; set; } = false;
        /// <summary>
        /// Max element units (lines, items, etc.)
        /// </summary>
        /// <example>
        /// Use this property to limit the number of dropdown items in a combobox, or the number of lines in a multiline text editor.
        /// </example>
        public int MaxUnits { get; set; }
        /// <summary>
        /// RGB Background Color
        /// </summary>
        public int? BackColor { get; set; }
        /// <summary>
        /// RGB Foreground Color
        /// </summary>
        public int? ForeColor { get; set; }
        /// <summary>
        /// Border size. Each side can have a different size. 0 means no border.
        /// </summary>
        public Padding BorderSize { get; set; } = new Padding(0);
        /// <summary>
        /// Border colors
        /// </summary>
        public Color LeftBorderColor { get; set; }
        public Color TopBorderColor { get; set; }
        public Color RightBorderColor { get; set; }
        public Color BottomBorderColor { get; set; }
        /// <summary>
        /// Label for command properties
        /// </summary>
        /// <remarks>
        /// When propertyname is a method name, this is the editor Caption text.
        /// </remarks>
        public string CommandLabel { get; set; }
        /// <summary>
        /// Caption for command buttons
        /// </summary>
        /// <remarks>
        /// When propertyname is a method name, this is the label for the command button.
        /// </remarks>
        public List<string> CommandCaptions { get; set; }
        /// <summary>
        /// Solve a string of name parts separated by dots to get the PropertyInfo object addressed by the path
        /// </summary>
        /// <param name="instance">
        /// Instance of the class declaring this property editor info
        /// </param>
        /// <returns>
        /// PropertyInfo corresponding with the addressed PropertyName.
        /// </returns>
        public PropertyInfo ResolveProperty(ref object instance)
        {
            List<string> propParts = PropertyName.Split('.').ToList();
            PropertyInfo property = instance.GetType().GetRuntimeProperty(propParts[0]);
            propParts.RemoveAt(0);
            while (propParts.Count > 0)
            {
                if (property.GetValue(instance) is IDictionary)
                {
                    // Find the property in a given Dicitionary element by its Key
                    instance = ((IDictionary)property.GetValue(instance))[propParts[0]];
                    propParts.RemoveAt(0);
                    property = instance.GetType().GetRuntimeProperty(propParts[0]);
                    propParts.RemoveAt(0);
                }
                else if (property.GetValue(instance) is IList)
                {
                    // Find the property in a given list element by its index
                    instance = ((IList)property.GetValue(instance))[int.Parse(propParts[0])];
                    propParts.RemoveAt(0);
                    property = instance.GetType().GetRuntimeProperty(propParts[0]);
                    propParts.RemoveAt(0);
                }
                else if (property.GetValue(instance) is Array array)
                {
                    // Handle multiple indices for arrays
                    int rank = array.Rank;
                    int[] indices = new int[rank];
                    for (int i = 0; i < rank; i++)
                    {
                        if (propParts.Count > 0 && int.TryParse(propParts[0], out int index))
                        {
                            indices[i] = index;
                            propParts.RemoveAt(0);
                        }
                        else
                        {
                            throw new ArgumentException("Insufficient indices for array dimensions.");
                        }
                    }
                    instance = array.GetValue(indices);
                    if (propParts.Count > 0)
                    {
                        property = instance.GetType().GetRuntimeProperty(propParts[0]);
                        propParts.RemoveAt(0);
                    }
                }
                else
                {
                    // Find the property at the next object level                    
                    instance = property.GetValue(instance);
                    property = instance.GetType().GetRuntimeProperty(propParts[0]);
                    propParts.RemoveAt(0);
                }
            }
            return property;
        }
        /// <summary>
        /// Solve a string of name parts separated by dots to get the MethodInfo object addressed by the path
        /// </summary>
        /// <param name="instance">
        /// Instance of the class declaring this property editor info
        /// </param>
        /// <returns>
        /// MethodInfo corresponding with the addressed PropertyName.
        /// </returns>
        public MethodInfo ResolveMethod(ref object instance)
        {
            PropertyInfo property = null;
            List<string> propParts = PropertyName.Split('.').ToList();
            IUIDataSheet sheet = instance as IUIDataSheet;
            sheet?.SetMethodParameterTypes(propParts[0], this);
            MethodInfo method = instance.GetType().GetRuntimeMethod(propParts[0], MethodParameters);
            if (method == null)
            {
                property = instance.GetType().GetRuntimeProperty(propParts[0]);
                propParts.RemoveAt(0);
                while (propParts.Count > 0)
                {
                    if (property.GetValue(instance) is IDictionary)
                    {
                        // Find the method in a given Dicitionary element by its Key
                        instance = ((IDictionary)property.GetValue(instance))[propParts[0]];
                        if (instance is IUIDataSheet)
                        {
                            sheet = (IUIDataSheet)instance;
                        }
                        propParts.RemoveAt(0);
                        sheet?.SetMethodParameterTypes(propParts[0], this);
                        method = instance.GetType().GetRuntimeMethod(propParts[0], MethodParameters);
                        if (method == null)
                        {
                            property = instance.GetType().GetRuntimeProperty(propParts[0]);
                            propParts.RemoveAt(0);
                        }
                        else
                        {
                            propParts.Clear();
                        }
                    }
                    else if (property.GetValue(instance) is IList)
                    {
                        // Find the method in a given list element by its index
                        instance = ((IList)property.GetValue(instance))[int.Parse(propParts[0])];
                        if (instance is IUIDataSheet)
                        {
                            sheet = (IUIDataSheet)instance;
                        }
                        propParts.RemoveAt(0);
                        sheet?.SetMethodParameterTypes(propParts[0], this);
                        method = instance.GetType().GetRuntimeMethod(propParts[0], MethodParameters);
                        if (method == null)
                        {
                            property = instance.GetType().GetRuntimeProperty(propParts[0]);
                            propParts.RemoveAt(0);
                        }
                        else
                        {
                            propParts.Clear();
                        }
                    }
                    else if (property.GetValue(instance) is Array array)
                    {
                        // Handle multiple indices for arrays
                        int rank = array.Rank;
                        int[] indices = new int[rank];
                        for (int i = 0; i < rank; i++)
                        {
                            if (propParts.Count > 0 && int.TryParse(propParts[0], out int index))
                            {
                                indices[i] = index;
                                propParts.RemoveAt(0);
                            }
                            else
                            {
                                throw new ArgumentException("Insufficient indices for array dimensions.");
                            }
                        }
                        instance = array.GetValue(indices);
                        if (instance is IUIDataSheet)
                        {
                            sheet = (IUIDataSheet)instance;
                        }
                        sheet?.SetMethodParameterTypes(propParts[0], this);
                        method = instance.GetType().GetRuntimeMethod(propParts[0], MethodParameters);
                        if (method == null)
                        {
                            property = instance.GetType().GetRuntimeProperty(propParts[0]);
                            propParts.RemoveAt(0);
                        }
                        else
                        {
                            propParts.Clear();
                        }
                    }
                    else
                    {
                        // Find the method at the next object level                    
                        instance = property.GetValue(instance);
                        if (instance is IUIDataSheet)
                        {
                            sheet = (IUIDataSheet)instance;
                        }
                        propParts.RemoveAt(0);
                        sheet?.SetMethodParameterTypes(propParts[0], this);
                        method = instance.GetType().GetRuntimeMethod(propParts[0], MethodParameters);
                        if (method == null)
                        {
                            property = instance.GetType().GetRuntimeProperty(propParts[0]);
                            propParts.RemoveAt(0);
                        }
                        else
                        {
                            propParts.Clear();
                        }
                    }
                }
            }
            return method;
        }
        public bool Equals(PropertyEditorInfo other)
        {
            return (PropertyName == other.PropertyName) &&
                (InitialValue == other.InitialValue) &&
                (CommandLabel == other.CommandLabel);
        }
    }
}
