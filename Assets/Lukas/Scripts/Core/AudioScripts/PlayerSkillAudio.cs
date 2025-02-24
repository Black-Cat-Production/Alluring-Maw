using UnityEngine;

namespace Scripts.Core.AudioScripts
{
    public class PlayerSkillAudio : MonoBehaviour
    {
        [SerializeField] AudioSource playerDashAudioSource;

        public void PlayDashAudio()
        {
            playerDashAudioSource.Stop();
            playerDashAudioSource.Play();
        }
    }
}