using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Jank.Props.Architecture;
using UnityEngine;

namespace Jank.Services.Animation
{
    public abstract class AAnimationService : AJankleScriptableObjectService, IAnimationService
    {
        public abstract UniTask InterpolateToken(ITransformable token, Vector3 to, CancellationToken clt);
        public abstract IDisposable DisableAnimations();
        public abstract void EnableAnimations();
    }
}