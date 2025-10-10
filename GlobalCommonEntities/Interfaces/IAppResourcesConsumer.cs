using GlobalCommonEntities.DependencyInjection;

namespace GlobalCommonEntities.Interfaces
{
    /// <summary>
    /// Object that uses application embedded resources
    /// </summary>
    public interface IAppResourcesConsumer
    {
        /// <summary>
        /// Application embedded resources repository
        /// </summary>
        ResourcesRepository AllResources { get; set; }
    }
}
