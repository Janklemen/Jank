using Cysharp.Threading.Tasks;
using Jank.Feedback;
using Jank.Observables.Observables;
using Jank.Observables.Subject;
using UnityEngine;

namespace Jank.Observables.MouseEvents
{
    /// <summary>
    /// Observes and reports mouse events that occur on the parent object.
    /// </summary>
    public class MouseEventObserver : MonoBehaviour
    {
        /// <summary>
        /// Gets the observable sequence of mouse events.
        /// </summary>
        /// <value>
        /// The mouse event observations.
        /// </value>
        public IObservableAsync<EStandardFeedbackEvent> Observations => _observationsSubject;

        ISubjectAsync<EStandardFeedbackEvent> _observationsSubject = new SubjectAsync<EStandardFeedbackEvent>();

        async UniTask OnMouseDown() => await _observationsSubject.OnNext(EStandardFeedbackEvent.Down);
        async UniTask OnMouseUpAsButton() => await _observationsSubject.OnNext(EStandardFeedbackEvent.Up);
        async UniTask OnMouseEnter() => await _observationsSubject.OnNext(EStandardFeedbackEvent.Enter);
        async UniTask OnMouseExit() => await _observationsSubject.OnNext(EStandardFeedbackEvent.Exit);
    }
}
