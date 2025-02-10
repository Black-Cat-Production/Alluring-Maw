using System.Collections.Generic;
using Scripts.Core.Modules;
using UnityEngine;

namespace Scripts.Core.Skills
{
    public class SU_LightBeamAttack : SkillBase
    {
        SkillContext context;
        bool hitEnemy;
        bool hitGround;


        protected void Start()
        {
            context = new SkillContext(transform.position, null, null, this);
            OnSpawn.Invoke(gameObject.transform.position);
        }

        public override void OnTriggerEnter(Collider _collider)
        {
            if (_collider.gameObject.CompareTag("HitBox"))
            {
                var target = _collider.gameObject.GetComponentInParent<EnemyAIModule>();
                context.Targets = new List<EnemyAIModule> { target };
            }
            else
            {
                context.Targets = null;
            }
            Use(context);
        }
    }
}