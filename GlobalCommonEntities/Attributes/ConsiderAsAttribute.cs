using System;

namespace GlobalCommonEntities.Attributes
{
    /// <summary>
    /// Use this attribute to indicate that a class or struct can be considered as another type for certain operations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class ConsiderAsAttribute : Attribute
    {
        public Type ConsideredType { get; }
        public ConsiderAsAttribute(Type consideredType)
        {
            ConsideredType = consideredType;
        }
    }
}
