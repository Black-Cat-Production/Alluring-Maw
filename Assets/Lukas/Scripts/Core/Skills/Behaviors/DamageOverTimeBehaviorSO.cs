using System.Collections.Generic;
using Lukas.Scripts.Core.Modules;
using Lukas.Scripts.Core.Skills.Effects;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    [CreateAssetMenu(menuName = "Scriptables/Skills/BehaviorSO/DamageOverTimeBehaviorSO")]
    public class DamageOverTimeBehaviorSO : SkillBehaviorSO
    {
        [SerializeField] string specificName;
        [SerializeField] List<ESkillTag> tags;
        [SerializeField] EffectData effectData;

        public override string SpecificName => specificName;

        public override List<ESkillTag> Tags => tags;

        public override void Execute(SkillContext _context, ref int _totalDamage)
        {
            var effect = new Effect
            {
                Name = effectData.EffectName,
                Duration = effectData.EffectDuration,
                Intensity = effectData.EffectIntensity,
                TickInterval = effectData.EffectTickInterval,
                Type = effectData.EffectType,
                Context = _context
            };
            _context.Effect = effect;
            
            if (_context.Targets.Count > 0)
            {
                var target = _context.Targets[0];
                target.AddEffect(effect);
            }
        }
    }
}