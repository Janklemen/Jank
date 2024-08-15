using System;
using System.Collections.Generic;

namespace Jank.Utilities.Disposing
{
    public class DisposableManager : IDisposable
    {
        readonly List<IDisposable> _disposables = new();

        public void Dispose() => _disposables.ForEach(d => d.Dispose());
        public void Add(IDisposable disposable) => _disposables.Add(disposable);
        public void Remove(IDisposable disposable) => _disposables.Remove(disposable);

        public static DisposableManager operator +(DisposableManager manager, IDisposable disposable) 
            => AddDisposable(manager, disposable);

        public static DisposableManager operator -(DisposableManager manager, IDisposable disposable) 
            => RemoveDisposable(manager, disposable);

        static DisposableManager AddDisposable(DisposableManager manager, IDisposable disposable)
        {
            manager.Add(disposable);
            return manager;
        }

        private static DisposableManager RemoveDisposable(DisposableManager manager, IDisposable disposable)
        {
            manager.Remove(disposable);
            return manager;
        }
    }
}