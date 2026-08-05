using UnityEngine;
using Event = AK.Wwise.Event;

namespace WWISE_Integration_Scripts
{
    public class TorchSoundStopScript : MonoBehaviour
    {
        [SerializeField] Event eventToPlay;
        [SerializeField] GameObject torch1;
        [SerializeField] GameObject torch2;
        
        public void OnClicked()
        {
            AkSoundEngine.PostEvent(eventToPlay.Name, torch1);
            AkSoundEngine.PostEvent(eventToPlay.Name, torch2);
        }
    }
}