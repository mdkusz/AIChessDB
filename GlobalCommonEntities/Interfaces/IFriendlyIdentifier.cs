namespace GlobalCommonEntities.Interfaces
{
    /// <summary>
    /// Generic object identifier with name, description and category
    /// </summary>
    public interface IFriendlyIdentifier
    {
        /// <summary>
        /// Element name
        /// </summary>
        string FriendlyName { get; set; }
        /// <summary>
        /// Element description
        /// </summary>
        string FriendlyDescription { get; set; }
        /// <summary>
        /// Element category
        /// </summary>
        string UICategory { get; set; }
    }
}
