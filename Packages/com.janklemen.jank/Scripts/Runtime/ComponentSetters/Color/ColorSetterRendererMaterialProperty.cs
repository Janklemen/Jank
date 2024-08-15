using Cysharp.Threading.Tasks;
using Jank.Props.Utilities.ComponentParents;
using Jank.Utilities;

namespace Jank.ComponentSetters.Color
{
    public class ColorSetterRendererMaterialProperty : AColorSetter
    {
        LazyComponent<RendererGroup> _renderer = new();

        public string PropertyName;
        
        public override UniTask Set(UnityEngine.Color val)
        {
            RendererGroup rend =_renderer.Get(gameObject);

            if (rend != null)
                rend.Materials.ForEach(m => m.SetColor(PropertyName, val));
            
            return UniTask.CompletedTask;
        }
    }
}