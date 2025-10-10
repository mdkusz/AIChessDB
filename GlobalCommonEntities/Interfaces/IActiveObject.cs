namespace GlobalCommonEntities.Interfaces
{
    /// <summary>
    /// Object with active and inactive states
    /// </summary>
    public interface IActiveObject
    {
        /// <summary>
        /// Activation state
        /// </summary>
        bool Active { get; set; }
        /// <summary>
        /// Selection state
        /// </summary>
        bool Selected { get; set; }
        /// <summary>
        /// Checked state
        /// </summary>
        bool? Checked { get; set; }
    }
}
