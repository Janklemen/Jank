﻿namespace Jank.Patterns.Events._interfaces
{
    /// <summary>
    /// An Event object that can be subscribed to and invoked
    /// </summary>
    public interface IEvent : IEventSubscriber
    {
        /// <summary>
        /// Invoke the event
        /// </summary>
        void Invoke();
    }

    /// <summary>
    /// An Event object that can be subscribed to and invoked
    /// </summary>
    public interface IEvent<T> : IEventSubscriber<T>
    {
        /// <summary>
        /// Invoke the event
        /// </summary>
        void Invoke(T args);
    }
}
