using Cysharp.Threading.Tasks;
using Jank.Feedback;
using Jank.Inspector.CustomEditorGenerator;
using UnityEngine;

namespace Jank
{
    [CreateAssetMenu(menuName = "Jank/Feedback/Behaviour/Standard")]
    public partial class FeedbackBehaviourStandard: FeedbackImplementation
    {
        public FeedbackImplementation SelectableFeedback;
        public FeedbackImplementation SelectedFeedback;
        public FeedbackImplementation EnterFeedback;
        public FeedbackImplementation DownFeedback;
        public FeedbackImplementation UpFeedback;
        public FeedbackImplementation ExitFeedback;
        public FeedbackImplementation AppearingFeedback;
        public FeedbackImplementation DisappearingFeedback;
        public FeedbackImplementation MovingFeedback;
        public FeedbackImplementation BeingAffectedFeedback;
        public FeedbackImplementation AffectingFeedback;
        
        public override async UniTask PerformFeedback(FeedbackEvent evt, GameObject caller)
        {
            switch (evt.StandardFeedbackEvent)
            {
                case EStandardFeedbackEvent.Selectable:
                    await DoFeedback(SelectableFeedback);
                    break;
                case EStandardFeedbackEvent.Selected:
                    await DoFeedback(SelectedFeedback);
                    break;
                case EStandardFeedbackEvent.Enter:
                    await DoFeedback(EnterFeedback);
                    break;
                case EStandardFeedbackEvent.Down:
                    await DoFeedback(DownFeedback);
                    break;
                case EStandardFeedbackEvent.Up:
                    await DoFeedback(UpFeedback);
                    break;
                case EStandardFeedbackEvent.Exit:
                    await DoFeedback(ExitFeedback);
                    break;
                case EStandardFeedbackEvent.Appearing:
                    await DoFeedback(AppearingFeedback);
                    break;
                case EStandardFeedbackEvent.Disappearing:
                    await DoFeedback(DisappearingFeedback);
                    break;
                case EStandardFeedbackEvent.Moving:
                    await DoFeedback(MovingFeedback);
                    break;
                case EStandardFeedbackEvent.BeingAffected:
                    await DoFeedback(BeingAffectedFeedback);
                    break;
                case EStandardFeedbackEvent.Affecting:
                    await DoFeedback(AppearingFeedback);
                    break;
            }

            async UniTask DoFeedback(FeedbackImplementation feedback)
            {
                if (feedback != null)
                    await feedback.PerformFeedback(evt, caller);
            }
        }
    }
}