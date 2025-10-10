using DesktopControls.Controls.PropertyTable.PropertyEditors;
using DesktopControls.PropertyTools;
using GlobalCommonEntities.DependencyInjection;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace DesktopControls.Controls.PropertyTable.Interfaces
{
    /// <summary>
    /// Contenedor de propiedades y bloques de propiedades / 
    /// Property and property block container
    /// </summary>
    public interface IPropertyBlockContainer
    {
        /// <summary>
        /// Pasar el foco a la siguiente propiedad al terminar la edición / 
        /// Move focus to the next property when editing the edition
        /// </summary>
        bool AutoTab { get; set; }
        /// <summary>
        /// Configuración de encabezados de columna / 
        /// Column headers settings
        /// </summary>
        int HeadersHeight { get; set; }
        Font HeadersFont { get; set; }
        Color HeadersBackColor { get; set; }
        Color HeadersForeColor { get; set; }
        /// <summary>
        /// Fuente para los editores de propiedades / 
        /// Property editors font
        /// </summary>
        Font EditorsFont { get; set; }
        /// <summary>
        /// Color de fondo de los editores de propiedades / 
        /// Property editors background color
        /// </summary>
        Color EditorsBackColor { get; set; }
        /// <summary>
        /// Color de primer plano de los editores de propiedades / 
        /// Property editors foreground color
        /// </summary>
        Color EditorsForeColor { get; set; }
        /// <summary>
        /// Color de fondo del editor cuando está seleccionado / 
        /// Selected property editor background color
        /// </summary>
        Color EditorsSelectedBackColor { get; set; }
        /// <summary>
        /// Color de texto del editor cuando está seleccionado / 
        /// Selected property editor text color
        /// </summary>
        Color EditorsSelectedForeColor { get; set; }
        /// <summary>
        /// Color de los bordes de los editores de propiedades / 
        /// Property editors border colors
        /// </summary>
        Color EditorsTopBorderColor { get; set; }
        Color EditorsLeftBorderColor { get; set; }
        Color EditorsBottomBorderColor { get; set; }
        Color EditorsRightBorderColor { get; set; }
        /// <summary>
        /// Color de fondo de las propiedades desplegables / 
        /// Expandable properties background color
        /// </summary>
        Color ExpandableBackColor { get; set; }
        /// <summary>
        /// Color de primer plano de las propiedades desplegables / 
        /// Expandable properties foreground color
        /// </summary>
        Color ExpandableForeColor { get; set; }
        /// <summary>
        /// Tamaño del contorno de los editores de propiedades / 
        /// Property editors surround size
        /// </summary>
        Padding EditorsDefaultPadding { get; set; }
        /// <summary>
        /// Tamaño del borde de los editores de propiedades / 
        /// Property editors border size
        /// </summary>
        Padding EditorsBorderSize { get; set; }
        /// <summary>
        /// Estilo por defecto de los editores de propiedades / 
        /// Property editors default style
        /// </summary>
        PropertyEditionStyle EditionStyle { get; set; }
        /// <summary>
        /// Respositorio de recursos incrustados / 
        /// Embedded resources repository
        /// </summary>
        ResourcesRepository AllResources { get; set; }
        /// <summary>
        /// Contenedor de nivel superior / 
        /// High level container
        /// </summary>
        IPropertyBlockContainer ParentContainer { get; set; }
        /// <summary>
        /// Contenedor de nivel inferior / 
        /// Low level container
        /// </summary>
        /// <param name="pname">
        /// Nombre de la propiedad buscada / 
        /// Property searched name 
        /// </param>
        /// <param name="pi">
        /// Información de la propiedad / 
        /// Property information
        /// </param>
        IPropertyBlockContainer GetChildContainer(string pname, out PropertyInfo pi);
        /// <summary>
        /// Columna del contenedor / 
        /// FOContainer column
        /// </summary>
        PropertyTableColumn AsColumn { get; }
        /// <summary>
        /// Alguna de las propiedades ha cambiado / 
        /// Some property has changed
        /// </summary>
        /// <param name="editor">
        /// Editor de la propiedad / 
        /// Property editor
        /// </param>
        /// <param name="oldvalue">
        /// Valor de la propiedad antes del cambio / 
        /// Property value before change
        /// </param>
        void PropertyChanged(PropertyEditorBase editor, object oldvalue);
        /// <summary>
        /// Alguna de las propiedades ha recibido el foco / 
        /// Some property has got focus
        /// </summary>
        /// <param name="editor">
        /// Editor de la propiedad / 
        /// Property editor
        /// </param>
        void PropertyGotFocus(PropertyEditorBase editor);
        /// <summary>
        /// Alguna de las propiedades ha perdido el foco / 
        /// Some property has lost focus
        /// </summary>
        /// <param name="editor">
        /// Editor de la propiedad / 
        /// Property editor
        /// </param>
        void PropertyLostFocus(PropertyEditorBase editor);
        /// <summary>
        /// La edición de la propiedad ha finalizado. Pasar el foco a la siguiente / 
        /// Property edition has finished. Move focus to the next property
        /// </summary>
        /// <param name="editor">
        /// Editor que ha finalizado la edición / 
        /// Finioshed editor
        /// </param>
        /// <param name="colindex">
        /// Índice de la columna / 
        /// Column index
        /// </param>
        /// <param name="rowindex">
        /// Índice de la fila / 
        /// Row index
        /// </param>
        void PropertyCommited(PropertyEditorBase editor, int colindex = 0, int rowindex = 0);
        /// <summary>
        /// El contenedor debe colapsar o expandir su contenido / 
        /// The container must collapse or expand its content
        /// </summary>
        /// <param name="collapse">
        /// true para colapsar contenido / 
        /// true to collapse content
        /// </param>
        void Collapse(bool collapse);
        /// <summary>
        /// Alguno de los elementos ha cambiado de tamaño / 
        /// Some element size has changed
        /// </summary>
        /// <param name="changedItem">
        /// Item que ha cambiado de tamaño / 
        /// Resized item
        /// </param>
        /// <param name="reason">
        /// Razón del cambio de tamaño / 
        /// Size change reason
        /// </param>
        void ContentSizeChanged(object changedItem, PropertyEditorChangedReason reason);
    }
}
