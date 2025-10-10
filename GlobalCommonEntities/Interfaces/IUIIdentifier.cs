namespace GlobalCommonEntities.Interfaces
{
    /// <summary>
    /// Generic object identifier and creator.
    /// </summary>
    public interface IUIIdentifier : IFriendlyIdentifier
    {
        /// <summary>
        /// Object implementation without parameters
        /// </summary>
        /// <returns>
        /// Object instance
        /// </returns>
        object Implementation();
        /// <summary>
        /// Parameterized object implementation
        /// </summary>
        /// <param name="p">
        /// parameter array
        /// </param>
        /// <returns>
        /// Initialized object
        /// </returns>
        object ParamImplementation(params object[] p);
        /// <summary>
        /// Force ToString implementation
        /// </summary>
        /// <returns>
        /// User interface textual representation
        /// </returns>
        string ToString();
    }
}
