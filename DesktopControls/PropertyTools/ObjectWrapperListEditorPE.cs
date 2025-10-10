using DesktopControls.Controls.PropertyTable.Interfaces;
using GlobalCommonEntities.DependencyInjection;
using GlobalCommonEntities.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DesktopControls.PropertyTools
{
    /// <summary>
    /// Editor de selección de tabla de consulta / 
    /// Editor to select a query table
    /// </summary>
    public class ObjectWrapperListEditorPE : UITypeEditor
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
                PEListBoxWithCommandButtons dropdown = new PEListBoxWithCommandButtons()
                {
                    CommandManager = context.Instance as IPropertyCommandManager,
                    PropertyName = context.PropertyDescriptor.Name
                };
                dropdown.ItemsList.Selection = value;
                dropdown.ItemsList.WInSrv = edSvc;
                // Llenar lista desplegable con las opciones disponibles
                // Fill dropdown list with available options
                foreach (ObjectWrapper obj in lprovider.GetAllowedValues(context.PropertyDescriptor.Name))
                {
                    dropdown.ItemsList.Items.Add(obj);
                }
                edSvc.DropDownControl(dropdown);
                if (dropdown.ItemsList.Selection != null)
                {
                    if (dropdown.ItemsList.Selection.GetType() == context.PropertyDescriptor.PropertyType)
                    {
                        return dropdown.ItemsList.Selection;
                    }
                    else if ((dropdown.ItemsList.Selection as ObjectWrapper) != null)
                    {
                        return (dropdown.ItemsList.Selection as ObjectWrapper).Implementation();
                    }
                }
            }
            return value;
        }
    }
}
