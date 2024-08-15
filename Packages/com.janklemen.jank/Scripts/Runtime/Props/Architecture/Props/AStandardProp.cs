using System;
using Cysharp.Threading.Tasks;
using Jank.Debugging;
using Jank.Feedback;
using Jank.Inspector.CustomEditorGenerator;
using Jank.Observables.MouseEvents;
using Jank.Observables.Observables;
using Jank.Observables.Subject;
using Jank.Props.Utilities.ComponentParents;
using Jank.Utilities;
using UnityEngine;

namespace Jank.Props.Architecture
{
    public abstract class AStandardProp<T> : AProp<T>, IClickable
    {
        [SerializeField] RendererGroup Rendering;
        [SerializeField] ColliderGroup Colliders;
        [JankInspect] protected IFeebackProvider Feedback;
        
        protected IObservableAsync<EStandardFeedbackEvent> MouseEventObservations;
        [JankInspect] protected MonoSubjectUnit ClicksSubject;
        
        public IObservableAsync<Unit> ObserveClicks => ClicksSubject;
        
        IDisposable _interactivity;

        protected override async UniTask OnLoadPropWithData(T data)
        {
            MonoSubjectUnit.MakeSubjectIfNull(ref ClicksSubject, gameObject, $"AStandardProp:OnClick");
            
            Rendering.EnableRenderers();
            MouseEventObservations = Colliders.MouseEventObservations();

            // if (JankEditorUtilities.IsUnityEditorPlayMode())
            //     _disposableManager.Add(
            //         MouseEventObservations.Subscribe(UTLogFactory.MouseEventLogger(gameObject.name)));

            LoadSubscriptions.Add(MouseEventObservations.Subscribe(onNext: async o =>
            {
                if (o == EStandardFeedbackEvent.Up)
                    await Click();
            }));

            await OnLoadStandard(data);
        }

        protected abstract UniTask OnLoadStandard(T path);

        protected override async UniTask OnUnloadPropWithData(T data)
        {
            await OnUnloadStandard(data);
            Rendering.DisableRenderers();
            MouseEventObservations = null;
        }

        protected abstract UniTask OnUnloadStandard(T data);

        protected override async UniTask OnSetInteractable(bool isInteractable)
        {
            if (isInteractable)
            {
                _interactivity = MouseEventObservations.Subscribe(
                    async (evt) =>
                    {
                        if(Feedback != null)
                            await Feedback.PerformFeedback(FeedbackEvent.Standard(evt), gameObject);
                    });
                Colliders.EnableColliders();

                if (Feedback != null)
                    _ = Feedback.PerformFeedback(FeedbackEvent.Standard(EStandardFeedbackEvent.Selectable), gameObject);
            }
            else
            {
                Colliders.DisableColliders();
                _interactivity?.Dispose();
            }

            Feedback?.PerformFeedback(FeedbackEvent.Standard(EStandardFeedbackEvent.Selectable), gameObject);
            await OnSetInteractableStandard(isInteractable);
        }

        protected abstract UniTask OnSetInteractableStandard(bool isInteractable);

        public async UniTask Click()
            => await ClicksSubject.OnNext(Unit.Default);
    }
}