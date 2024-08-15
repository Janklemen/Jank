using System;
using Jank.Patterns.Events._interfaces;

namespace Jank.Patterns.Events
{
    /// <summary>
    /// An Event object that can be subscribed to and invoked
    /// </summary>
    public class Event : IEvent
    {
        event Action EventAction;
        event Action SingleUseEvent;

        /// <inheritdoc />
        public EventBinding Subscribe(Action callback)
        {
            EventBinding binding = new(
                () => EventAction += callback,
                () => EventAction -= callback
            );

            binding.Subscribe();
            return binding;
        }

        /// <inheritdoc />
        public EventBinding SubscribeSingleUse(Action callback)
        {
            EventBinding binding = new(
                () => SingleUseEvent += callback,
                () => SingleUseEvent -= callback
            );

            binding.Subscribe();
            return binding;
        }

        /// <inheritdoc />
        public void Unsubscribe(Action callback)
        {
            if (callback != null)
            {
                EventAction -= callback;
            }
        }

        /// <summary>
        /// Invoke the event
        /// </summary>
        public void Invoke()
        {
            if (EventAction != null)
            {
                EventAction.Invoke();
            }

            if (SingleUseEvent != null)
            {
                SingleUseEvent.Invoke();

                Delegate[] callbacks = SingleUseEvent.GetInvocationList();

                foreach (Delegate d in callbacks)
                {
                    SingleUseEvent -= d as Action;
                }
            }
        }
    }

    /// <summary>
    /// An Event object that can be subscripbed to and invoked
    /// </summary>
    public class Event<T> : IEvent<T>
    {
        event Action<T> EventAction;
        event Action<T> SingleUseEvent;

        /// <inheritdoc />
        public EventBinding Subscribe(Action<T> callback)
        {
            EventBinding binding = new(
                () => EventAction += callback,
                () => EventAction -= callback
            );

            binding.Subscribe();
            return binding;
        }

        /// <inheritdoc />
        public EventBinding SubscribeSingleUse(Action<T> callback)
        {
            EventBinding binding = new(
                () => SingleUseEvent += callback,
                () => SingleUseEvent -= callback
            );

            binding.Subscribe();
            return binding;
        }

        /// <inheritdoc />
        public void Unsubscribe(Action<T> callback)
        {
            if (callback != null)
            {
                EventAction -= callback;
            }
        }

        /// <summary>
        /// Invoke the event
        /// </summary>
        public void Invoke(T arg)
        {
            if (EventAction != null)
            {
                EventAction.Invoke(arg);
            }

            if (SingleUseEvent != null)
            {
                SingleUseEvent.Invoke(arg);

                Delegate[] callbacks = SingleUseEvent.GetInvocationList();

                foreach (Delegate d in callbacks)
                {
                    SingleUseEvent -= d as Action<T>;
                }
            }
        }
    }
}
