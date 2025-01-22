using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lukas.Scripts.Core.Visual
{
    public class MaterialHandler : MonoBehaviour
    {
        [SerializeField] List<SkinnedMeshRenderer> meshRenderes;
        [SerializeField] Material defaultProtagMaterial;
        [SerializeField] Material lightProtagMaterial;
        [SerializeField] Material darkProtagMaterial;


        public void UpdateMaterialOnMeshes(EMaterialType _materialType)
        {
            var selectedMaterial = _materialType switch
            {
                EMaterialType.DefaultProtag => defaultProtagMaterial,
                EMaterialType.LightProtag => lightProtagMaterial,
                EMaterialType.DarkProtag => darkProtagMaterial,
                _ => throw new ArgumentOutOfRangeException(nameof(_materialType), _materialType, null)
            };
            foreach (var skinnedMeshRenderer in meshRenderes) skinnedMeshRenderer.material = selectedMaterial;
        }
    }
}