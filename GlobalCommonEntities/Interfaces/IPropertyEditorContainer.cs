namespace GlobalCommonEntities.Interfaces
{
    /// <summary>
    /// Interfaz para unificar el acceso a la propiedad SelectedObject de editores de propiedades / 
    /// Interface to unify the SelectedObject property of property editors
    /// </summary>
    public interface IPropertyEditorContainer
    {
        /// <summary>
        /// Objeto seleccionado en el editor / 
        /// Editor selected object
        /// </summary>
        object SelectedObject { get; set; }
    }
}
