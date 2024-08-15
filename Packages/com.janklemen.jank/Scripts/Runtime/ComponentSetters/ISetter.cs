using Cysharp.Threading.Tasks;

namespace Jank.ComponentSetters
{
    public interface ISetter<TValue>
    {
        UniTask Set(TValue val);
    }
}