using BaseClassesAndInterfaces.Interfaces;
using BaseClassesAndInterfaces.SQL;
using BaseClassesAndInterfaces.UserInterface;
using System;

namespace AIChessDatabase.Query
{
    /// <summary>
    /// Query filters and order container to modify a query.
    /// </summary>
    [Serializable]
    public class DataFilter
    {
        public DataFilter()
        {
        }
        /// <summary>
        /// Where clause filters for the query.
        /// </summary>
        public UIFilterExpression WFilter { get; set; }
        /// <summary>
        /// Having clause filters for the query.
        /// </summary>
        public UIFilterExpression HFilter { get; set; }
        /// <summary>
        /// Gets or sets the order-by expression used for sorting UI elements.
        /// </summary>
        public UIOrderByExpression OExpr { get; set; }
        /// <summary>
        /// Select distinct (or all) records in the query.
        /// </summary>
        public SQLExpression SetQ { get; set; }
        /// <summary>
        /// Query to apply the filter to. Optional.
        /// </summary>
        public ISQLUIQuery Query { get; set; }
    }
}
