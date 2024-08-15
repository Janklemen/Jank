using System;
using Cysharp.Threading.Tasks;

namespace Jank.Observables.Observers
{
    public class DisposedObserverAsync<T> : IObserverAsync<T>
    {
        public static readonly DisposedObserverAsync<T> Instance = new DisposedObserverAsync<T>();

        DisposedObserverAsync()
        {

        }

        public UniTask OnCompleted()
        {
            throw new ObjectDisposedException("");
        }

        public UniTask OnError(Exception error)
        {
            throw new ObjectDisposedException("");
        }

        public UniTask OnNext(T value)
        {
            throw new ObjectDisposedException("");
        }
    }
}