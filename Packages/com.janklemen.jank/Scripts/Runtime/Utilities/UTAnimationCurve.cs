using UnityEngine;

namespace Jank.Utilities
{
    public static class UTAnimationCurve
    {
        public static AnimationCurve Linear() => AnimationCurve.Linear(0, 0, 1, 1);
        public static AnimationCurve Ease() => AnimationCurve.EaseInOut(0, 0, 1, 1);
    }
}