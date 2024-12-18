using LL_Unity_Utils.Timers;
using Lukas.Scripts.Core.Modules;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public class Projectile : Skill
    {
        [SerializeField] float despawnTimerDuration;
        Timer despawnTimer;
        bool hasHit;


        //Only for debugging
        [SerializeField] int hitDamageValue;

        protected virtual void Awake()
        {
            despawnTimer = new Timer(despawnTimerDuration);
            despawnTimer.StartTimer();
        }

        void FixedUpdate()
        {
            if (despawnTimer.CheckTimer()) Destroy(gameObject);
        }

        protected virtual void ApplyToTarget(HealthSystemModule _target)
        {
            _target.TakeDamage(hitDamageValue);
        }

        protected virtual void OnCollisionEnter(Collision _hit)
        {
            if (!hasHit) hasHit = true;
            else Destroy(gameObject);
            if (_hit.gameObject.TryGetComponent(out HealthSystemModule healthSystem))
            {
                ApplyToTarget(healthSystem);
            }
            Destroy(gameObject);
        }
    }
}