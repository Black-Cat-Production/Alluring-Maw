using System;
using Scripts.Core.Modules;
using UnityEngine;

namespace Scripts.Core.KI
{
    public class AttackCollider : MonoBehaviour
    {
        public Action<HealthSystemModule> OnHit;

        void OnTriggerEnter(Collider _target)
        {
            if (_target.TryGetComponent(out HealthSystemModule healthSystemModule)) OnHit.Invoke(healthSystemModule);
        }
    }
}