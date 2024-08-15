using System;
using Cysharp.Threading.Tasks;
using Jank.App;
using Jank.Enums;
using Jank.Inspector;
using Jank.Inspector.CustomEditorGenerator;
using Jank.Objects;
using Jank.Observables.Observables;
using Jank.Observables.Subject;
using Jank.Services.Animation;
using Jank.Utilities;
using Jank.Utilities.Disposing;
using UnityEngine;

namespace Jank.Props.Architecture
{
    public abstract class AProp : MonoBehaviour, ILoadable, IInteractable, ITransformable
    {
        [JankInject] protected AppSignals Signals;
        [JankInject] protected AnimationService Anims;
        
        [JankInspect] public object DebugData;
        [JankInspect] public ELoadState LoadState { get; private set; } = ELoadState.Unloaded;
        [JankInspect] public EInteractionStatus InteractionStatus { get; private set; } = EInteractionStatus.NotInteractable;
        
        /// <summary>
        /// Subscriptions that were created on Load, that should be disposed on unload. 
        /// </summary>
        protected DisposableManager LoadSubscriptions = new();
        
        /// <summary>
        /// The Transform associated with this token
        /// </summary>
        public Transform Transform => transform;
        
        public IObservableAsync<Unit> ObserveLoad() => SubjectLoad;
        protected MonoSubjectUnit SubjectLoad;

        public IObservableAsync<Unit> ObserveUnload() => SubjectUnload;
        protected MonoSubjectUnit SubjectUnload;

        async UniTask Awake() => await Unload();
        
        [JankInspect]
        public async UniTask Load()
        {
            MonoSubjectUnit.MakeSubjectIfNull(ref SubjectLoad, gameObject, $"AProp:OnLoad");
            MonoSubjectUnit.MakeSubjectIfNull(ref SubjectUnload, gameObject, $"AProp:OnUnload");

            using IDisposable anims = Anims?.DisableAnimations();
            
            if (LoadState == ELoadState.Loaded)
                return;

            await Unload();
            await OnLoadProp();
            LoadState = ELoadState.Loaded;

            await SubjectLoad.OnNext(Unit.Default);
        }
        
        protected abstract UniTask OnLoadProp();
        
        public async UniTask Unload()
        {
            if(SubjectUnload != null)
                await SubjectUnload.OnNext(Unit.Default);

            await OnUnloadProp();
            await SetInteractable(false);
            LoadSubscriptions.Dispose();
            LoadState = ELoadState.Unloaded;
        }

        protected abstract UniTask OnUnloadProp();
        
        public async UniTask SetInteractable(bool isInteractable)
        {
            if (isInteractable && LoadState == ELoadState.Unloaded)
                return;

            await OnSetInteractable(isInteractable);
            InteractionStatus = isInteractable ? EInteractionStatus.Interactable : EInteractionStatus.NotInteractable;
        }

        protected abstract UniTask OnSetInteractable(bool isInteractable);
        
        public async UniTask Clear() => await Unload();
    }

    public abstract class AProp<T> : AProp, ILoadable<T>
    {
        [JankInspect] public T LoadData { get; set; }

        protected override async UniTask OnLoadProp()
            => await OnLoadPropWithData(LoadData);

        /// <summary>
        /// Loads all visuals related to the data immediately. No animations will occur.
        /// </summary>
        public async UniTask LoadWithData(T data)
        {
            LoadData = data;
            await Load();
        }

        protected abstract UniTask OnLoadPropWithData(T data);

        protected override async UniTask OnUnloadProp()
        {
            await OnUnloadPropWithData(LoadData);
        }

        protected abstract UniTask OnUnloadPropWithData(T data);
    }
}