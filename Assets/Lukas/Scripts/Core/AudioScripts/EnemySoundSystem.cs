using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Core.AudioScripts
{
    [RequireComponent(typeof(AudioSource))]
    public class EnemySoundSystem : MonoBehaviour
    {
        [SerializeField] AudioClip[] idleClips;
        [SerializeField] AudioClip[] damageClips;
        [SerializeField] AudioClip[] attackClips;
        [SerializeField] AudioSource audioSource;

        [Header("AudioSettings")]
        [SerializeField] float minAudioPitch;
        [SerializeField] float maxAudioPitch;
        
        public void PlayIdleClip()
        {
            PlayAudioClip(GetRandomClip(idleClips));
        }

        public void PlayDamageClip()
        {
            PlayAudioClip(GetRandomClip(damageClips));
        }

        public void PlayAttackClip()
        {
            PlayAudioClip(GetRandomClip(attackClips));
        }

        void PlayAudioClip(AudioClip _clip)
        {
            audioSource.Stop();
            audioSource.clip = _clip;
            audioSource.pitch = GetRandomPitch();
            audioSource.Play();
        }

        public bool GetIsPlaying()
        {
            return audioSource.isPlaying;
        }

        float GetRandomPitch()
        {
            return Random.Range(minAudioPitch, maxAudioPitch);
        }


        AudioClip GetRandomClip(AudioClip[] _array)
        {
            return _array[Random.Range(0, _array.Length)];
        }
    }
}