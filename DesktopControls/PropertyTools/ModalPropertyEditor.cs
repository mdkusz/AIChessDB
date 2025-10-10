using GlobalCommonEntities.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace DesktopControls.PropertyTools
{
    /// <summary>
    /// Editor de propiedades mediante un cuadro de diálogo modal proporcionado por la instancia / 
    /// Property editor using a modal dialog provided by the instance
    /// </summary>
    public class ModalPropertyEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context.Instance is IPropertyCommandManager)
            {
                PropertyCommandEventArgs args = new PropertyCommandEventArgs()
                {
                    PropertyName = context.PropertyDescriptor.Name,
                    CommandIndex = 0
                };
                ((IPropertyCommandManager)context.Instance).PropertyCommand(this, args);
                if (!args.Cancel)
                {
                    value = context.PropertyDescriptor.GetValue(context.Instance);
                }
            }
            return value;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}
