namespace GlobalCommonEntities.Interfaces
{
    /// <summary>
    /// Interface to interact with UI elements.
    /// </summary>
    public interface IUIElementInteractor
    {
        /// <summary>
        /// Element collector to resolve path to elements.
        /// </summary>
        IUIRelevantElementCollector ElementCollector { get; set; }
        /// <summary>
        /// Highlight the element at the specified path for a given number of seconds.
        /// </summary>
        /// <param name="path">
        /// Path to the UI element to highlight.
        /// </param>
        /// <param name="seconds">
        /// Seconds to highlight the element.
        /// </param>
        /// <param name="mode">
        /// Implementation-dependant mode of highlighting.
        /// </param>
        void HighLight(string path, int seconds, string mode);
        /// <summary>
        /// Displays a notification balloon with the specified title and message for a given duration.
        /// </summary>
        /// <param name="path">
        /// Path to the UI element to comment.
        /// </param>
        /// <param name="title">
        /// The title of the notification balloon.
        /// </param>
        /// <param name="message">
        /// The message content of the notification balloon.
        /// </param>
        /// <param name="mode">
        /// Implementation-dependant mode of highlighting.
        /// </param>
        /// <param name="seconds">
        /// The duration, in seconds, for which the notification balloon is displayed.
        /// </param>
        void ShowBalloon(string path, string title, string message, string mode, int seconds);
        /// <summary>
        /// Invoke an action on a UI element at the specified path.
        /// </summary>
        /// <param name="path">
        /// Path to the UI element to invoke the action on.
        /// </param>
        /// <param name="action">
        /// Action name to invoke on the UI element, such as "click", "double-click", etc.
        /// </param>
        /// <returns>
        /// Value or null
        /// </returns>
        object Invoke(string path, string action);
    }
}
