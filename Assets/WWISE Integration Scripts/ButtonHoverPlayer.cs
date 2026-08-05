using UnityEngine;
using UnityEngine.EventSystems;
using Event = AK.Wwise.Event;

namespace WWISE_Integration_Scripts
{
    public class ButtonHoverPlayer : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] Event eventToPlay;


        public void OnPointerEnter(PointerEventData _eventData)
        {
            AkSoundEngine.PostEvent(eventToPlay.Name, gameObject);
        }
    }
}