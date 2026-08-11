using UnityEngine;
using Event = AK.Wwise.Event;

namespace WWISE_Integration_Scripts
{
    public class PlayerSounds : MonoBehaviour
    {
        [Header("Event Links")]
        [SerializeField] Event playerHitEvent;
        [SerializeField] Event playerDeathEvent;
        [SerializeField] Event playerFootstepEvent;
        [SerializeField] Event playerDashEvent;
        [SerializeField] Event playerAttackLMCEvent;
        [SerializeField] Event playerChargeLightEvent;
        [SerializeField] Event playerChargeDarkEvent;
        [SerializeField] Event playerHeartbeatStopEvent;
        [SerializeField] Event playerSkillSelectionSoundEvent;
        [SerializeField] Event playerStartDoorCutsceneSoundEvent;

        [Header("Sound Settings")]
        [SerializeField] public float FootstepInterval;
        
        void PlayEvent(Event _event)
        {
            AkSoundEngine.PostEvent(_event.Name, gameObject);
        }
        
        public void PlayHitEvent()
        {
            PlayEvent(playerHitEvent);
        }

        public void PlayDeathEvent()
        {
            PlayEvent(playerDeathEvent);
            PlayEvent(playerHeartbeatStopEvent);
        }

        public void PlayFootstepEvent()
        {
            PlayEvent(playerFootstepEvent);
        }

        public void PlayDashEvent()
        {
            PlayEvent(playerDashEvent);
        }

        public void PlayLMCEvent()
        {
            PlayEvent(playerAttackLMCEvent);
        }

        public void PlayChargeEventLight()
        {
            PlayEvent(playerChargeLightEvent);
        }

        public void PlayChargeDarkEvent()
        {
            PlayEvent(playerChargeDarkEvent);
        }

        public void PlaySkillSelectionSound()
        {
            PlayEvent(playerSkillSelectionSoundEvent);
        }

        public void PlayDoorCutsceneSound()
        {
            PlayEvent(playerStartDoorCutsceneSoundEvent);
        }
    }
}