using System;
using Jank.Observables.Observers;

namespace Jank.Observables.Observables
{
    public class EmptyObservableAsync<T> : IObservableAsync<T>
    {
        public static EmptyObservableAsync<T> Instance = new();
        
        private EmptyObservableAsync()
        {
            
        }

        public IDisposable Subscribe(IObserverAsync<T> observer)
        {
            return EmptyDisposable.Instance;
        }
        
        public class EmptyDisposable : IDisposable
        {
            public static EmptyDisposable Instance = new();
            
            private EmptyDisposable()
            {
                
            }
            
            public void Dispose()
            {
            }
        }
    }
}