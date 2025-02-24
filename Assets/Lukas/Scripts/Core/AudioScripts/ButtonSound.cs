using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Core.AudioScripts
{
    [RequireComponent(typeof(AudioSource))]
    public class ButtonSound : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] AudioClip hoverSound;
        [SerializeField] AudioClip clickSound;

        AudioSource audioSource;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void OnPointerEnter(PointerEventData _eventData)
        {
            audioSource.Stop();
            audioSource.clip = hoverSound;
            audioSource.Play();
        }

        public void PlayClickSound()
        {
            audioSource.Stop();
            audioSource.clip = clickSound;
            audioSource.Play();
        }
    }
}