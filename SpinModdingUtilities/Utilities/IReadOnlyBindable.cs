using System;

namespace SMU.Utilities {
    /// <summary>
    /// A read-only interface for a bindable
    /// </summary>
    public interface IReadOnlyBindable<T> {
        /// <summary>
        /// The stored value
        /// </summary>
        public T Value { get; }
        
        /// <summary>
        /// Subscribes an action to the bindable's onChanged event
        /// </summary>
        /// <param name="action">The action to subscribe</param>
        public void Bind(Action<T> action);

        /// <summary>
        /// Subscribes an action to the bindable's onChanged event and invokes the action immediately
        /// </summary>
        /// <param name="action">The action to subscribe and invoke</param>
        public void BindAndInvoke(Action<T> action);

        /// <summary>
        /// Unsubscribes an action from the bindable's onChanged event
        /// </summary>
        /// <param name="action">The action to unsubscribe</param>
        public void Unbind(Action<T> action);
    }
}