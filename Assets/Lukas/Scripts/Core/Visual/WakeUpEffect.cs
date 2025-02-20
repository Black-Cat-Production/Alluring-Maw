using System;
using System.Collections;
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
        [SerializeField] AudioSource bgmSource;

        void Awake()
        {
            mainUIObject.SetActive(false);
            bgmSource.volume = 0;
            StartCoroutine(LerpColorUp());
            StartCoroutine(LerpSoundUp());
        }

        void OnDisable()
        {
            StopAllCoroutines();
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
                Debug.Log(t);
                yield return null;
            }
            mainUIObject.SetActive(true);
        }

        IEnumerator LerpSoundUp()
        {
            float timer = 0f;
            while (timer < lerpDuration)
            {
                timer += Time.deltaTime;
                float t = timer / lerpDuration;
                bgmSource.volume = Mathf.Lerp(0f, 0.5f, t);
                Debug.Log(t);
                yield return null;
            }
        }
    }
}