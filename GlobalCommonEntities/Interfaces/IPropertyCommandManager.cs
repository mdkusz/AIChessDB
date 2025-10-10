using System;

namespace GlobalCommonEntities.Interfaces
{
    /// <summary>
    /// Argumentos de evento de comando de editor de propiedades / 
    /// Property editor command event arguments
    /// </summary>
    public class PropertyCommandEventArgs : EventArgs
    {
        public PropertyCommandEventArgs()
        {
            Cancel = false;
        }
        /// <summary>
        /// Nombre de la propiedad / 
        /// Property name
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// Índice del botón de comando / 
        /// Command button index
        /// </summary>
        public int CommandIndex { get; set; }
        /// <summary>
        /// Objeto creado como resultado de un comando / 
        /// Object created as command result
        /// </summary>
        public object CommandResult { get; set; }
        /// <summary>
        /// Información extra sobre el resultado / 
        /// Result extra information
        /// </summary>
        public string ExtraData { get; set; }
        /// <summary>
        /// El comando se ha cancelado / 
        /// Command cancelled
        /// </summary>
        public bool Cancel { get; set; }
    }
    /// <summary>
    /// Delegado para invocar eventos de comando de editor de propiedades / 
    /// Delegate to invoke property editor command events
    /// </summary>
    /// <param name="sender">
    /// Objeto que dispara el evento / 
    /// Object that triggers the event
    /// </param>
    /// <param name="e">
    /// Argumentos del evento / 
    /// Event arguments
    /// </param>
    public delegate void PropertyCommandEventHandler(object sender, PropertyCommandEventArgs e);
    /// <summary>
    /// Gestor de comandos de editor de propiedades / 
    /// Property editor command manager
    /// </summary>
    public interface IPropertyCommandManager
    {
        /// <summary>
        /// El valor se puede cambia manualmente o solo desde el cuadro de diálogo modal / 
        /// The value can be changed manually or only from the modal dialog box
        /// </summary>
        /// <param name="propertyName">
        /// Nombre de la propiedad / 
        /// Property name
        /// </param>
        bool TextReadOnly(string propertyName);
        /// <summary>
        /// Invocación de comando / 
        /// Command invocation
        /// </summary>
        /// <param name="sender">
        /// Objeto que dispara el evento / 
        /// Object that triggers the event
        /// </param>
        /// <param name="e">
        /// Argumentos del evento / 
        /// Event arguments
        /// </param>
        void PropertyCommand(object sender, PropertyCommandEventArgs e);
        /// <summary>
        /// Evento para solicitar la creación de un objeto / 
        /// Event to ask for a new object
        /// </summary>
        event PropertyCommandEventHandler ObjectNeeded;
    }
}
