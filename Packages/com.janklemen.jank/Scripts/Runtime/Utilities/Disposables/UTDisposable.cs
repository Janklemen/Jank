using System;

namespace Jank.Utilities.Disposables
{
    public static class UTDisposable
    {
        static EmptyDisposable _disposable = new();

        public static IDisposable Empty => _disposable;
    }
}