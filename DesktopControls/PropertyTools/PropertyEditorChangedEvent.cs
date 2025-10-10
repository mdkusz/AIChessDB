using System;

namespace DesktopControls.PropertyTools
{
    /// <summary>
    /// Razones por las que un editor de propiedades ha cambiado / 
    /// Reasons why a property editor has changed
    /// </summary>
    public enum PropertyEditorChangedReason
    {
        Resized,
        ValueChanged,
        Expanded,
        Collapsed
    }
    /// <summary>
    /// Argumentos del evento de cambio de editor de propiedades / 
    /// Property editor changed event arguments
    /// </summary>
    public class PropertyEditorChangedEventArgs : EventArgs
    {
        public PropertyEditorChangedEventArgs(PropertyEditorChangedReason reason, object item)
        {
            Reason = reason;
            Item = item;
        }
        /// <summary>
        /// Razón del cambio de tamaño / 
        /// Resize reason
        /// </summary>
        public PropertyEditorChangedReason Reason { get; private set; }
        /// <summary>
        /// Item que ha cambiado de tamaño / 
        /// Resized item
        /// </summary>
        public object Item { get; private set; }
    }
    /// <summary>
    /// Delegado para el evento de cambio de editor de propiedades / 
    /// Property editor changed event delegate
    /// </summary>
    /// <param name="sender">
    /// Objeto que ha disparado el evento / 
    /// Object that has fired the event
    /// </param>
    /// <param name="e">
    /// Argumentos del evento / 
    /// Event arguments
    /// </param>
    public delegate void PropertyEditorChangedEventHandler(object sender, PropertyEditorChangedEventArgs e);
}
