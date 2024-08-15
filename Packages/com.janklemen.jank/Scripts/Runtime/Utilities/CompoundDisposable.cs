using System;
using System.Collections.Generic;

namespace Jank.Utilities
{
    public class CompositeDisposable : IDisposable
    {
        readonly IEnumerable<IDisposable> _disposables;
        bool _disposed;

        public CompositeDisposable(IEnumerable<IDisposable> disposables)
        {
            _disposables = disposables;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }

            _disposed = true;
        }
    }
}