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

        void Awake()
        {
            mainUIObject.SetActive(false);
            StartCoroutine(LerpColorUp());
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
    }
}