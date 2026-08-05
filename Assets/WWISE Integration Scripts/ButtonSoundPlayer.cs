using UnityEngine;

namespace WWISE_Integration_Scripts
{
    public class ButtonSoundPlayer : MonoBehaviour
    {
        [SerializeField] AK.Wwise.Event eventToPlay;
        [SerializeField] GameObject gameObjectToPlayOn;


        public void OnClicked()
        {
            AkSoundEngine.PostEvent(eventToPlay.Name, gameObjectToPlayOn == null ? gameObject : gameObjectToPlayOn);
        }
    }
}
