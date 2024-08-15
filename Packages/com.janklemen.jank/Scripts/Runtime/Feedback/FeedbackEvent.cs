namespace Jank.Feedback
{
    public struct FeedbackEvent {
        public bool IsStandardFeedback;
        public EStandardFeedbackEvent StandardFeedbackEvent;

        public static FeedbackEvent Standard(EStandardFeedbackEvent evt)
        {
            return new FeedbackEvent()
            {
                IsStandardFeedback = true,
                StandardFeedbackEvent = evt
            };
        }
    }
}