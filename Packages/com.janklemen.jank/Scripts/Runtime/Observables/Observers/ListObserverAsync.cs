using System;
using Cysharp.Threading.Tasks;
using Jank.Utilities;

namespace Jank.Observables.Observers
{
    public class ListObserverAsync<T> : IObserverAsync<T>, ICompositeObserverAsync<T>
    {
        public ImmutableList<IObserverAsync<T>> Observers { get; }

        public ListObserverAsync(ImmutableList<IObserverAsync<T>> observers)
        {
            Observers = observers;
        }

        public async UniTask OnCompleted()
        {
            IObserverAsync<T>[] targetObservers = Observers.Data;
            foreach (IObserverAsync<T> t in targetObservers)
                await t.OnCompleted();
        }

        public async UniTask OnError(Exception error)
        {
            IObserverAsync<T>[] targetObservers = Observers.Data;
            foreach (IObserverAsync<T> t in targetObservers)
                await t.OnError(error);
        }

        public async UniTask OnNext(T value)
        {
            IObserverAsync<T>[] targetObservers = Observers.Data;
            foreach (IObserverAsync<T> t in targetObservers) 
                await t.OnNext(value);
        }

        internal IObserverAsync<T> Add(IObserverAsync<T> observer) => new ListObserverAsync<T>(Observers.Add(observer));

        internal IObserverAsync<T> Remove(IObserverAsync<T> observer)
        {
            var i = Array.IndexOf(Observers.Data, observer);
            if (i < 0)
                return this;

            if (Observers.Data.Length == 2)
            {
                return Observers.Data[1 - i];
            }
            else
            {
                return new ListObserverAsync<T>(Observers.Remove(observer));
            }
        }
    }
}