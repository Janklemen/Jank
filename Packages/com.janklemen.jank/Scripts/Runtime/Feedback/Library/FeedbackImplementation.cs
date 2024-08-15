using Cysharp.Threading.Tasks;
using Jank.Feedback;
using UnityEngine;

namespace Jank
{
    public abstract class FeedbackImplementation : ScriptableObject, IFeebackProvider
    {
        public abstract UniTask PerformFeedback(FeedbackEvent evt, GameObject caller);
    }
}