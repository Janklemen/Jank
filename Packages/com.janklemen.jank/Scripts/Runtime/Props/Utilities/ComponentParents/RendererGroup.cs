using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Jank.ComponentSetters.Color;
using Jank.Utilities;
using UnityEngine;

namespace Jank.Props.Utilities.ComponentParents
{
    public class RendererGroup : AComponentGroup<Renderer>
    {
        public void EnableRenderers()
            => Components.ForEach(t => t.enabled = true);
        
        public void DisableRenderers()
            => Components.ForEach(t => t.enabled = false);

        public IEnumerable<Material> Materials => Components
            .Where(rend => rend != null)
            .Select(rend => rend.material);
    }
}