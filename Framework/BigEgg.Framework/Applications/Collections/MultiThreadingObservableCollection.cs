using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Threading;

namespace BigEgg.Framework.Applications.Collections
{
    /// <summary>
    /// Represents an ObservableCollection (a dynamic data collection that provides notifications when items gets added, removed, or when the whole list is refreshed) which support the multiThreading control
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class MultiThreadingObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        /// Occurs when an item is added, removed, changed, moved, or entire list is refreshed.
        /// </summary>
        public override event NotifyCollectionChangedEventHandler CollectionChanged;    // Override the event so this class can access it

        /// <summary>
        /// Raises the <see cref="System.Collections.ObjectModel.ObservableCollection&lt;T&gt;.CollectionChanged"/> event with the provided arguments.
        /// </summary>
        /// <param name="e">Arguments of the event being raised.</param>
        protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Be nice - use BlockReentrancy like MSDN said
            using (BlockReentrancy())
            {
                System.Collections.Specialized.NotifyCollectionChangedEventHandler eventHandler = CollectionChanged;
                if (eventHandler == null)
                    return;

                Delegate[] delegates = eventHandler.GetInvocationList();
                // Walk thru invocation list
                foreach (System.Collections.Specialized.NotifyCollectionChangedEventHandler handler in delegates)
                {
                    DispatcherObject dispatcherObject = handler.Target as DispatcherObject;
                    // If the subscriber is a DispatcherObject and different thread
                    if (dispatcherObject != null && dispatcherObject.CheckAccess() == false)
                    {
                        // Invoke handler in the target dispatcher's thread
                        dispatcherObject.Dispatcher.Invoke(DispatcherPriority.DataBind, handler, this, e);
                    }
                    else // Execute handler as is
                        handler(this, e);
                }
            }
        }
    }
}
