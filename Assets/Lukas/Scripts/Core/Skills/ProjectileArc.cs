using Lukas.Scripts.Core.Modules;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public class ProjectileArc : Projectile
    {
       [SerializeField] LayerMask environmentLayer;
        protected void OnTriggerEnter(Collider _other)
        {
            if ((environmentLayer.value & (1 << _other.gameObject.layer)) != 0)
            {
                Destroy(gameObject);
            }
            else if (_other.gameObject.TryGetComponent(out HealthSystemModule healthSystem))
            {
                ApplyToTarget(healthSystem);
            }
        }

        protected override void OnCollisionEnter(Collision _hit)
        {
            
        }
    }
}