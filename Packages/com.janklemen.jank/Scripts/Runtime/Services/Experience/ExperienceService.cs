using System;
using Cysharp.Threading.Tasks;
using Jank.Utilities;
using UnityEngine;

namespace Jank.Services.Experience
{
    /// <summary>
    /// Represents a service for managing experience related functionality including turning audio on and off, or
    /// playing animations.
    /// </summary>
    [CreateAssetMenu(menuName = "Jank/Services/TimedAction", fileName = "TimedActionService")]
    public class ExperienceService : AExperienceService
    { 
        [SerializeField] bool _playAwaited;
        [SerializeField] bool _playAnimations;
        
        [SerializeField] bool _playAudio;
        [SerializeField] float _volumeAudio;

        /// <inheritdoc />
        public override void Animate(Action action)
        {
            if (_playAnimations)
                action();
        }

        /// <inheritdoc />
        public override void SetAnimatorTrigger(Animator animator, string trigger)
        {
            if (_playAnimations)
            {
                animator.SetTrigger(trigger);
            }
        }

        /// <inheritdoc />
        public override void PlayAudioClip(AudioSource source, AudioClip clip)
        {
            if (_playAudio)
            {
                source.clip = clip;
                source.volume = _volumeAudio;
                source.Play();
            }
        }

        /// <inheritdoc />
        public override async UniTask PlayAwaited(ActionAsync timedAction)
        {
            if (_playAwaited)
                await timedAction();
        }
    }
}