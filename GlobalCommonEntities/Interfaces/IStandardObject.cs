using System;

namespace GlobalCommonEntities.Interfaces
{
    /// <summary>
    /// Interface for objects with unique identifier, name and description
    /// </summary>
    /// <remarks>
    /// This is another interface to standardize objects with unique identifier, name, description and UIDataSheet configuration.
    /// </remarks>
    /// <seealso cref="UIDataSheet"/>
    /// <descendant>IFileStandardObject</descendant>
    public interface IStandardObject
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        string StdUID { get; }
        /// <summary>
        /// Element name
        /// </summary>
        string StdName { get; }
        /// <summary>
        /// Element description
        /// </summary>
        string StdDescription { get; }
        /// <summary>
        /// Information to edit the object
        /// </summary>
        IUIDataSheet DataSheet { get; }
    }
    /// <summary>
    /// IStandardObject interface extension for files.
    /// </summary>
    /// <remarks>
    /// IStandardObject interface extension for file objects.
    /// </remarks>
    public interface IFileStandardObject : IStandardObject
    {
        /// <summary>
        /// File size
        /// </summary>
        long StdFileSize { get; }
        /// <summary>
        /// File creation date
        /// </summary>
        DateTime StdFileDate { get; }
    }
}
