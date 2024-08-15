using Cysharp.Threading.Tasks;
using Jank.Objects;

namespace Jank.App
{
    public interface IRunOnContainerLoad
    {
        UniTask RunLoadOperation(ObjectManager manager);
    }
}