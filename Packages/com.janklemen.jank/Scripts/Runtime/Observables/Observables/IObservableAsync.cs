using System;
using Jank.Observables.Observers;
using Jank.Utilities;
using UnityEngine;

namespace Jank.Observables.Observables
{
    /// <summary>
    /// Allows subscription of async observers. <see cref="IObserverAsync{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObservableAsync<out T>
    {
        IDisposable Subscribe(IObserverAsync<T> observer);
    }

    public static class UTIObservableAsync
    {
        public static IDisposable Subscribe<T>(this IObservableAsync<T> target, ActionAsync<T> onNext = null, ActionAsync<Exception> onError = null, ActionAsync onComplete = null, Component componentReference = null)
        {
            return target.Subscribe(new AnonymousObserverAsync<T>(onNext, onError, onComplete, componentReference));
        }
        
        public static IDisposable Subscribe<T>(this IObservableAsync<T> target, Action<T> onNext = null, Action<Exception> onError = null, Action onComplete = null, Component componentReference = null)
        {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            return target.Subscribe(new AnonymousObserverAsync<T>(
                onNext == null ? null : async e => onNext(e), 
                onError == null ? null : async ex => onError(ex), 
                onComplete == null ? null : async () => onComplete(),
                componentReference
            ));
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        }
    }
}