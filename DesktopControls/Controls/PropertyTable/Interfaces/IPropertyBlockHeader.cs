namespace DesktopControls.Controls.PropertyTable.Interfaces
{
    /// <summary>
    /// Cabecera de un bloque de propiedades / 
    /// Property block header
    /// </summary>
    public interface IPropertyBlockHeader
    {
        /// <summary>
        /// Título de la cabecera / 
        /// Header title
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// Colapsar o expandir el contenido del bloque / 
        /// Collapse or expand block contents
        /// </summary>
        bool Collapsed { get; set; }
        /// <summary>
        /// Bloque contenedor de la cabecera / 
        /// Header block container
        /// </summary>
        IPropertyBlockContainer ParentContainer { get; set; }
    }
}
