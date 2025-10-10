using DesktopControls.Controls;
using DesktopControls.Controls.InputEditors;
using GlobalCommonEntities.DependencyInjection;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System.Collections.Generic;
using System.Linq;
using static DesktopControls.Properties.UIResources;

namespace DesktopControls
{
    /// <summary>
    /// Dependency provider for GlobalCommonEntities
    /// </summary>
    public class ObjectProvider : IDependencyProvider
    {
        private List<string> _services = new List<string> { nameof(IInputEditorFactory),
            nameof(IUIElementInteractor), nameof(IUIRelevantElementCollector) };
        public ObjectProvider() { }
        /// <summary>
        /// IDependencyProvider: Central repository of embedded resources
        /// </summary>
        public ResourcesRepository AllResources { get; set; }
        /// <summary>
        /// IDependencyProvider: Check if a given class or interface is supported
        /// </summary>
        /// <param type="services">
        /// Type names separated by semicolon
        /// </param>
        /// <returns>
        /// True if can instantiante objets of at least one of the given types
        /// </returns>
        public bool QueryClassOrInterface(string services)
        {
            List<string> lservices = new List<string>(services.Split(';'));
            return lservices.Any(s => _services.Contains(s));
        }
        /// <summary>
        /// IDependencyProvider: Get all the instances of all the rquested types or interfaces
        /// </summary>
        /// <param type="services">
        /// Semicolon separated type type list
        /// </param>
        /// <param type="filter">
        /// Object containing a filter to select elements
        /// </param>
        /// <returns>
        /// Enumeration of objects implementing the types as IUIIdentifier objects
        /// </returns>
        public IEnumerable<IUIIdentifier> GetObjects(string services, object filter = null)
        {
            List<string> lservices = new List<string>(services.Split(';'));
            foreach (string service in lservices)
            {
                if (service == nameof(IInputEditorFactory))
                {
                    IUIDataSheet iesheet = filter as IUIDataSheet;
                    if (iesheet == null)
                    {
                        yield return new ObjectWrapper(new InputEditorFactory(), NAME_InputEditorFactory, "");
                    }
                    else
                    {
                        InputEditorFactory qief = new InputEditorFactory();
                        foreach (PropertyEditorInfo pi in iesheet.Properties)
                        {
                            if (!qief.AcceptsEditorType(pi.EditorType))
                            {
                                qief = null;
                                yield break;
                            }
                        }
                        if (qief != null)
                        {
                            yield return new ObjectWrapper(qief, NAME_InputEditorFactory, "");
                        }
                    }
                }
                else if (service == nameof(IUIElementInteractor))
                {
                    yield return new ObjectWrapper(new ControlInteractor(), NAME_ControlInteractor, "");
                }
                else if (service == nameof(IUIRelevantElementCollector))
                {
                    yield return new ObjectWrapper(new RelevantControlCollector(), NAME_RelevantControlCollector, "");
                }
            }
        }
    }
}
