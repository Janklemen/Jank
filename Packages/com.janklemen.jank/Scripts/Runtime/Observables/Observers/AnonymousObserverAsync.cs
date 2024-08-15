using System;
using Cysharp.Threading.Tasks;
using Jank.Utilities;
using UnityEngine;

namespace Jank.Observables.Observers
{
    public class AnonymousObserverAsync<T> : IObserverAsync<T>, ISingleObserveAsync<T>, IComponentReferencer
    {
        public Component ComponentReference { get; }
        
        readonly ActionAsync _onComplete;
        readonly ActionAsync<Exception> _onError;
        readonly ActionAsync<T> _onNext;

        public IObserverAsync<T> Observer => this;
        
        public AnonymousObserverAsync(ActionAsync<T> onNext = null, ActionAsync<Exception> onError = null, ActionAsync onComplete = null, Component componentReference = null)
        {
            _onNext = onNext ?? DefaultOnNext;
            _onError = onError ?? DefaultOnError;
            _onComplete = onComplete ?? DefaultOnComplete;
            ComponentReference = componentReference;
        }

        UniTask DefaultOnNext(T value) => UniTask.CompletedTask;
        UniTask DefaultOnComplete() => UniTask.CompletedTask;
        UniTask DefaultOnError(Exception error)
            => throw error;

        public async UniTask OnCompleted() => await _onComplete();
        public async UniTask OnError(Exception error) => await _onError(error);
        public async UniTask OnNext(T value) => await _onNext(value);
    }
}