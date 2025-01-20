using System.Collections.Generic;
using Lukas.Scripts.Core.Modules;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public sealed class SU_ArcAttack : SkillBase
    {
        public override void OnTriggerEnter(Collider _collider)
        {
            if ((obstructionLayer.value & (1 << _collider.gameObject.layer)) != 0)
            {
                Destroy(gameObject);
                return;
            }
            if (!_collider.gameObject.CompareTag("HitBox")) return;
            var target = _collider.gameObject.GetComponentInParent<EnemyAIModule>();
            var context = new SkillContext(transform.position, new List<EnemyAIModule> { target }, null);
            Use(context);
        }
    }
}