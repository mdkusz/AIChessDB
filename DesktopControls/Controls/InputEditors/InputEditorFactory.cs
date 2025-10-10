using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using static DesktopControls.Properties.UIResources;

namespace DesktopControls.Controls.InputEditors
{
    /// <summary>
    /// Factory to create UIDataSheet property editors
    /// </summary>
    /// <example>
    /// You can see examples of use of this class in almost all forms in the AIAPIWinAdmin project.
    /// </example>
    /// <remarks>
    /// Use this class to create editors for UIDataSheet properties in Windows Desktop applications.
    /// You can create your own editor classes to edit properties using a different set of controls.
    /// </remarks>
    /// <seealso cref="UIDataSheet"/>
    /// <seealso cref="InputEditorBase"/>
    /// <seealso cref="PropertyInputEditorBase"/> 
    /// <seealso cref="MethodInputEditorBase"/>
    public class InputEditorFactory : IInputEditorFactory
    {
        public InputEditorFactory() { }
        /// <summary>
        /// IInputEditorFactory: Editor border size. Each side can have a different size. 0 means no border.
        /// </summary>
        public Padding BorderSize { get; set; } = new Padding(-1);
        /// <summary>
        /// IInputEditorFactory: Editor left border color
        /// </summary>
        public Color LeftBorderColor { get; set; } = Color.Empty;
        /// <summary>
        /// IInputEditorFactory: Editor top border color
        /// </summary>
        public Color TopBorderColor { get; set; } = Color.Empty;
        /// <summary>
        /// IInputEditorFactory: Editor right border color
        /// </summary>
        public Color RightBorderColor { get; set; } = Color.Empty;
        /// <summary>
        /// IInputEditorFactory: Editor bottom border color
        /// </summary>
        public Color BottomBorderColor { get; set; } = Color.Empty;
        /// <summary>
        /// IInputEditorFactory: Editor background color
        /// </summary>
        public Color EditorBackColor { get; set; } = Color.Empty;
        /// <summary>
        /// IInputEditorFactory: Editor foreground color
        /// </summary>
        public Color EditorForeColor { get; set; } = Color.Empty;
        /// <summary>
        /// IInputEditorFactory: Block header foreground color
        /// </summary>
        public Color BlockHeaderForeColor { get; set; } = Color.Empty;
        /// <summary>
        /// IInputEditorFactory: Block header background color
        /// </summary>
        public Color BlockHeaderBackColor { get; set; } = Color.Empty;
        /// <summary>
        /// IInputEditorFactory: Check whether an editor type is supported
        /// </summary>
        /// <param name="etype">
        /// EditorType
        /// </param>
        /// <returns>
        /// True if the editor type is suppoerted
        /// </returns>
        public virtual bool AcceptsEditorType(InputEditorType etype)
        {
            switch (etype)
            {
                case InputEditorType.BlockTitle:
                case InputEditorType.CommandButton:
                case InputEditorType.ObjectSelector:
                case InputEditorType.SingleLineText:
                case InputEditorType.MultilineText:
                case InputEditorType.FixedComboBox:
                case InputEditorType.ComboBox:
                case InputEditorType.MultiSelectList:
                case InputEditorType.IntValue:
                case InputEditorType.FloatValue:
                case InputEditorType.BoolValue:
                case InputEditorType.Color:
                case InputEditorType.Directory:
                case InputEditorType.FileName:
                case InputEditorType.FileCollection:
                case InputEditorType.Password:
                case InputEditorType.DataSheet:
                    return true;
                default:
                    return false;
            }
        }
        /// <summary>
        /// IInputEditorFactory: Create an editor for a property
        /// </summary>
        /// <param name="pinfo">
        /// Porperty creation and configuration information
        /// </param>
        /// <param name="instance">
        /// IUIDataSheet object to edit
        /// </param>
        /// <param name="container">
        /// Editor container control
        /// </param>
        /// <returns>
        /// InputEditorBase control
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// No editor implemented for the EditorType enum value 
        /// </exception>
        /// <remarks>
        /// Editors are all derived classes of InputEditorBase.
        /// Editors for Properties are derived from PropertyInputEditorBase.
        /// Editors that use methods are derived from MethodInputEditorBase.
        /// The enum InputEditorType is used to select the editor type. Exceptions of that are IDictionary properties, that use the DictionaryKeyInputEditor.
        /// </remarks>
        /// <seealso cref="InputEditorType"/>
        /// <seealso cref="PropertyEditorInfo"/>
        /// <seealso cref="HeaderInputEditor"/>
        /// <seealso cref="CommandButtonInputEditor"/>
        /// <seealso cref="ObjectSelectorInputEditor"/>
        /// <seealso cref="DictionaryKeyInputEditor"/>
        /// <seealso cref="SingleLineTextInputEditor"/>
        /// <seealso cref="MultiLineTextInputEditor"/>
        /// <seealso cref="MultiSelectListInputEditor"/>
        /// <seealso cref="DirectoryInputEditor"/>
        /// <seealso cref="FileNameInputEditor"/>
        /// <seealso cref="MultipleFileNameInputEditor"/>
        /// <seealso cref="FixedComboBoxInputEditor"/>
        /// <seealso cref="ComboBoxInputEditor"/>
        /// <seealso cref="IntValueInputEditor"/>
        /// <seealso cref="FloatValueInputEditor"/>
        /// <seealso cref="BoolValueInputEditor"/>
        /// <seealso cref="ColorInputEditor"/>
        public virtual IInputEditorBase CreateEditor(PropertyEditorInfo pinfo, object instance, Control container)
        {
            if (pinfo.EditorType == InputEditorType.BlockTitle)
            {
                if (BlockHeaderBackColor != Color.Empty)
                {
                    pinfo.BackColor = BlockHeaderBackColor.ToArgb();
                }
                if (BlockHeaderForeColor != Color.Empty)
                {
                    pinfo.ForeColor = BlockHeaderForeColor.ToArgb();
                }
                return new HeaderInputEditor(pinfo, instance, container);
            }
            if (EditorForeColor != Color.Empty)
            {
                pinfo.ForeColor = EditorForeColor.ToArgb();
            }
            if (EditorBackColor != Color.Empty)
            {
                pinfo.BackColor = EditorBackColor.ToArgb();
            }
            if (BorderSize != new Padding(-1))
            {
                pinfo.BorderSize = BorderSize;
            }
            if (LeftBorderColor != Color.Empty)
            {
                pinfo.LeftBorderColor = LeftBorderColor;
            }
            if (TopBorderColor != Color.Empty)
            {
                pinfo.TopBorderColor = TopBorderColor;
            }
            if (RightBorderColor != Color.Empty)
            {
                pinfo.RightBorderColor = RightBorderColor;
            }
            if (BottomBorderColor != Color.Empty)
            {
                pinfo.BottomBorderColor = BottomBorderColor;
            }
            if (pinfo.EditorType == InputEditorType.CommandButton)
            {
                return new CommandButtonInputEditor(pinfo, instance, container);
            }
            if (pinfo.EditorType == InputEditorType.ObjectSelector)
            {
                return new ObjectSelectorInputEditor(pinfo, instance, container);
            }
            string[] propParts = pinfo.PropertyName.Split('.');
            PropertyInfo property = instance.GetType().GetRuntimeProperty(propParts[0]);
            if (property == null)
            {
                throw new ArgumentException(ERR_UnknownProperty);
            }
            if (property.PropertyType.GetInterface(nameof(IDictionary)) != null)
            {
                if (propParts.Length == 1)
                {
                    return new DictionaryKeyInputEditor(pinfo, instance, container);
                }
            }
            switch (pinfo.EditorType)
            {
                case InputEditorType.SingleLineText:
                case InputEditorType.Password:
                    return new SingleLineTextInputEditor(pinfo, instance, container);
                case InputEditorType.MultilineText:
                    return new MultiLineTextInputEditor(pinfo, instance, container);
                case InputEditorType.FixedComboBox:
                    return new FixedComboBoxInputEditor(pinfo, instance, container);
                case InputEditorType.ComboBox:
                    return new ComboBoxInputEditor(pinfo, instance, container);
                case InputEditorType.MultiSelectList:
                    return new MultiSelectListInputEditor(pinfo, instance, container);
                case InputEditorType.IntValue:
                    return new IntValueInputEditor(pinfo, instance, container);
                case InputEditorType.FloatValue:
                    return new FloatValueInputEditor(pinfo, instance, container);
                case InputEditorType.BoolValue:
                    return new BoolValueInputEditor(pinfo, instance, container);
                case InputEditorType.Color:
                    return new ColorInputEditor(pinfo, instance, container);
                case InputEditorType.Directory:
                    return new DirectoryInputEditor(pinfo, instance, container);
                case InputEditorType.FileName:
                    return new FileNameInputEditor(pinfo, instance, container);
                case InputEditorType.FileCollection:
                    return new MultipleFileNameInputEditor(pinfo, instance, container);
                case InputEditorType.DataSheet:
                    return new GenericDataSheetInputEditor(pinfo, instance, container);
                default:
                    throw new NotImplementedException(ERR_BadEditorType);
            }
        }
        /// <summary>
        /// IInputEditorFactory: Populate a list of proerty editors in a container
        /// </summary>
        /// <param name="ds">
        /// IUIDataSheet object with properties
        /// </param>
        /// <param name="container">
        /// Container control to add property editors
        /// </param>
        /// <param name="readOnly">
        /// Populate read only or regular properties
        /// </param>
        public void SetPropertyEditors(IUIDataSheet ds, Control container, bool readOnly = false)
        {
            List<PropertyEditorInfo> properties = readOnly ? ds.ROProperties : ds.Properties;
            foreach (PropertyEditorInfo pi in properties)
            {
                IInputEditorBase dsp = CreateEditor(pi, ds, container);
                container.Controls.Add(dsp as Control);
            }
        }
    }
}
