using System.Collections.Generic;
using Lukas.Scripts.Core.Modules;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public sealed class SU_ArcAttack : GenericSkill<ArcAttack>
    {
        //protected override void Awake()
        //{
        //    Skill = new ArcAttack(name, baseSkillHitDamage);
        //    base.Awake();
        //}

        public override void OnTriggerEnter(Collider _collider)
        {
            if ((obstructionLayer.value & (1 << _collider.gameObject.layer)) != 0)
            {
                Destroy(gameObject);
                return;
            }
            if (_collider.TryGetComponent(out HealthSystemModule target))
            {
                var context = new SkillContext(transform.position, new List<HealthSystemModule> { target }, null);
                Use(context);
            }
        }
    }
}