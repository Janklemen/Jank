using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Jank.Objects;
using Jank.Props.Architecture.Token;
using Jank.Services.Animation;
using Jank.Utilities;
using UnityEngine;

namespace Jank.Props.Utilities.Arrangers
{
    /// <summary>
    /// Arranges children by applying a step provided by the developer to the children, spacing them by
    /// that much, then centering them. 
    /// </summary>
    public partial class StepArranger : ATokenArranger
    {
        [SerializeField] float GizmoRadius;
        [SerializeField] Color GizmoColor;
        [SerializeField] Vector3 Step;

        [JankInject] IAnimationService _anim;

        CancellationTokenSource _cancelReorder;
        
        void OnDrawGizmos()
        {
            Vector3 origin = CalculateInitialPoint(4);

            for (int i = 0; i < 4; i++)
            {
                Color last = Gizmos.color;
                Gizmos.color = GizmoColor;
                Gizmos.DrawSphere(transform.TransformPoint(origin + Step * i), GizmoRadius);
                Gizmos.color = last;
            }
        }

        public override async UniTask Reorder(IEnumerable<IToken> currentTokens)
        {
            UTCancellationTokenSource.CancelAndRenew(ref _cancelReorder);
            
            IEnumerable<IToken> tokens = currentTokens as IToken[] ?? currentTokens.ToArray();
            
            Vector3 point = CalculateInitialPoint(tokens.Count());

            List<UniTask> lerps = new();
            foreach (IToken token in tokens)
            {
                lerps.Add( _anim.InterpolateToken(token, point, _cancelReorder.Token));
                point += Step;
            }

            await UniTask.WhenAll(lerps);
        }
        
        Vector3 CalculateInitialPoint(int count)
        {
            return -(Step * (count -1) / 2);
        }
    }
}