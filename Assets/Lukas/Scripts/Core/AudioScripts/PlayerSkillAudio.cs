using UnityEngine;

namespace Scripts.Core.AudioScripts
{
    public class PlayerSkillAudio : MonoBehaviour
    {
        [SerializeField] AudioClip chargeSkillAudio;
        [SerializeField] AudioClip releaseSkillAudio;
        [SerializeField] AudioSource playerArmAudioSource;

        public void PlayChargeAudio()
        {
            playerArmAudioSource.Stop();
            playerArmAudioSource.clip = chargeSkillAudio;
            playerArmAudioSource.Play();
        }

        public void PlayReleaseAudio()
        {
            playerArmAudioSource.Stop();
            playerArmAudioSource.clip = releaseSkillAudio;
            playerArmAudioSource.Play();
        }
    }
}