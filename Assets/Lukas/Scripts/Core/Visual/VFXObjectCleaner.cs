using System;
using UnityEngine;

namespace Scripts.Core.Visual
{
    public class VFXObjectCleaner : MonoBehaviour
    {
        [SerializeField] ParticleSystem mainParticleSystem;

        void FixedUpdate()
        {
            if(mainParticleSystem == null || mainParticleSystem.isEmitting == false)
                Destroy(gameObject);
        }
    }
}