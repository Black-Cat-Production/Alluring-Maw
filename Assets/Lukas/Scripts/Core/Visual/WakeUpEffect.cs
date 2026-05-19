using System;
using System.Collections;
using Scripts.Core.UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Scripts.Core.Visual
{
    public class WakeUpEffect : MonoBehaviour
    {
        [SerializeField] Volume volume;
        [SerializeField] GameObject mainUIObject;
        [SerializeField] Color startColor;
        [SerializeField] Color endColor;
        [SerializeField] float lerpDuration;
        //[SerializeField] AudioSource bgmSource;
        [SerializeField] TutorialUI tutorialUI;

        public bool IsDoneBlackout { get; private set; }

        void Awake()
        {
            mainUIObject.SetActive(false);
           // bgmSource.volume = 0;
            StartCoroutine(LerpColorUp());
            StartCoroutine(LerpSoundUp());
        }

        void OnDisable()
        {
            StopAllCoroutines();
        }

        public void Blackout()
        {
            StartCoroutine(LerpSoundDown());
            StartCoroutine(LerpColorDown());
        }

        IEnumerator LerpColorUp()
        {
            float timer = 0f;
            volume.profile.TryGet(out ColorAdjustments colorAdjustments);
            while (timer < lerpDuration)
            {
                timer += Time.deltaTime;
                float t = timer / lerpDuration;
                colorAdjustments.colorFilter.Interp(startColor, endColor, t);
                yield return null;
            }

            mainUIObject.SetActive(true);
            tutorialUI.StartTutorial();
        }

        IEnumerator LerpSoundUp()
        {
            float timer = 0f;
            while (timer < lerpDuration)
            {
                timer += Time.deltaTime;
                float t = timer / lerpDuration;
                //bgmSource.volume = Mathf.Lerp(0f, 0.5f, t);
                yield return null;
            }
        }

        IEnumerator LerpSoundDown()
        {
            float timer = 0f;
            while (timer < lerpDuration)
            {
                timer += Time.deltaTime;
                float t = timer / lerpDuration;
                //bgmSource.volume = Mathf.Lerp(0.5f, 0, t);
                yield return null;
            }
        }

        IEnumerator LerpColorDown()
        {
            float timer = 0f;
            volume.profile.TryGet(out ColorAdjustments colorAdjustments);
            while (timer < lerpDuration)
            {
                timer += Time.deltaTime;
                float t = timer / lerpDuration;
                colorAdjustments.colorFilter.Interp(endColor, startColor, t);
                yield return null;
            }

            IsDoneBlackout = true;
        }
    }
}