using Jank.Observables.Observables;
using Jank.Observables.Observers;

namespace Jank.Observables.Subject
{
    public interface ISubjectAsync<in TSource, out TResult> : IObserverAsync<TSource>, IObservableAsync<TResult>
    {
    }
    
    public interface ISubjectAsync<T> : ISubjectAsync<T, T>
    {
    }
}