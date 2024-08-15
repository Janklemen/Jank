using Cysharp.Threading.Tasks;
using Jank.Observables.Observables;
using Jank.Utilities;

namespace Jank.Props.Architecture
{
    public interface IClickable
    {
        IObservableAsync<Unit> ObserveClicks { get; }

        UniTask Click();
    }
}