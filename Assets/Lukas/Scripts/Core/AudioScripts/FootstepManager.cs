using System.Collections;
using Scripts.UserInput;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Scripts.Core.AudioScripts
{
    [RequireComponent(typeof(AudioSource))]
    public class FootstepManager : MonoBehaviour
    {
        [SerializeField] AudioClip[] audioClips;
        [SerializeField] float delayBetweenClips;

        AudioSource audioSource;
        InputController inputController;

        Button button;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            inputController = GetComponent<InputController>();
            StartCoroutine(FootstepRoutine());
        }

        IEnumerator FootstepRoutine()
        {
            while (true)
            {
                while (inputController.HasMoveInput)
                {
                    audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
                    if (!audioSource.isPlaying) audioSource.Play();
                    yield return new WaitForSeconds(delayBetweenClips);
                }

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}