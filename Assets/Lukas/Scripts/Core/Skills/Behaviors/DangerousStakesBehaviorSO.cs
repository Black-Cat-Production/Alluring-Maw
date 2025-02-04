using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Core.Skills.Behaviors
{
    [CreateAssetMenu(menuName = "Scriptables/Skills/BehaviorSO/DangerousStakesBehaviorSO")]
    public class DangerousStakesBehaviorSO : SkillBehaviorSO
    {
        [SerializeField] string specificName;
        [SerializeField] List<ESkillTag> tags;

        [SerializeField] float triggerChance;
        public override string SpecificName => specificName;

        public override List<ESkillTag> Tags => tags;

        public override void Execute(SkillContext _context, ref int _totalDamage)
        {
            foreach (var target in _context.Targets)
            {
                float randomNumber = Random.Range(0f, 100f);
                if (!(randomNumber <= triggerChance) || target.HealthSystemModule.IsBoss) continue;
                _context.CasterHealthModule.TakeDamage(10);
                target.HealthSystemModule.TakeDamage(9999);
            }
        }
    }
}