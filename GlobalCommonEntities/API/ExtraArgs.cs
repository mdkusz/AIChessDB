using System;
using System.Collections.Generic;

namespace GlobalCommonEntities.API
{
    /// <summary>
    /// Extra arguments description
    /// </summary>
    /// <remarks>
    /// API endpoints may require extra url arguments to be passed. This class describes in a generic way the extra arguments required.
    /// You can use this class to provide parameters for any class method too.
    /// </remarks>
    public class ExtraArgs : IComparable<ExtraArgs>
    {
        /// <summary>
        /// Argument position
        /// </summary>
        /// <remarks>
        /// When required, the argument position is used to place the argument in the correct position in the argument list.
        /// </remarks>
        public int Position { get; set; }
        /// <summary>
        /// Argument description
        /// </summary>
        /// <remarks>
        /// Use this property to document the argument purpose and usage.
        /// </remarks>
        public string Description { get; set; }
        /// <summary>
        /// Argumento datatype
        /// </summary>
        /// <remarks>
        /// Argument data type. Use this property to validate the argument value.
        /// </remarks>
        public Type type { get; set; }
        /// <summary>
        /// Parameter name
        /// </summary>
        /// <remarks>
        /// Argument name. Use this property to build the url query string.
        /// </remarks>
        public string Name { get; set; }
        /// <summary>
        /// Argument values if restricted. Null if unrestricted.
        /// </summary>
        /// <remarks>
        /// You can provide a list of allowed values for the argument.
        /// </remarks>
        public List<object> Values { get; set; }
        /// <summary>
        /// Argument is optional
        /// </summary>
        public bool Optional { get; set; } = true;

        public int CompareTo(ExtraArgs other)
        {
            return Position.CompareTo(other.Position);
        }
    }
}
