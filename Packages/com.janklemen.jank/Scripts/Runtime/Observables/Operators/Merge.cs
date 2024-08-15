using System;
using System.Collections.Generic;
using Jank.Observables.Observables;
using Jank.Observables.Observers;
using Jank.Utilities;

namespace Jank.Observables.Operators
{
    public class MergeObservableAsync<T> : IObservableAsync<T>
    {
        IEnumerable<IObservableAsync<T>> _observables;
        
        public MergeObservableAsync(IEnumerable<IObservableAsync<T>> merged)
        {
            _observables = merged;
        }

        public IDisposable Subscribe(IObserverAsync<T> observer)
        {
            List<IDisposable> disposables = new();
            
            foreach (IObservableAsync<T> observableAsync in _observables)
                disposables.Add(observableAsync.Subscribe(observer));
            
            return new CompositeDisposable(disposables);
        }
    }
    
    public static class UTMergeObservableAsync
    {
        public static IObservableAsync<T> Merge<T>(this IEnumerable<IObservableAsync<T>> target)
            => new MergeObservableAsync<T>(target);
    }
}