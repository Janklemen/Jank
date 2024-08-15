using Cysharp.Threading.Tasks;
using Jank.Props.Utilities.ComponentParents;
using Jank.Utilities;

namespace Jank.ComponentSetters.Color
{
    public class ColorSetterShapes : AColorSetter
    {
        LazyComponent<RendererGroup> _renderer = new();

        public override UniTask Set(UnityEngine.Color val)
        {
            RendererGroup rend =_renderer.Get(gameObject);

            if (rend != null)
                rend.Materials.ForEach(m => m.color = val);
            
            return UniTask.CompletedTask;
        }
    }
}