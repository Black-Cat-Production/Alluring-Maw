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

        [Header("Optional Walk Sounds")]
        [SerializeField] AudioClip[] walkClips;

        [Header("Experimental Sound Settings")]
        [SerializeField] bool useExperimentalSettings;

        [SerializeField] AudioSource audioSource2;
        int currentAudioSourceToPlayAt;

        public void PlayIdleClip()
        {
            if (idleClips.Length == 0) return;
            PlayAudioClip(GetRandomClip(idleClips));
        }

        public void PlayDamageClip()
        {
            if (damageClips.Length == 0) return;
            PlayAudioClip(GetRandomClip(damageClips));
        }

        public void PlayAttackClip()
        {
            if (attackClips.Length == 0) return;
            PlayAudioClip(GetRandomClip(attackClips));
        }

        public void PlayWalkClip()
        {
            if (walkClips.Length == 0) return;
            PlayAudioClip(GetRandomClip(walkClips));
        }

        void PlayAudioClip(AudioClip _clip)
        {
            if (useExperimentalSettings)
            {
                switch (audioSource.isPlaying)
                {
                    case true when audioSource2.isPlaying:
                        return;
                    case false:
                        audioSource.clip = _clip;
                        audioSource.pitch = GetRandomPitch();
                        audioSource.Play();
                        break;
                    default:
                    {
                        if (!audioSource2.isPlaying)
                        {
                            audioSource2.clip = _clip;
                            audioSource2.pitch = GetRandomPitch();
                            audioSource2.Play();
                        }

                        break;
                    }
                }
            }
            else
            {
                audioSource.Stop();
                audioSource.clip = _clip;
                audioSource.pitch = GetRandomPitch();
                audioSource.Play();
            }
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