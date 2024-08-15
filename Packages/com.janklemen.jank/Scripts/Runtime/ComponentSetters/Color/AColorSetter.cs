using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Jank.ComponentSetters.Color
{
    public abstract class AColorSetter : MonoBehaviour, IColorSetter
    {
        public abstract UniTask Set(UnityEngine.Color val);
    }
}