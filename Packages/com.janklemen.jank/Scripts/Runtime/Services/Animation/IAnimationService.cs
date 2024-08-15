using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Jank.Props.Architecture;
using UnityEngine;

namespace Jank.Services.Animation
{
    public interface IAnimationService
    {
        UniTask InterpolateToken(ITransformable token, Vector3 to, CancellationToken clt);
        IDisposable DisableAnimations();
        void EnableAnimations();
    }
}