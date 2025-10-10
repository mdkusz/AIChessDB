namespace GlobalCommonEntities.Interfaces
{
    /// <summary>
    /// Defines a contract for monitoring and updating progress in a multi-step operation.
    /// </summary>
    public interface IProgressMonitor
    {
        /// <summary>
        /// Sets the total step number of the operation.
        /// </summary>
        /// <param name="steps">
        /// Number of steps in the operation.
        /// </param>
        void SetTotalSteps(int steps);
        /// <summary>
        /// Restets the progress monitor to its initial state.
        /// </summary>
        /// <param name="sender">
        /// Object that is resetting the progress monitor, typically the operation initiator.
        /// </param>
        void Reset(object sender);
        /// <summary>
        /// Advances the progress monitor.
        /// </summary>
        /// <param name="c">
        /// Number of steps to advance the progress monitor.
        /// </param>
        void Step(int c = 1);
        /// <summary>
        /// Process finished event.
        /// </summary>
        /// <param name="sender">
        /// Finishing object, typically the operation initiator.
        /// </param>
        void Stop(object sender);
    }
}
