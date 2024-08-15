using Cysharp.Threading.Tasks;
using Jank.Feedback;
using Jank.Props.Utilities.ComponentParents;
using Jank.Utilities;
using UnityEngine;

namespace Jank
{
    [CreateAssetMenu(menuName = "Jank/Feedback/Indicator/AnimatorBool")]
    public class FeedbackIndicatorAnimatorBool : FeedbackImplementation
    {
        public string Name;
        public bool Value;
     
        public override UniTask PerformFeedback(FeedbackEvent evt, GameObject caller)
        {
            AnimatorGroup animators = caller.GetComponent<AnimatorGroup>();
            animators.Components.ForEach(a => a.SetBool(Name, Value));
            return UniTask.CompletedTask;
        }
    }
}