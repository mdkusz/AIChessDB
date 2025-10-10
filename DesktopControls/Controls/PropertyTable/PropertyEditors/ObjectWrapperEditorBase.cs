using DesktopControls.Controls.PropertyTable.Interfaces;
using GlobalCommonEntities.DependencyInjection;

namespace DesktopControls.Controls.PropertyTable.PropertyEditors
{
    /// <summary>
    /// Editor base para propiedades ObjectWrapper / 
    /// ObjectWrapper properties base editor
    /// </summary>
    public class ObjectWrapperEditorBase : PropertyEditorBase
    {
        protected IValueSelectionListProvider _vProvider;
        protected ObjectWrapper _selectedItem;
        public ObjectWrapperEditorBase() : base()
        {
        }
        /// <summary>
        /// Nombre de la propiedad para mostrar al usuario / 
        /// Property name to show to the user
        /// </summary>
        public override string DisplayName
        {
            get
            {
                if (_showValueDescription &&
                    (_property != null) &&
                    (_instance != null) &&
                    _selectedItem != null)
                {
                    return _selectedItem.FriendlyName;
                }
                return base.DisplayName;
            }
            set { base.DisplayName = value; }
        }
        /// <summary>
        /// Descripción de la propiedad para mostrar al usuario / 
        /// Property description to show to the user
        /// </summary>
        public override string Description
        {
            get
            {
                if (_showValueDescription &&
                    (_property != null) &&
                    (_instance != null) &&
                    _selectedItem != null)
                {
                    return _selectedItem.FriendlyDescription;
                }
                return base.Description;
            }
            set { base.Description = value; }
        }
        /// <summary>
        /// Instance del objeto que contiene la propiedad / 
        /// Property owner object
        /// </summary>
        public override object Instance
        {
            get => base.Instance;
            set
            {
                _vProvider = value as IValueSelectionListProvider;
                base.Instance = value;
            }
        }
        /// <summary>
        /// Refrescar los datos mostrados al usuario / 
        /// Refresh data shown to the user
        /// </summary>
        public override void RefreshValue()
        {
            if ((_property != null) &&
                (_vProvider != null))
            {
                GetAvailableValues();
            }
            base.RefreshValue();
        }
        /// <summary>
        /// Lista de valores disponibles para la propiedad / 
        /// Property available values list
        /// </summary>
        protected virtual void GetAvailableValues()
        {
        }
    }
}
