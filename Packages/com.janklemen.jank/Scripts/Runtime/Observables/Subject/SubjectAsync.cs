using System;
using Cysharp.Threading.Tasks;
using Jank.Observables.Observers;
using Jank.Utilities;
using Jank.Utilities.Disposables;

namespace Jank.Observables.Subject
{
    public class SubjectAsync<T> : ISubjectAsync<T>, IDisposable, ISingleObserveAsync<T>
    {
        readonly object _observerLock = new();

        bool _isStopped;
        bool _isDisposed;
        Exception _lastError;

        IObserverAsync<T> _outObserver = EmptyObserverAsync<T>.Instance;
        
        public IObserverAsync<T> Observer => _outObserver;
        
        public SubjectAsync()
        {
            
        }
        
        public bool HasObservers => !(_outObserver is EmptyObserverAsync<T>) && !_isStopped && !_isDisposed;

        public UniTask OnCompleted()
        {
            IObserverAsync<T> old;
            lock (_observerLock)
            {
                ThrowIfDisposed();
                if (_isStopped)
                    return UniTask.CompletedTask;

                old = _outObserver;
                _outObserver = EmptyObserverAsync<T>.Instance;
                _isStopped = true;
            }

            old.OnCompleted();
            return UniTask.CompletedTask;
        }

        public UniTask OnError(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            IObserverAsync<T> old;
            lock (_observerLock)
            {
                ThrowIfDisposed();
                if (_isStopped) 
                    return UniTask.CompletedTask;

                old = _outObserver;
                _outObserver = EmptyObserverAsync<T>.Instance;
                _isStopped = true;
                _lastError = error;
            }

            old.OnError(error);
            return UniTask.CompletedTask;
        }

        public async UniTask OnNext(T value)
        {
            await _outObserver.OnNext(value);
        }

        public IDisposable Subscribe(IObserverAsync<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            Exception ex;

            lock (_observerLock)
            {
                ThrowIfDisposed();
                if (!_isStopped)
                {
                    if (_outObserver is ListObserverAsync<T> listObserver)
                        _outObserver = listObserver.Add(observer);
                    else
                    {
                        IObserverAsync<T> current = _outObserver;
                        _outObserver = current is EmptyObserverAsync<T>
                            ? observer
                            : new ListObserverAsync<T>(new ImmutableList<IObserverAsync<T>>(new[] {current, observer}));
                    }

                    return new Subscription(this, observer);
                }

                ex = _lastError;
            }

            if (ex != null)
            {
                observer.OnError(ex);
            }
            else
            {
                observer.OnCompleted();
            }

            return UTDisposable.Empty;
        }

        public void Dispose()
        {
            lock (_observerLock)
            {
                _isDisposed = true;
                _outObserver = DisposedObserverAsync<T>.Instance;
            }
        }

        void ThrowIfDisposed()
        {
            if (_isDisposed) throw new ObjectDisposedException("");
        }

        public bool IsRequiredSubscribeOnCurrentThread()
        {
            return false;
        }

        class Subscription : IDisposable
        {
            readonly object _gate = new();
            SubjectAsync<T> _parent;
            IObserverAsync<T> _unsubscribeTarget;

            public Subscription(SubjectAsync<T> parent, IObserverAsync<T> unsubscribeTarget)
            {
                this._parent = parent;
                this._unsubscribeTarget = unsubscribeTarget;
            }

            public void Dispose()
            {
                lock (_gate)
                {
                    if (_parent != null)
                    {
                        lock (_parent._observerLock)
                        {
                            if (_parent._outObserver is ListObserverAsync<T> listObserver)
                                _parent._outObserver = listObserver.Remove(_unsubscribeTarget);
                            else
                                _parent._outObserver = EmptyObserverAsync<T>.Instance;

                            _unsubscribeTarget = null;
                            _parent = null;
                        }
                    }
                }
            }
        }
    }
}