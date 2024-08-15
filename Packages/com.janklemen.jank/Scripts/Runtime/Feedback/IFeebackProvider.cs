using Cysharp.Threading.Tasks;
using Jank.Feedback;
using UnityEngine;

namespace Jank
{
    public interface IFeebackProvider
    {
        UniTask PerformFeedback(FeedbackEvent evt, GameObject caller);
    }
}
