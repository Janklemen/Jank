using System;
using Cysharp.Threading.Tasks;

namespace Jank.Observables.Observers
{
    public class EmptyObserverAsync<T> : IObserverAsync<T>
    {
        public static readonly EmptyObserverAsync<T> Instance = new();

        EmptyObserverAsync() { }
        public UniTask OnCompleted() => UniTask.CompletedTask;
        public UniTask OnError(Exception error) => UniTask.CompletedTask;
        public UniTask OnNext(T value) => UniTask.CompletedTask;
    }
}