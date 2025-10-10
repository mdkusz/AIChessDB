using System;

namespace GlobalCommonEntities.Attributes
{
    /// <summary>
    /// Use this attribute to decorate a POCO class with a specific database table name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class TableNameAttribute : Attribute
    {
        public string TableName { get; private set; }
        public TableNameAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}
