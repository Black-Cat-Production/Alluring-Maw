using UnityEngine;
using UnityEngine.EventSystems;
using NotImplementedException = System.NotImplementedException;

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

        public void OnPointerEnter(PointerEventData eventData)
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