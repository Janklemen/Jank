using System;
using Cysharp.Threading.Tasks;

namespace Jank.Observables.Observers
{
    /// <summary>
    /// An async version of a normal observable. Subscribed observers are run as async functions and awaited.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObserverAsync<in T>
    {
        UniTask OnCompleted();
        UniTask OnError(Exception error);
        UniTask OnNext(T value);
    }
}