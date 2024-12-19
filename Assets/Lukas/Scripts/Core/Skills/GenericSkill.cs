using System;
using System.Collections.Generic;
using LL_Unity_Utils.Timers;
using Lukas.Scripts.Core.Modules;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public abstract class GenericSkill<T> : SkillBridgeUnity where T : ISkill
    {
        public T Skill { get; protected set; }
        [SerializeField] float despawnTimerDuration;
        [SerializeField] protected float baseSkillHitDamage;
        [SerializeField] protected LayerMask obstructionLayer;
        Timer despawnTimer;
        protected ISkill skillLogic;


        public void Initialize(ISkill _skillLogic)
        {
            skillLogic = _skillLogic;
        }

        protected virtual void Awake()
        {
            despawnTimer = new Timer(despawnTimerDuration);
            despawnTimer.StartTimer();
            skillLogic = Skill;
        }

        void FixedUpdate()
        {
            if (despawnTimer.CheckTimer()) Destroy(gameObject);
        }

        public virtual void OnTriggerEnter(Collider _collider)
        {
            if (_collider.TryGetComponent(out HealthSystemModule target))
            {
                skillLogic.Use(transform.position, new List<HealthSystemModule> { target });
            }

            Destroy(gameObject);
        }
    }
}