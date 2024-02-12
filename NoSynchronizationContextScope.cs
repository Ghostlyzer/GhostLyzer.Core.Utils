namespace GhostLyzer.Core.Utils
{
    /// <summary>
    /// Provides a scope within which the SynchronizationContext is set to null.
    /// </summary>
    public static class NoSynchronizationContextScope
    {
        /// <summary>
        /// Enters a scope within which the SynchronizationContext is set to null.
        /// </summary>
        /// <returns>
        /// A Disposable that restores the original SynchronizationContext when disposed.
        /// </returns>
        public static Disposable Enter()
        {
            var context = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(null);
            return new Disposable(context);
        }

        /// <summary>
        /// A struct that restores the original SynchronizationContext when disposed.
        /// </summary>
        public struct Disposable : IDisposable 
        {
            private readonly SynchronizationContext? _synchronizationContext;

            /// <summary>
            /// Initializes a new instance of the Disposable struct with the specified SynchronizationContext.
            /// </summary>
            /// <param name="synchronizationContext">The SynchronizationContext to restore when disposed.</param>
            public Disposable(SynchronizationContext? synchronizationContext)
            {
                _synchronizationContext = synchronizationContext;
            }

            /// <summary>
            /// Restores the original SynchronizationContext.
            /// </summary>
            public void Dispose() => SynchronizationContext.SetSynchronizationContext(_synchronizationContext);
        }
    }
}
