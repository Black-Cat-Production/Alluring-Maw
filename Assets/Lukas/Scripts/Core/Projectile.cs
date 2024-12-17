using System;
using UnityEngine;
using LL_Unity_Utils.Timers;

namespace Lukas.Scripts.Core
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