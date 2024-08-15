using System;

namespace Jank.Utilities.Disposables
{
    public class EmptyDisposable : IDisposable
    {
        public void Dispose() { }
    }
}