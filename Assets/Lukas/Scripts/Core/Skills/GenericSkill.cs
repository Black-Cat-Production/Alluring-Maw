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
        [SerializeField] protected List<SkillBehaviorSO> baseBehaviours;
        Timer despawnTimer;
        
        
        protected virtual void Awake()
        {
            despawnTimer = new Timer(despawnTimerDuration);
            despawnTimer.StartTimer();
        }

        void Start()
        {
            foreach (var behaviour in baseBehaviours)
            {
                AddBehavior(behaviour);
            }
        }
        

        void FixedUpdate()
        {
            if (despawnTimer.CheckTimer()) Destroy(gameObject);
        }

        protected virtual void Use(SkillContext _context)
        {
            int totalDamage = baseSkillHitDamage;
            foreach (var behavior in behaviors)
            {
                behavior.Execute(_context, ref totalDamage);
            }

            if (_context.Targets.Count <= 0) return;
            var target = _context.Targets[0];
            target.HealthSystemModule.TakeDamage(totalDamage);
        }

        public virtual void OnTriggerEnter(Collider _collider)
        {
            if (_collider.TryGetComponent(out EnemyAIModule target))
            {
                var context = new SkillContext(transform.position, new List<EnemyAIModule> { target }, null);
                Use(context);
            }

            Destroy(gameObject);
        }
    }
}