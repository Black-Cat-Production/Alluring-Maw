using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lukas.Scripts.Core.Visual
{
    public class MaterialHandler : MonoBehaviour
    {
        [SerializeField] Material protagMaterial;
        [SerializeField] float changePerFrame;

        static readonly int energyValue = Shader.PropertyToID("_EnergyValue");

        void Awake()
        {
            protagMaterial.SetFloat(energyValue, 0);
        }

        public void UpdateMaterialOnMeshes(EMaterialType _materialType)
        {
            StopAllCoroutines();
            _ = _materialType switch
            {
                EMaterialType.DefaultProtag => StartCoroutine(ApplyMaterialChanges(0)),
                EMaterialType.LightProtag => StartCoroutine(ApplyMaterialChanges(1)),
                EMaterialType.DarkProtag => StartCoroutine(ApplyMaterialChanges(-1)),
                _ => throw new ArgumentOutOfRangeException(nameof(_materialType), _materialType, null)
            };
        }

        IEnumerator ApplyMaterialChanges(float _goal)
        {
            while (Math.Abs(protagMaterial.GetFloat(energyValue) - _goal) > 0.001)
            {
                float currentValue = protagMaterial.GetFloat(energyValue);
                if (Math.Abs(currentValue - _goal) < 0.001) yield break;
                switch (_goal)
                {
                    case 0 when Math.Abs(currentValue - _goal) > 0.001:
                    {
                        if (currentValue > 0) ApplyDarkEnergy(changePerFrame);
                        else ApplyLightEnergy(changePerFrame);
                        break;
                    }
                    case -1:
                        ApplyDarkEnergy(changePerFrame);
                        break;
                    case 1:
                        ApplyLightEnergy(changePerFrame);
                        break;
                }

                yield return null;
            }
        }


        void ApplyDarkEnergy(float _change)
        {
            ApplyLightEnergy(-_change);
        }

        void ApplyLightEnergy(float _change)
        {
            float calculatedFloat = protagMaterial.GetFloat(energyValue) + _change;
            Debug.Log(calculatedFloat);
            protagMaterial.SetFloat(energyValue, calculatedFloat);
        }
    }
}