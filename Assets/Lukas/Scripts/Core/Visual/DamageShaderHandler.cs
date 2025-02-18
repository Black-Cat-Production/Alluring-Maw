using System.Collections;
using UnityEngine;

namespace Scripts.Core.Visual
{
    public class DamageShaderHandler : MonoBehaviour
    {
        [SerializeField] Material damageShaderMaterial;
        [SerializeField] float flashDuration = 0.5f;

        Coroutine damageRoutine;
        static readonly int damageIntensity = Shader.PropertyToID("_Damage_Intensity");

        public void HitTaken()
        {
           StopAllCoroutines();
           damageShaderMaterial.SetFloat(damageIntensity,0);
           damageRoutine = StartCoroutine(ShowHitShader());
        }

        IEnumerator ShowHitShader()
        {
            damageShaderMaterial.SetFloat(damageIntensity, 0);
            float elapsedTime = 0f;
            while (elapsedTime < flashDuration)
            {
                float t = elapsedTime / flashDuration;
                float interpolationValue = Mathf.Lerp(0, 1, t);
                damageShaderMaterial.SetFloat(damageIntensity, interpolationValue);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0f;
            while (elapsedTime < flashDuration)
            {
                float t = elapsedTime / flashDuration;
                float interpolationValue = Mathf.Lerp(1, 0, t);
                damageShaderMaterial.SetFloat(damageIntensity, interpolationValue);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            damageRoutine = null;
        }
        
    }
}