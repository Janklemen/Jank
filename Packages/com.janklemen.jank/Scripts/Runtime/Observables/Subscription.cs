using System;

namespace Jank.Observables
{
    /// <summary>
    /// Stores an action that is called on <see cref="Dispose"/> which should unsubscribe from the subscription
    /// </summary>
    public class Subscription : IDisposable
    {
        readonly object _origin;
        readonly Action _unsubscribe;

        public Subscription(object origin, Action unsubscribe)
        {
            _origin = origin;
            _unsubscribe = unsubscribe;
        }

        public void Dispose() => _unsubscribe();
    }
}