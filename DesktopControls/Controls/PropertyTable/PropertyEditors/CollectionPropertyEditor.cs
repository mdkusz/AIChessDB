using DesktopControls.Controls.PropertyTable.Interfaces;
using System;
using static DesktopControls.Properties.UIResources;

namespace DesktopControls.Controls.PropertyTable.PropertyEditors
{
    /// <summary>
    /// Editor de propiedades de tipo colección / 
    /// Collection type property editor
    /// </summary>
    public class CollectionPropertyEditor : LabelExpandablePropertyEditor
    {
        protected IPropertyConfigurationProvider _cfgProvider;
        protected IndexedPropertyValueManager _ixManager;

        public CollectionPropertyEditor()
        {
        }
        /// <summary>
        /// Objeto al que pertenece la propiedad / 
        /// Object owning the property
        /// </summary>
        public override object Instance
        {
            get => base.Instance;
            set
            {
                // Comprobar interfaces necesarios
                // Check for mandatory interfaces
                if (!(value is IPropertyConfigurationProvider))
                {
                    throw new ArgumentException(ERR_PropertyConfiguration, "Instance");
                }
                _cfgProvider = value as IPropertyConfigurationProvider;
                if (!(value is IndexedPropertyValueManager))
                {
                    throw new ArgumentException(ERR_IndexedProperty, "Instance");
                }
                _ixManager = value as IndexedPropertyValueManager;
                base.Instance = value;
            }
        }
        /// <summary>
        /// Refrescar los datos mostrados al usuario / 
        /// Refresh data shown to the user
        /// </summary>
        public override void RefreshValue()
        {
            if (_ixManager.CollectionChanged(_property.Name))
            {
                ConfigureUI();
            }
            else
            {
                base.RefreshValue();
            }
        }
        /// <summary>
        /// Listar los valores visibles de la colección / 
        /// List visible collection values
        /// </summary>
        protected override void ListProperties()
        {
            for (int ix = 0; ix < _ixManager.ValueCount(_property.Name); ix++)
            {
                bool browsable = _cfgProvider.Browsable(_property.Name, ix).HasValue ? _cfgProvider.Browsable(_property.Name, ix).Value : true;
                if (browsable)
                {
                    _properties.AddProperty(_property, _instance, ix);
                }
            }
        }
    }
}
