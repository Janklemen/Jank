using Cysharp.Threading.Tasks;
using Jank.Utilities.Random;

namespace Jank.Props.Scenes
{
    public class PropScene : APropTestScene
    {
        protected async UniTask Start()
        {
            await LoadProp(() => UTRandom.Color(), true);
        }
    }
}