using DesktopControls.Controls.PropertyTable.Interfaces;
using GlobalCommonEntities.DependencyInjection;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DesktopControls.PropertyTools
{
    /// <summary>
    /// Editor de selección de lista de objetos / 
    /// Editor to select an object from a list
    /// </summary>
    public class ObjectWrapperListEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
        public override object EditValue(ITypeDescriptorContext context,
            IServiceProvider provider, object value)
        {
            IValueSelectionListProvider lprovider = context.Instance as IValueSelectionListProvider;
            if (lprovider != null)
            {
                IWindowsFormsEditorService edSvc =
                    (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                PropertyEditorListBox dropdown = new PropertyEditorListBox(value, edSvc);
                // Llenar lista desplegable con las opciones disponibles
                // Fill dropdown list with available options
                foreach (ObjectWrapper obj in lprovider.GetAllowedValues(context.PropertyDescriptor.Name))
                {
                    dropdown.Items.Add(obj);
                }
                if (dropdown.Items.Count > 0)
                {
                    edSvc.DropDownControl(dropdown);
                    if (dropdown.Selection != null)
                    {
                        if (dropdown.Selection.GetType() == context.PropertyDescriptor.PropertyType)
                        {
                            return dropdown.Selection;
                        }
                        else if ((dropdown.Selection as ObjectWrapper) != null)
                        {
                            return (dropdown.Selection as ObjectWrapper).Implementation();
                        }
                    }
                }
            }
            return value;
        }
    }
}
