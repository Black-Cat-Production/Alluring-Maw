
using Lukas.Scripts.Core.Skills.Effects;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public sealed class SU_PoisonAttack : GenericSkill<PoisonAttack>
    {
        [SerializeField] EffectData effectData;

        protected override void Awake()
        {
            Skill = new PoisonAttack(name, baseSkillHitDamage, effectData);
            skillLogic = Skill;
            base.Awake();
        }
    }
}