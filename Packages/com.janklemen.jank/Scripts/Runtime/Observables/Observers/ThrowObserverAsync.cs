using System;
using Cysharp.Threading.Tasks;

namespace Jank.Observables.Observers
{
    public class ThrowObserverAsync<T> : IObserverAsync<T>
    {
        public static readonly ThrowObserverAsync<T> Instance = new();

        ThrowObserverAsync()
        {
        }

        public UniTask OnCompleted()
        {
            return UniTask.CompletedTask;
        }

        public UniTask OnError(Exception error)
        {
            throw error;
        }

        public UniTask OnNext(T value)
        {
            return UniTask.CompletedTask;
        }
    }
}