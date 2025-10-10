using BaseClassesAndInterfaces.Interfaces;
using System.Collections.Generic;

namespace AIChessDatabase.Query
{
    /// <summary>
    /// Master - detail query container.
    /// </summary>
    public class MasterDetailQuery
    {
        /// <summary>
        /// Master query
        /// </summary>
        public ISQLUIQuery MasterQuery { get; set; }
        /// <summary>
        /// List of detail queries.
        /// </summary>
        public List<ISQLUIQuery> DetailQueries { get; set; }
    }
}
