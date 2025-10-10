using AIAssistants.Data;
using AIAssistants.Interfaces;
using GlobalCommonEntities.API;
using GlobalCommonEntities.DependencyInjection;
using GlobalCommonEntities.Interfaces;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace AIChessDatabase.AI
{
    /// <summary>
    /// Use this application to use the application IPlayer as an IAPIPlayer.
    /// </summary>
    public class ApplicationAPIElement : IPlayer, IAPIElement
    {
        private IPlayer _app;
        public ApplicationAPIElement(IPlayer app)
        {
            _app = app;
        }
        /// <summary>
        /// IPlayer: Player name
        /// </summary>
        public string PlayerName { get { return _app.PlayerName; } set { _app.PlayerName = value; } }
        /// <summary>
        /// IPlayer: Player role
        /// </summary>
        public GenericRole PlayerRole { get { return _app.PlayerRole; } }
        /// <summary>
        /// IPlayer: Message that will be sent on each request to remind the player important things
        /// </summary>
        public string Reminder { get { return _app.Reminder; } set { _app.Reminder = value; } }
        /// <summary>
        /// IPlayer: List of allowed application services identifiers
        /// </summary>
        public List<string> AllowedServices { get { return _app.AllowedServices; } set { _app.AllowedServices = value; } }
        /// <summary>
        /// IPlayer: Bubble header background color
        /// </summary>
        public Color BackColor { get { return _app.BackColor; } set { _app.BackColor = value; } }
        /// <summary>
        /// IPlayer: Bubble header foreground color
        /// </summary>
        public Color ForeColor { get { return _app.ForeColor; } set { _app.ForeColor = value; } }
        /// <summary>
        /// IPlayer: Console to show player messages
        /// </summary>
        public IMessagesConsole PrivateConsole { get { return _app.PrivateConsole; } set { _app.PrivateConsole = value; } }
        /// <summary>
        /// IPlayer: Specific player settings
        /// </summary>
        /// <remarks>
        /// This is an extended data sheet for specific player settings, if needed.
        /// </remarks>
        public IUIDataSheet PlayerSettings { get { return _app.PlayerSettings; } set { _app.PlayerSettings = value; } }
        public bool Equals(IPlayer other)
        {
            return _app.Equals(other);
        }
        /// <summary>
        /// IAPIElement: Unique identifier
        /// </summary>
        /// <remarks>
        /// All objects must have unique identifiers. This help to get objects from configuration files and provides an easy way to implement IEquatable.
        /// </remarks>
        public string Identifier { get; set; }
        /// <summary>
        /// IAPIElement: Element name
        /// </summary>
        /// <remarks>
        /// The element name helps users to identify the object in the configuration.
        /// </remarks>
        public string Name { get { return _app.PlayerName; } set { _app.PlayerName = value; } }
        /// <summary>
        /// IAPIElement: Element description
        /// </summary>
        /// <remarks>
        /// The desciption provides a brief explanation of the object purpose. It can be used in tooltips and help messages.
        /// </remarks>
        public string Description { get; set; }
        /// <summary>
        /// IAPIElement: Element generic type
        /// </summary>
        /// <remarks>
        /// This is the most representative generic interface of the object. It can be used to filter objects in the configuration.
        /// </remarks>
        public string GenericType { get; }
        /// <summary>
        /// IAPIElement: Element capabilities
        /// </summary>
        public ElementCapabilities Capabilities { get; }
        /// <summary>
        /// IAPIElement: API manager object
        /// </summary>
        /// <remarks>
        /// API elements are created by a given API manager. Each AI provider has its own API manager.
        /// </remarks>
        public IAPIManager APIManager { get; set; }
        /// <summary>
        /// IAPIElement: Information to edit the object
        /// </summary>
        /// <remarks>
        /// Definition of the custom properties that can be edited in the object. This is used to create the object editor.
        /// Those properties can be stored in a sepearted configuration file, or in the provider databse.
        /// </remarks>
        /// <seealso cref="APIUIDataSheet"/>
        public IAPIUIDataSheet EditorInfo { get; }
        /// <summary>
        /// IAPIElement: Information to edit the object configuration
        /// </summary>
        /// <remarks>
        /// This defines the common configuration properties for this object.
        /// Each AI provider defines its own configuration properties.
        /// </remarks>
        /// <seealso cref="APIUIDataSheet"/>
        public IAPIUIDataSheet ConfigInfo { get; }
        /// <summary>
        /// IAPIElement: Usage information of this element after the current request.
        /// </summary>
        /// <remarks>
        /// This is a dictionary with information useful to billing and limits.
        /// Each dictionary key is the name of anusage element field, such as input tokens, audio tokens, etc.
        /// Keys can be names of composite properties, such as "input_token_details.text_tokens".
        /// </remarks>
        public Dictionary<string, object> Usage { get; }
        /// <summary>
        /// IAPIElement: Query for function extra arguments
        /// </summary>
        /// <param name="functionName">
        /// Name of the function
        /// </param>
        /// <returns>
        /// List of extra arguments
        /// </returns>
        /// <remarks>
        /// Some standard API calls require extra url arguments. This method is used to query the extra arguments needed for a given function.
        /// The caller of the methods with arguments, should query first this method to provide or ask user for the required arguments.
        /// Usually, you can call those methods without arguments too, ant they will use default values for them.
        /// </remarks>
        /// <seealso cref="ExtraArgs"/>
        public List<ExtraArgs> QueryExtraArgs(string functionName)
        {
            return null;
        }
        /// <summary>
        /// IAPIElement: Update the element configuration
        /// </summary>
        /// <param name="fromElement">
        /// IAPIElement that needs to update the configuration
        /// </param>
        /// <remarks>
        /// Use this method to force an update of the element configuration. This is useful when the element configuration is changed anywhere.
        /// </remarks>
        public void UpdateConfiguration(IAPIElement fromElement = null) { }
        /// <summary>
        /// IAPIElement: Update the element properties (exposed in the EditorInfo property)  in the provider server
        /// </summary>
        /// <param name="odata">
        /// Custom object information contained in an ObjectWrapper object
        /// </param>
        public async Task UpdateObject(ObjectWrapper odata = null)
        {
            await Task.Yield();
        }
        public bool Equals(IAPIElement other)
        {
            return Name == other?.Name;
        }
    }
}
