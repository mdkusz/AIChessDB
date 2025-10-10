using AIChessDatabase.Data;
using BaseClassesAndInterfaces.Interfaces;
using System.Data;
using System.Threading.Tasks;

namespace AIChessDatabase.Interfaces
{
    /// <summary>
    /// Central repository for database objects.
    /// </summary>
    public interface IObjectRepository : IGenericObjectRepository<ObjectBase>
    {
        /// <summary>
        /// List of non-standard queries, database dependant, for this repository.
        /// </summary>
        NonStandardQueries Queries { get; set; }
        /// <summary>
        /// Run a query with a specific timeout.
        /// </summary>
        /// <param name="query">
        /// Compiled query to execute.
        /// </param>
        /// <param name="connection">
        /// Connection index to execute the query.
        /// </param>
        /// <returns>
        /// DataTable with the results of the query.
        /// </returns>
        Task<DataTable> GlobalQuery(ISQLUIQuery query, int connection);
    }
}
