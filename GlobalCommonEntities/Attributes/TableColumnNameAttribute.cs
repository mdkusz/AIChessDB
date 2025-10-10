using System;

namespace GlobalCommonEntities.Attributes
{
    /// <summary>
    /// Use this attribute to decorate a property with a specific database table column name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class TableColumnNameAttribute : Attribute
    {
        public string Name { get; private set; }
        public TableColumnNameAttribute(string name)
        {
            Name = name;
        }
    }
}
