using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DesktopControls.PropertyTools
{
    /// <summary>
    /// ListBox genérica para seleccionar objetos en editores de propiedades / 
    /// Generic ListBox to select objects in property editors
    /// </summary>
    public class PropertyEditorListBox : ListBox
    {
        protected object m_oSelection = null;
        protected IWindowsFormsEditorService m_iwsService = null;
        public PropertyEditorListBox() : base()
        {
            SelectionMode = SelectionMode.One;
            BorderStyle = BorderStyle.None;
        }
        public IWindowsFormsEditorService WInSrv
        {
            get
            {
                return m_iwsService;
            }
            set
            {
                m_iwsService = value;
            }
        }
        public PropertyEditorListBox(object selection, IWindowsFormsEditorService edsvc) : base()
        {
            Selection = selection;
            m_iwsService = edsvc;
            SelectionMode = SelectionMode.One;
            BorderStyle = BorderStyle.None;
        }
        /// <summary>
        /// Objeto seleccionado / 
        /// Selected object
        /// </summary>
        public object Selection
        {
            get
            {
                return m_oSelection;
            }
            set
            {
                m_oSelection = value;
                SelectedItem = m_oSelection;
            }
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            if (SelectedItem != null)
            {
                m_oSelection = SelectedItem;
                if (m_iwsService != null)
                {
                    m_iwsService.CloseDropDown();
                }
            }
        }
    }
}
