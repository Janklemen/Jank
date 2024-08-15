using System;
using Cysharp.Threading.Tasks;
using Jank.Feedback;
using UnityEngine;

namespace Jank
{
    [CreateAssetMenu(menuName = "Jank/Feedback/Filter/StandardEvent")]
    public class FeedbackFilterStandardEvent : FeedbackImplementation
    {
        public EStandardFeedbackEvent Event;
        public FeedbackImplementation[] Feedbacks;
        
        public override async UniTask PerformFeedback(FeedbackEvent evt, GameObject caller)
        {
            if (evt.IsStandardFeedback && (evt.StandardFeedbackEvent & Event) != 0)
            {
                foreach (FeedbackImplementation feedbackImplementation in Feedbacks)
                {
                    if(feedbackImplementation != null)
                        await feedbackImplementation.PerformFeedback(evt, caller);
                }
            }
        }
    }
}