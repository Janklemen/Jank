using Cysharp.Threading.Tasks;
using Jank.ComponentSetters.Color;
using Jank.Inspector.CustomEditorGenerator;
using Jank.Props.Architecture.Token;
using UnityEngine;

namespace Jank.Props.Library
{
    [JankCustomEditor]
    public partial class SimpleToken : AToken<Color>
    {
        [SerializeField] AColorSetter TypeIndicatorSetter;
        
        protected override async UniTask OnLoadToken(Color data)
        {
            await TypeIndicatorSetter.Set(data);
        }

        protected override async UniTask OnUnloadToken(Color data)
        {
            await TypeIndicatorSetter.Set(default);
        }

        protected override UniTask OnSetInteractableStandard(bool isInteractable)
        {
            return UniTask.CompletedTask;
        }
    }
}