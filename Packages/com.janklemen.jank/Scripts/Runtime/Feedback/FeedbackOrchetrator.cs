using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Jank.Inspector.CustomEditorGenerator;
using UnityEngine;

namespace Jank.Feedback
{
    [JankCustomEditor]
    public partial class FeedbackOrchetrator : MonoBehaviour, IFeebackProvider
    {
        [Header("Mouse Event Feedbacks")] 
        [JankInspect] public List<IFeebackProvider> FeedbackProviders;

        public async UniTask PerformFeedback(FeedbackEvent evt, GameObject caller)
        {
            foreach (IFeebackProvider feedbackProvider in FeedbackProviders)
            {
                if (feedbackProvider != null)
                    await feedbackProvider.PerformFeedback(evt, gameObject);
            }
        }
    }
}