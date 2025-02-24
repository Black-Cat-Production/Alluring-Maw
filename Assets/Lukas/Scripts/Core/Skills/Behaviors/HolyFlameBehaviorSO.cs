using System.Collections.Generic;
using Scripts.Core.Skills.Effects;
using UnityEngine;

namespace Scripts.Core.Skills.Behaviors
{
    [CreateAssetMenu(menuName = "Scriptables/Skills/BehaviorSO/HolyFlameBehaviorSO")]
    public class HolyFlameBehaviorSO : SkillBehaviorSO
    {
        [SerializeField] string specificName;
        [SerializeField] List<ESkillTag> tags;
        [SerializeField] EffectData effectData;
        public override string SpecificName => specificName;

        public override List<ESkillTag> Tags => tags;

        public override void Execute(SkillContext _context, ref int _totalDamage)
        {
            _totalDamage = 0;

            var effect = new Effect
            {
                Name = effectData.EffectName,
                Duration = effectData.EffectDuration,
                Intensity = effectData.EffectIntensity,
                IntensityIncrease = effectData.EffectIntensityIncrease,
                TickInterval = effectData.EffectTickInterval,
                Type = effectData.EffectType,
                VFXSpawner = effectData.EffectSpawner,
                Context = _context
            };
            _context.Effect = effect;

            if (_context.Targets.Count > 0)
            {
                var target = _context.Targets[0];
                target.HealthSystemModule.AddEffect(effect);
            }
        }
    }
}