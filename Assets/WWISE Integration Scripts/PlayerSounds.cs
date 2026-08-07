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
    }
}