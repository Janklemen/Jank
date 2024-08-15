using System;
using Cysharp.Threading.Tasks;
using Jank.Utilities;
using UnityEngine;

namespace Jank.Services.Experience
{
    /// <summary>
    /// This is a ScriptableObject with exposed settings that can be used to control things like animations playing,
    /// audio playing, etc.
    /// </summary>
    public abstract class AExperienceService : AJankleScriptableObjectService, IExperienceService
    {
        /// <inheritdoc/>
        public abstract void Animate(Action action);

        /// <inheritdoc/>
        public abstract void SetAnimatorTrigger(Animator animator, string trigger);


        /// <inheritdoc/>
        public abstract void PlayAudioClip(AudioSource source, AudioClip clip); 
        
        /// <inheritdoc/>
        public abstract UniTask PlayAwaited(ActionAsync timedAction);
    }
}