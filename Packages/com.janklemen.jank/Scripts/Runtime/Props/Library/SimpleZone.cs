using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Jank.ComponentSetters.Color;
using Jank.Inspector;
using Jank.Inspector.CustomEditorGenerator;
using Jank.Objects;
using Jank.Props.Architecture.Zone;
using Jank.Props.Utilities.Arrangers;
using Jank.Services.Animation;
using Jank.Utilities;
using UnityEngine;


namespace Jank.Props.Library
{
    [JankCustomEditor]
    public partial class SimpleZone : AZone<Color>
    {
        [SerializeField] AColorSetter ColorSetter;
        [SerializeField] ATokenArranger TokenArranger;

        [JankInject] IAnimationService _anims;

        CancellationTokenSource _reorderCancellation;
        
        public override Transform TokenParent => TokenArranger?.transform ?? transform;

        protected override async UniTask OnLoadZone(Color data)
        {
            if(ColorSetter != null)
                await ColorSetter.Set(data);
        }

        protected override async UniTask OnUnloadZone(Color data)
        {
            if(ColorSetter != null)
                await ColorSetter.Set(default);
        }

        public override async UniTask Reorder()
        {
            UTCancellationTokenSource.CancelAndRenew(ref _reorderCancellation);

            if (TokenArranger == null)
            {
                IEnumerable<UniTask> uniTasks = CurrentTokens.Select(token => _anims.InterpolateToken(token, Vector3.zero, _reorderCancellation.Token));
                await UniTask.WhenAll(uniTasks);
                return;
            }
            
            await TokenArranger.Reorder(CurrentTokens);
        }

        protected override UniTask OnSetInteractableStandard(bool isInteractable)
        {
            return UniTask.CompletedTask;
        }
    }
}