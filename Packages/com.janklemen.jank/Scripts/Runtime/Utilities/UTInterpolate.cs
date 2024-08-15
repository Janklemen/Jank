using System.Threading;
using Cysharp.Threading.Tasks;
using Jank.Engine;
using UnityEngine;

namespace Jank.Utilities
{
    public static class UTInterpolate
    {
        public static async UniTask LinearPosition(Transform target, Transform dest, float duration, CancellationToken clt)
            => await LinearPosition(target, dest.position, duration, clt);
        
        public static async UniTask LinearPosition(Transform target, Vector3 dest, float duration, CancellationToken clt)
            => await PerformLerp(target, target.localPosition, dest, duration, UTAnimationCurve.Linear(), clt);

        public static async UniTask EasePosition(Transform target, Transform dest, float duration, CancellationToken clt)
        {
             target.SetParent(dest, true);
             await EasePosition(target, Vector3.zero, duration, clt);
        }
        
        public static async UniTask EasePosition(Transform target, Vector3 dest, float duration, CancellationToken clt)
            => await PerformLerp(target, target.localPosition, dest, duration, UTAnimationCurve.Ease(), clt);
        
        public static async UniTask PerformLerp(Transform target, Vector3 start, Vector3 dest, float duration,
            AnimationCurve curve, CancellationToken clt)
        {
            float timeElapsed = 0;

            while (timeElapsed < duration)
            {
                if(clt.IsCancellationRequested)
                    return;
                    
                if(target == null)
                    return;

                target.localPosition = Interpolate(start, dest, timeElapsed/duration, curve); 
                float delta = await EngineHooks.AwaitNextFrame();
                timeElapsed += delta;
            }

            if(target == null)
                return;
            
            if(clt.IsCancellationRequested)
                return;
            
            target.localPosition = dest;
        }

        public static Vector3 Interpolate(Vector3 start, Vector3 end, float time, AnimationCurve curve)
        {
            float t = curve.Evaluate(time);
            return start + t * (end - start);
        }
    }
}