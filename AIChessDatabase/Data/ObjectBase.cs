using AIChessDatabase.Interfaces;
using AIChessDatabase.Query;
using BaseClassesAndInterfaces.DML;
using BaseClassesAndInterfaces.Interfaces;
using BaseClassesAndInterfaces.SQL;
using BaseClassesAndInterfaces.UserInterface;
using GlobalCommonEntities.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AIChessDatabase.Data
{
    /// <summary>
    /// Base class for all objects in the database.
    /// </summary>
    /// <remarks>
    /// Each object represents a record in a database table, and can hace child objects.
    /// </remarks>
    [Serializable]
    public abstract class ObjectBase : IEquatable<ObjectBase>
    {
        [NonSerialized]
        protected IObjectRepository _repository = null;
        [NonSerialized]
        protected string _querySQL = null;
        protected string _queryCountSQL = null;
        protected string _parameterPrefix = null;

        public ObjectBase() { }
        public ObjectBase(IObjectRepository rep)
        {
            Repository = rep;
        }
        public ObjectBase(IObjectRepository rep, ObjectBase copy)
        {
            Repository = rep;
        }
        /// <summary>
        /// Database repository where the object is stored.
        /// </summary>
        [JsonIgnore]
        public virtual IObjectRepository Repository
        {
            get
            {
                return _repository;
            }
            set
            {
                _repository = value;
                _parameterPrefix = _repository?.ParameterPrefix;
            }
        }
        /// <summary>
        /// Get the SQL query used to retrieve the object.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        /// <param name="clone">
        /// Return a cloned query that can be modified without affecting the original query in the repository.
        /// </param>
        /// <returns>
        /// Specialized SQL query object for UI operations with advanced filtering capabilities.
        /// </returns>
        public ISQLUIQuery ObjectQuery(int connection = 0, bool clone = true)
        {
            return GetQuery(connection, clone);
        }
        /// <summary>
        /// Object primary key values array. All components of primary keys are numbers, as all tables have surrogates.
        /// </summary>
        public abstract ulong[] Key { get; }
        /// <summary>
        /// Set the primary key values of the object.
        /// </summary>
        /// <param name="key">
        /// Array of primary key values. The order of the values must match the order of the primary key components.
        /// </param>
        public abstract void SetKey(ulong[] key);
        /// <summary>
        /// Retrieve the object from the database using its primary key (asynchronous version).
        /// </summary>
        /// <param name="key">
        /// Primary key values array. When there is more than one component, the order matters.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        public abstract Task GetAsync(ulong[] key, int connection = 0);
        /// <summary>
        /// Retrieve the object from the database using its primary key.
        /// </summary>
        /// <param name="key">
        /// Primary key values array. When there is more than one component, the order matters.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        public virtual void Get(ulong[] key, int connection = 0) { }
        /// <summary>
        /// Use a DataRow to fill the object properties.
        /// </summary>
        /// <param name="row">
        /// DataRow to use to fill the object properties. The row must have been executed with a query that returns the object properties.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        public virtual void Get(DataRow row, int connection)
        {
            foreach (PropertyInfo pi in GetType().GetProperties())
            {
                TableColumnNameAttribute pname = pi.GetCustomAttribute<TableColumnNameAttribute>();
                if (pi.CanWrite && (pname != null) && row.Table.Columns.Contains(pname.Name))
                {
                    if (pi.PropertyType.IsSubclassOf(typeof(ObjectBase)))
                    {
                        ObjectBase child = _repository.CreateObject(pi.PropertyType);
                        child.GetInnerObject(Convert.ToUInt64(row[pname.Name]), pname.Name, connection);
                        pi.SetValue(this, child);
                    }
                    else
                    {
                        object value = row[pname.Name];
                        try
                        {
                            if (pi.PropertyType == typeof(string))
                            {
                                pi.SetValue(this, value?.ToString());
                            }
                            else if (pi.PropertyType == typeof(DateTime))
                            {
                                if (value == DBNull.Value)
                                {
                                    pi.SetValue(this, DateTime.MinValue);
                                }
                                else
                                {
                                    pi.SetValue(this, Convert.ToDateTime(value));
                                }
                            }
                            else
                            {
                                pi.SetValue(this, Convert.ChangeType(value, pi.PropertyType));
                            }
                        }
                        catch { }
                    }
                }
            }
        }
        /// <summary>
        /// Use a DataRow to fill the object properties without querying the database.
        /// </summary>
        /// <param name="row">
        /// DataRow to use to fill the object properties. The row must have been executed with a query that returns the object properties.
        /// </param>
        public virtual void Get(DataRow row) { }
        /// <summary>
        /// Retrieve all child objects of this object. This method is called after the object has been retrieved from the database.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        public virtual async Task GetChildrenAsync(int connection = 0) { await Task.Yield(); }
        /// <summary>
        /// Retrieve all child objects of this object. This method is called after the object has been retrieved from the database.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        public virtual void GetChildren(int connection = 0) { }
        /// <summary>
        /// Get all objects of this type from the database, applying the specified filter.
        /// </summary>
        /// <param name="filter">
        /// DataFilter to apply to the query. If null, no filter is applied.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        /// <returns>
        /// List of objects that match the filter.
        /// </returns>
        public virtual async Task<List<ObjectBase>> GetAllAsync(DataFilter filter, int connection = 0)
        {
            ISQLUIQuery query = filter.Query ?? GetQuery(connection);
            query.UserWhereFilters = null;
            query.UserHavingFilters = null;
            query.UserOrderBy = null;
            if (filter != null)
            {
                if (filter.WFilter != null)
                {
                    query.AddFilter(filter.WFilter);
                }
                if (filter.HFilter != null)
                {
                    query.AddFilter(filter.HFilter);
                }
                if (filter.OExpr != null)
                {
                    query.AddOrder(filter.OExpr);
                }
                if (filter.SetQ != null)
                {
                    query.SetQuantifier = filter.SetQ;
                }
            }
            List<ObjectBase> list = new List<ObjectBase>();
            using (DataTable dt = await _repository.Connector.ExecuteTableAsync(query, null, null, connection))
            {
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ObjectBase obj = _repository.CreateObject(GetType());
                        obj.Get(row, connection);
                        await obj.GetChildrenAsync(connection);
                        list.Add(obj);
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// Get all objects of this type from the database, applying the specified filter.
        /// </summary>
        /// <param name="filter">
        /// DataFilter to apply to the query. If null, no filter is applied.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        /// <returns>
        /// List of objects that match the filter.
        /// </returns>
        public virtual List<ObjectBase> GetAll(DataFilter filter, int connection = 0)
        {
            ISQLUIQuery query = filter.Query ?? GetQuery(connection);
            query.UserWhereFilters = null;
            query.UserHavingFilters = null;
            query.UserOrderBy = null;
            if (filter != null)
            {
                if (filter.WFilter != null)
                {
                    query.AddFilter(filter.WFilter);
                }
                if (filter.HFilter != null)
                {
                    query.AddFilter(filter.HFilter);
                }
                if (filter.OExpr != null)
                {
                    query.AddOrder(filter.OExpr);
                }
                if (filter.SetQ != null)
                {
                    query.SetQuantifier = filter.SetQ;
                }
            }
            List<ObjectBase> list = new List<ObjectBase>();
            using (DataTable dt = _repository.Connector.ExecuteTable(query, connection))
            {
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ObjectBase obj = _repository.CreateObject(GetType());
                        obj.Get(row, connection);
                        obj.GetChildren(connection);
                        list.Add(obj);
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// Perform the custom query for this object type using the specified filter.
        /// </summary>
        /// <param name="filter">
        /// DataFilter to apply to the query. If null, no filter is applied.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        /// <returns>
        /// DataTable with the results of the query.
        /// </returns>
        public virtual async Task<DataTable> Query(DataFilter filter, int connection = 0)
        {
            ISQLUIQuery query = filter.Query ?? GetQuery(connection);
            query.UserWhereFilters = null;
            query.UserHavingFilters = null;
            query.UserOrderBy = null;
            if (filter != null)
            {
                if (filter.WFilter != null)
                {
                    query.AddFilter(filter.WFilter);
                }
                if (filter.HFilter != null)
                {
                    query.AddFilter(filter.HFilter);
                }
                if (filter.OExpr != null)
                {
                    query.AddOrder(filter.OExpr);
                }
                if (filter.SetQ != null)
                {
                    query.SetQuantifier = filter.SetQ;
                }
            }
            return await _repository.Connector.ExecuteTableAsync(query, null, null, connection);
        }
        /// <summary>
        /// Get all child objects of a specific type for this object, applying the specified filter.
        /// </summary>
        /// <param name="ChildType">
        /// Type of child objects to retrieve. The type must be a subclass of ObjectBase.
        /// </param>
        /// <param name="filter">
        /// DataFilter to apply to the query. If null, no filter is applied.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        /// <returns>
        /// List of child objects that match the filter. If no children are found, an empty list is returned.
        /// </returns>
        public virtual async Task<List<ObjectBase>> GetAllChildren(Type ChildType, DataFilter filter, int connection = 0) { await Task.Yield(); return null; }
        /// <summary>
        /// Insert the object into the database. If the object has a primary key with a surrogate, it will be automatically generated.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        public virtual async Task Insert(int connection = 0)
        {
            await LoopInsert(connection, null);
        }
        /// <summary>
        /// Insert for descendant objects that need to re-use the cloned query object.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        /// <param name="query">
        /// Cloned query to re-use. If null, a new query will be retrieved from the repository.
        /// </param>
        /// <returns>
        /// The query used to insert the object to re-use in other objects.
        /// </returns>
        public virtual async Task<ISQLUIQuery> LoopInsert(int connection, ISQLUIQuery query)
        {
            if (query == null)
            {
                query = GetQuery(connection);
            }
            if (query != null)
            {
                Table bdtabla = query.Tables[0] as Table;
                DMLCommand cmd = bdtabla.InsertCommand;
                foreach (SQLParameter p in cmd.Parameters)
                {
                    p.DefaultValue = GetFieldValue(p.Name);
                }
                await cmd.ExecuteAsync(_repository.Connector, connection);
                if ((Key.Length == 1) && (Key[0] == 0))
                {
                    SetKey(new ulong[] { await cmd.AutogeneratedKey(_repository.Connector, connection) });
                }
            }
            return query;
        }
        /// <summary>
        /// Update the object in the database. The object must have been previously inserted, and its primary key must be set.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        public async virtual Task Update(int connection = 0)
        {
            await LoopUpdate(connection, null);
        }
        /// <summary>
        /// Update for descendant objects that need to re-use the cloned query object.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        /// <param name="query">
        /// Cloned query to re-use. If null, a new query will be retrieved from the repository.
        /// </param>
        /// <returns>
        /// The query used to insert the object to re-use in other objects.
        /// </returns>
        public virtual async Task<ISQLUIQuery> LoopUpdate(int connection, ISQLUIQuery query)
        {
            if (query == null)
            {
                query = GetQuery(connection);
            }
            if (query != null)
            {
                Table bdtabla = query.Tables[0] as Table;
                DMLCommand cmd = bdtabla.UpdateCommand;
                foreach (SQLParameter p in cmd.Parameters)
                {
                    p.DefaultValue = GetFieldValue(p.Name);
                }
                await cmd.ExecuteAsync(_repository.Connector, connection);
            }
            return query;
        }
        /// <summary>
        /// Delete the object from the database. The object must have been previously inserted, and its primary key must be set.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        public async virtual Task Delete(int connection = 0)
        {
            await LoopDelete(connection, null);
        }
        /// <summary>
        /// Delete for descendant objects that need to re-use the cloned query object.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        /// <param name="query">
        /// Cloned query to re-use. If null, a new query will be retrieved from the repository.
        /// </param>
        /// <returns>
        /// The query used to insert the object to re-use in other objects.
        /// </returns>
        public virtual async Task<ISQLUIQuery> LoopDelete(int connection, ISQLUIQuery query)
        {
            if (query == null)
            {
                query = GetQuery(connection);
            }
            if (query != null)
            {
                Table bdtabla = query.Tables[0] as Table;
                DMLCommand cmd = bdtabla.DeleteCommand;
                foreach (SQLParameter p in cmd.Parameters)
                {
                    p.DefaultValue = GetFieldValue(p.Name);
                }
                await cmd.ExecuteAsync(_repository.Connector, connection);
            }
            return query;
        }
        /// <summary>
        /// Perform a query to count the number of records in the table represented by this object type.
        /// </summary>
        /// <param name="filter">
        /// DataFilter to apply to the query. If null, no filter is applied.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        /// <returns>
        /// Unsigned long with the number of records in the table. If no records are found, 0 is returned.
        /// </returns>
        public virtual async Task<ulong> GetCount(DataFilter filter = null, int connection = 0)
        {
            if (filter == null)
            {
                return Convert.ToUInt64(await _repository.Connector.ExecuteScalarAsync($"select count(*) from {GetType().GetCustomAttribute<TableNameAttribute>().TableName}", null, connection));
            }
            else
            {
                ISQLUIQuery query = filter.Query ?? GetCountQuery(connection);
                query.UserWhereFilters = null;
                query.UserHavingFilters = null;
                if (filter.WFilter != null)
                {
                    query.AddFilter(filter.WFilter);
                }
                if (filter.HFilter != null)
                {
                    query.AddFilter(filter.HFilter);
                }
                if (filter.OExpr != null)
                {
                    query.AddOrder(filter.OExpr);
                }
                if (filter.SetQ != null)
                {
                    query.SetQuantifier = filter.SetQ;
                }
                query.SetQuantifier = filter.SetQ;
                return Convert.ToUInt64(await _repository.Connector.ExecuteScalarAsync(query, connection));
            }
        }
        /// <summary>
        /// Add a child object to this object. The child object must be a subclass of ObjectBase.
        /// </summary>
        /// <param name="child">
        /// Child object to add. The child object must have a primary key with a surrogate, which will be automatically generated when the child is inserted.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections. Default is 0.
        /// </param>
        public virtual void AddChild(ObjectBase child, int connection = 0) { }
        /// <summary>
        /// Add a child object to this object asynchronously. The child object must be a subclass of ObjectBase.
        /// </summary>
        /// <param name="child">
        /// Child object to add. The child object must have a primary key with a surrogate, which will be automatically generated when the child is inserted.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections. Default is 0.
        /// </param>
        public virtual async Task AddChildAsync(ObjectBase child, int connection = 0)
        {
            await Task.Yield();
        }
        /// <summary>
        /// Remove a child object from this object. The child object must be a subclass of ObjectBase.
        /// </summary>
        /// <param name="child">
        /// Child object to remove. The child object must have a primary key with a surrogate, which will be automatically generated when the child is inserted.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections. Default is 0.
        /// </param>
        public virtual async Task RemoveChildAsync(ObjectBase child, int connection = 0)
        {
            await Task.Yield();
        }
        /// <summary>
        /// Remove a child object from this object. The child object must be a subclass of ObjectBase.
        /// </summary>
        /// <param name="child">
        /// Child object to remove. The child object must have a primary key with a surrogate, which will be automatically generated when the child is inserted.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections. Default is 0.
        /// </param>
        public virtual void RemoveChild(ObjectBase child, int connection = 0)
        {
        }
        /// <summary>
        /// Get the inner object that corresponds to a given field name and its value in the database.
        /// </summary>
        /// <param name="key">
        /// Surrogate key value.
        /// </param>
        /// <param name="pname">
        /// Field name in the IDataReader that contains the primary key value of the inner object.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        protected virtual void GetInnerObject(ulong key, string pname, int connection = 0)
        {
        }
        /// <summary>
        /// Internal method to retrieve the object from the database using its primary key and a filter expression.
        /// </summary>
        /// <param name="key">
        /// Primary key values array. When there is more than one component, the order matters.
        /// </param>
        /// <param name="filter">
        /// DataFilter to apply to the query. If null, no filter is applied.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        protected virtual async Task InternalGetAsync(ulong[] key, string filter, int connection = 0)
        {
            ISQLUIQuery query = GetQuery(connection);
            try
            {
                query.UserWhereFilters = null;
                ISQLElementBuilder sbquery = query as ISQLElementBuilder;
                SQLElement efilter = sbquery.BuildExpression(ref filter);
                SQLExpression expr = efilter as SQLExpression;
                if (expr != null)
                {
                    UIFilterExpression fexpr = new UIFilterExpression();
                    fexpr.SetElement(expr);
                    query.AddFilter(fexpr);
                    for (int ix = 0; ix < key.Length; ix++)
                    {
                        query.Parameters[ix].DefaultValue = key[ix];
                    }
                    DataTable dt = await _repository.Connector.ExecuteTableAsync(query, null, null, connection);
                    Get(dt.Rows[0], connection);
                    query.RemoveFilter(fexpr);
                }
            }
            finally
            {
                await GetChildrenAsync(connection);
            }
        }
        /// <summary>
        /// Internal method to retrieve the object from the database using its primary key and a filter expression.
        /// </summary>
        /// <param name="key">
        /// Primary key values array. When there is more than one component, the order matters.
        /// </param>
        /// <param name="filter">
        /// DataFilter to apply to the query. If null, no filter is applied.
        /// </param>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        protected virtual void InternalGet(ulong[] key, string filter, int connection = 0)
        {
            ISQLUIQuery query = GetQuery(connection);
            try
            {
                query.UserWhereFilters = null;
                ISQLElementBuilder sbquery = query as ISQLElementBuilder;
                SQLElement efilter = sbquery.BuildExpression(ref filter);
                SQLExpression expr = efilter as SQLExpression;
                if (expr != null)
                {
                    UIFilterExpression fexpr = new UIFilterExpression();
                    fexpr.SetElement(expr);
                    query.AddFilter(fexpr);
                    for (int ix = 0; ix < key.Length; ix++)
                    {
                        query.Parameters[ix].DefaultValue = key[ix];
                    }
                    DataTable dt = _repository.Connector.ExecuteTable(query, connection);
                    Get(dt.Rows[0], connection);
                    query.RemoveFilter(fexpr);
                }
            }
            finally
            {
                GetChildren(connection);
            }
        }
        /// <summary>
        /// Build the SQL query to retrieve the object from the database.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        /// <param name="clone">
        /// Return a cloned query that can be modified without affecting the original query in the repository.
        /// </param>
        /// <returns>
        /// Specialized SQL query object for UI operations with advanced filtering capabilities.
        /// </returns>
        /// <remarks>
        /// Think that this is a multi-threaded environment, and the query object can be modified by other threads. 
        /// Therefore, it is recommended to always use a cloned query when performing operations that modify the query.
        /// </remarks>
        protected virtual ISQLUIQuery GetQuery(int connection = 0, bool clone = true)
        {
            ISQLUIQuery query = Repository.GetQueryForType(GetType().Name, clone);
            if (query == null)
            {
                string sql = _querySQL;
                query = Repository.SetQueryForType(GetType().Name, sql, connection);
                ConfigureQuery(query);
                if (clone)
                {
                    query = Repository.GetQueryForType(GetType().Name, true);
                    query.Parser.ConnectionIndex = connection;
                }
            }
            return query;
        }
        /// <summary>
        /// Build the SQL query to count the number of records in the table represented by this object type.
        /// </summary>
        /// <param name="connection">
        /// Connection index to use for the query, in case of multiple connections.
        /// </param>
        /// <param name="clone">
        /// Return a cloned query that can be modified without affecting the original query in the repository.
        /// </param>
        /// <returns>
        /// Specialized SQL query object for UI operations with advanced filtering capabilities that counts the records.
        /// </returns>
        /// <remarks>
        /// Think that this is a multi-threaded environment, and the query object can be modified by other threads. 
        /// Therefore, it is recommended to always use a cloned query when performing operations that modify the query.
        /// </remarks>
        protected virtual ISQLUIQuery GetCountQuery(int connection = 0, bool clone = true)
        {
            ISQLUIQuery query = Repository.GetQueryForType(GetType().Name + ".Count", clone);
            if (query == null)
            {
                string sql = _querySQL;
                query = Repository.SetQueryForType(GetType().Name, sql, connection);
                if (clone)
                {
                    query = Repository.GetQueryForType(GetType().Name + ".Count", true);
                    query.Parser.ConnectionIndex = connection;
                }
            }
            return query;
        }
        /// <summary>
        /// Get a property of the object by its databse field name.
        /// </summary>
        /// <param name="fieldName">
        /// Database field name to search for. The field name is the name provided in the TableColumnNameAttribute.
        /// </param>
        /// <returns>
        /// PropertyInfo of the property that matches the field name, or null if no property is found.
        /// </returns>
        protected PropertyInfo GetProperty(string fieldName)
        {
            foreach (PropertyInfo pi in GetType().GetProperties())
            {
                TableColumnNameAttribute pname = pi.GetCustomAttribute<TableColumnNameAttribute>();
                if ((pname != null) && (string.Compare(pname.Name, fieldName, true) == 0))
                {
                    return pi;
                }
            }
            return null;
        }
        /// <summary>
        /// Retrieve the value of a field by its databse field name.
        /// </summary>
        /// <param name="fieldName">
        /// Name of the field to retrieve the value for. The field name is the name provided in the TableColumnNameAttribute.
        /// </param>
        /// <returns>
        /// Object with the value of the field. If the field is a child object, the value is the primary key of the child object. If the field is a DateTime, it returns DBNull.Value if the date is DateTime.MinValue.
        /// </returns>
        protected virtual object GetFieldValue(string fieldName)
        {
            PropertyInfo pi = GetProperty(fieldName);
            if (pi != null)
            {
                if (pi.PropertyType.IsSubclassOf(typeof(ObjectBase)))
                {
                    ObjectBase child = pi.GetValue(this) as ObjectBase;
                    return child.Key[0];
                }
                else if (pi.PropertyType == typeof(DateTime))
                {
                    DateTime dt = (DateTime)pi.GetValue(this);
                    if (dt == DateTime.MinValue)
                    {
                        return DBNull.Value;
                    }
                    else
                    {
                        return dt;
                    }
                }
                return pi.GetValue(this);
            }
            return null;
        }
        /// <summary>
        /// Create the UI configuration for the query columns based on the properties of the object decorated with custom attribute.
        /// </summary>
        /// <param name="query">
        /// Specialized SQL query object for UI operations with advanced filtering capabilities.
        /// </param>
        protected void ConfigureQuery(ISQLUIQuery query)
        {
            foreach (QueryColumn col in query.QueryColumns)
            {
                PropertyInfo pi = GetProperty(col.Name);
                if (pi != null)
                {
                    bool ro = pi.GetCustomAttribute<ReadOnlyAttribute>()?.IsReadOnly ?? false;
                    bool hid = pi.GetCustomAttribute<HiddenFieldAttribute>()?.Hidden ?? false;
                    bool vis = pi.GetCustomAttribute<VisibleFieldAttribute>()?.Visible ?? true;
                    bool locked = pi.GetCustomAttribute<LockedFieldAttribute>()?.Locked ?? false;
                    bool exp = pi.GetCustomAttribute<ExportFieldAttribute>()?.Export ?? true;
                    string colName = pi.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? col.Alias ?? col.Name;
                    string coldesc = pi.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "";
                    if (col.ColumnUIConfig == null)
                    {
                        col.ColumnUIConfig = new GridColumnConfiguration()
                        {
                            Caption = colName,
                            Description = coldesc,
                            ReadOnly = ro,
                            Locked = locked,
                            Visible = vis,
                            Hidden = hid,
                            Export = exp,
                            ColumnType = GridColumnType.Text
                        };
                    }
                }
                else if (col.ColumnUIConfig == null)
                {
                    col.ColumnUIConfig = new GridColumnConfiguration()
                    {
                        Caption = col.Alias ?? col.Name,
                        ColumnType = GridColumnType.Text
                    };
                }
            }
        }
        public override bool Equals(object obj)
        {
            if (obj is ObjectBase)
            {
                return Equals(obj as ObjectBase);
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public virtual bool Equals(ObjectBase other)
        {
            if (other.GetType() != GetType())
            {
                return false;
            }
            bool zero = false;
            for (int i = 0; i < Key.Length; i++)
            {
                if (Key[i] != other.Key[i])
                {
                    return false;
                }
                if (Key[i] == 0)
                {
                    zero = true;
                }
            }
            if (zero)
            {
                return base.Equals(other);
            }
            return true;
        }
    }
}
