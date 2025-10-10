using GlobalCommonEntities.DependencyInjection;
using GlobalCommonEntities.Interfaces;
using System;
using System.Windows.Forms;

namespace DesktopControls.Controls.PropertyTable.PropertyEditors
{
    /// <summary>
    /// Editor de propiedad ObjectWrapper con lista de elementos activos / 
    /// ObjectWrapper property editor with an active element list
    /// </summary>
    public class ObjectWrapperActiveObjectListPropertyEditor : ObjectWrapperCheckedListBoxPropertyEditor
    {
        public ObjectWrapperActiveObjectListPropertyEditor() : base()
        {
        }
        /// <summary>
        /// Lista de valores disponibles para la propiedad / 
        /// Property available values list
        /// </summary>
        protected override void GetAvailableValues()
        {
            _editor.Controls.Clear();
            foreach (ObjectWrapper owr in _vProvider.GetAllowedValues(_property.Name))
            {
                CheckBox cb = new CheckBox()
                {
                    Name = "cb" + (_editor.Controls.Count + 1).ToString(),
                    Text = owr.FriendlyName,
                    AutoSize = true,
                    AutoCheck = true,
                    Checked = owr.Selected,
                    Tag = owr.Implementation()
                };
                cb.CheckedChanged += new EventHandler(cbEditor_ItemCheck);
                _editor.Controls.Add(cb);
            }
            if ((_editor != null) && (_editor.Controls.Count > 0))
            {
                _editor.Height = Math.Min(8, _editor.Controls.Count + 1) * _editor.Controls[0].Height;
            }
        }
        /// <summary>
        /// Cambiar estado de selección de uno de los posibles valores / 
        /// Change value selection state
        /// </summary>
        protected override void cbEditor_ItemCheck(object sender, EventArgs e)
        {
            if (_valueList != null)
            {
                CheckBox cb = sender as CheckBox;
                IActiveObject aobj = cb.Tag as IActiveObject;
                if (aobj != null)
                {
                    aobj.Active = cb.Checked;
                    if (aobj is IUIIdentifier)
                    {
                        cb.Text = ((IUIIdentifier)aobj).FriendlyName;
                    }
                }
                SetValue();
            }
        }
    }
}
