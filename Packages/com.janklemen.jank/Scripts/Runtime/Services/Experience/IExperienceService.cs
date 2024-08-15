using System;
using Cysharp.Threading.Tasks;
using Jank.Utilities;
using UnityEngine;

namespace Jank.Services.Experience
{
    /// <summary>
    /// Provides global control of experience related functionality so that they can be toggled
    /// </summary>
    /// <remarks>
    /// The point of the visual service is that it wraps calls with global options so that animations, sounds, etc can
    /// be modified globally. This is useful for global settings, and for setting up a training scene where visuals
    /// should not play
    /// </remarks>
    public interface IExperienceService
    {
        /// <summary>
        /// Perform an action that results in an update to an animator.
        /// </summary>
        /// <remarks>
        /// Obviously any action can be used here, but the control over whether this action is invoked will depend on
        /// the animation settings controlling this function
        /// </remarks>
        void Animate(Action action);

        /// <summary>
        /// Sets the trigger on the given animator if settings allow
        /// </summary>
        void SetAnimatorTrigger(Animator animator, string trigger);
        
        /// <summary>
        /// Plays an audio clip on the provided source modulating volume, or simply not playing depending on settings.
        /// </summary>
        void PlayAudioClip(AudioSource source, AudioClip clip);
        
        /// <summary>
        /// Plays an awaited action. This will be skipped if turned off in settings
        /// </summary>
        UniTask PlayAwaited(ActionAsync timedAction);
    }
}
