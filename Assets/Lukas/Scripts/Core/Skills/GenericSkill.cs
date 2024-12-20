using System;
using System.Collections.Generic;
using LL_Unity_Utils.Timers;
using Lukas.Scripts.Core.Modules;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public abstract class GenericSkill<T> : SkillBridgeUnity where T : ISkill
    {
        [SerializeField] float despawnTimerDuration;
        [SerializeField] protected int baseSkillHitDamage;
        [SerializeField] protected LayerMask obstructionLayer;
        Timer despawnTimer;
        
        protected virtual void Awake()
        {
            despawnTimer = new Timer(despawnTimerDuration);
            despawnTimer.StartTimer();
        }

        void FixedUpdate()
        {
            if (despawnTimer.CheckTimer()) Destroy(gameObject);
        }

        protected virtual void Use(Vector3 _startPosition, List<HealthSystemModule> _enemiesInRange)
        {
            int totalDamage = baseSkillHitDamage;
            foreach (var behavior in behaviors)
            {
                behavior.Execute(_startPosition, _enemiesInRange, ref totalDamage);
            }

            if (_enemiesInRange.Count <= 0) return;
            var target = _enemiesInRange[0];
            target.TakeDamage(totalDamage);
        }

        public virtual void OnTriggerEnter(Collider _collider)
        {
            if (_collider.TryGetComponent(out HealthSystemModule target))
            {
                Use(transform.position, new List<HealthSystemModule> { target });
            }

            Destroy(gameObject);
        }
    }
}