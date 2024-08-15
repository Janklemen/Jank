using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Jank.Props.Architecture;
using Jank.Utilities;
using UnityEngine;

namespace Jank.Services.Animation
{
    [CreateAssetMenu(fileName = "AnimationService", menuName = "Jank/Services/AnimationService")]
    public class AnimationService : AAnimationService
    {
        int _disables;
        float _cachedAnimationSpeed = -1;
        [SerializeField] float AnimationSpeed;

        public override async UniTask InterpolateToken(ITransformable token, Vector3 to,
            CancellationToken clt)
            => await UTInterpolate.EasePosition(token.Transform, to, AnimationSpeed, clt);

        public override IDisposable DisableAnimations()
        {
            _disables++;

            if (AnimationSpeed == 0)
                return new DisabledAnimationSubscription(this);

            _cachedAnimationSpeed = AnimationSpeed;
            AnimationSpeed = 0;
            return new DisabledAnimationSubscription(this);
        }

        public override void EnableAnimations()
        {
            if (AnimationSpeed != 0)
                return;

            if (_disables != 0)
                _disables--;

            if (_disables == 0)
                AnimationSpeed = _cachedAnimationSpeed;
        }

        public class DisabledAnimationSubscription : IDisposable
        {
            AnimationService _service;
            
            public DisabledAnimationSubscription(AnimationService service)
            {
                _service = service;
            }

            public void Dispose()
            {
                _service.EnableAnimations();
            }
        }
    }
}