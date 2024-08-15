using Jank.Utilities;

namespace Jank.Observables.Observers
{
    public interface ICompositeObserverAsync<T>
    {
        public ImmutableList<IObserverAsync<T>> Observers { get; }
    }
}