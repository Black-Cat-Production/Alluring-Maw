using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Core.Skills.Behaviors
{
    [CreateAssetMenu(menuName = "Scriptables/Skills/BehaviorSO/HitDamageIncreaseBehavior")]
    public class HitDamageIncreaseBehaviorSO : SkillBehaviorSO
    {
        [SerializeField] string specificName;
        [SerializeField] List<ESkillTag> tags;
        public int damageIncrease;

        public override string SpecificName => specificName;

        public override List<ESkillTag> Tags => tags;

        public override void Execute(SkillContext _context, ref int _totalDamage)
        {
            _totalDamage += damageIncrease;
        }
    }
}