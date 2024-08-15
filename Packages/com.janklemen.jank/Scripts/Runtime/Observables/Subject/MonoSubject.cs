using System;
using Cysharp.Threading.Tasks;
using Jank.Observables.Observers;
using Jank.Utilities;
using Jank.Utilities.Disposables;
using UnityEngine;

namespace Jank.Observables.Subject
{
    public class MonoSubject<T> : MonoBehaviour, ISubjectAsync<T>, IDisposable, IComponentReferencer, ISingleObserveAsync<T>
    {
        public string EventName { get; protected set; }
        public Component ComponentReference => this;
        
        readonly object _observerLock = new();

        bool _isStopped;
        bool _isDisposed;
        Exception _lastError;

        public IObserverAsync<T> OutObserver { get; private set; } = EmptyObserverAsync<T>.Instance;
        
        public IObserverAsync<T> Observer => OutObserver;
        
        public bool HasObservers => !(OutObserver is EmptyObserverAsync<T>) && !_isStopped && !_isDisposed;

        public UniTask OnCompleted()
        {
            IObserverAsync<T> old;
            lock (_observerLock)
            {
                ThrowIfDisposed();
                if (_isStopped)
                    return UniTask.CompletedTask;

                old = OutObserver;
                OutObserver = EmptyObserverAsync<T>.Instance;
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

                old = OutObserver;
                OutObserver = EmptyObserverAsync<T>.Instance;
                _isStopped = true;
                _lastError = error;
            }

            old.OnError(error);
            return UniTask.CompletedTask;
        }

        public async UniTask OnNext(T value)
        {
            await OutObserver.OnNext(value);
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
                    if (OutObserver is ListObserverAsync<T> listObserver)
                        OutObserver = listObserver.Add(observer);
                    else
                    {
                        IObserverAsync<T> current = OutObserver;
                        OutObserver = current is EmptyObserverAsync<T>
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
                OutObserver = DisposedObserverAsync<T>.Instance;
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
            MonoSubject<T> _parent;
            IObserverAsync<T> _unsubscribeTarget;

            public Subscription(MonoSubject<T> parent, IObserverAsync<T> unsubscribeTarget)
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
                            if (_parent.OutObserver is ListObserverAsync<T> listObserver)
                                _parent.OutObserver = listObserver.Remove(_unsubscribeTarget);
                            else
                                _parent.OutObserver = EmptyObserverAsync<T>.Instance;

                            _unsubscribeTarget = null;
                            _parent = null;
                        }
                    }
                }
            }
        }
    }
}