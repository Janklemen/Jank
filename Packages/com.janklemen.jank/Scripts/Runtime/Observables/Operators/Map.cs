using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Jank.Observables.Observables;
using Jank.Observables.Observers;
using Jank.Utilities;
using UnityEngine;

namespace Jank.Observables.Operators
{
    public class MapObservableAsync<TIn, TOut> : IObservableAsync<TOut>
    {
        IObservableAsync<TIn> _origin;
        Observer _observer;
        
        public MapObservableAsync(IObservableAsync<TIn> origin, FuncAsync<TIn, TOut> selector)
        {
            _origin = origin;
            _observer = new Observer(selector);
            _origin.Subscribe(_observer);
        }

        public IDisposable Subscribe(IObserverAsync<TOut> observer) => _observer.AddObserver(this, observer);

        class Observer : IObserverAsync<TIn>
        {
            public GameObject GameObject { get; set; }
            
            readonly List<IObserverAsync<TOut>> _observers = new();
            FuncAsync<TIn, TOut> _selector;

            public Observer(FuncAsync<TIn, TOut> selector) => _selector = selector;

            public Subscription AddObserver(object origin, IObserverAsync<TOut> observer)
            {
                if(!_observers.Contains(observer))
                    _observers.Add(observer);

                return new Subscription(origin, () => _observers.Remove(observer));
            }

            public async UniTask OnCompleted()
                => await _observers.ForEachAsync(async (target) => await target.OnCompleted());

            public async UniTask OnError(Exception error)
                => await _observers.ForEachAsync(async (target) => await target.OnError(error) );

            public async UniTask OnNext(TIn value)
            {
                TOut selected = await _selector(value);
                await _observers.ForEachAsync(async (target) => await target.OnNext(selected));
            }
        }
    }
    
    public static class UTMapObservableAsync
    {
        public static IObservableAsync<TOut> Map<TIn, TOut>(this IObservableAsync<TIn> target, FuncAsync<TIn, TOut> selector)
            => new MapObservableAsync<TIn, TOut>(target, selector);
    }
}