namespace Jank.Observables.Observers
{
    public interface ISingleObserveAsync<T>
    {
        public IObserverAsync<T> Observer { get; }
    }
}