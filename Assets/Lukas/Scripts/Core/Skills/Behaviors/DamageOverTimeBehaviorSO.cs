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
        [SerializeField] List<string> tags;
        [SerializeField] EffectData effectData;

        public override string SpecificName => specificName;

        public override List<string> Tags => tags;

        public override void Execute(Vector3 _startPosition, List<HealthSystemModule> _enemiesInRange, ref int _totalDamage)
        {
            var effect = new Effect
            {
                Name = effectData.EffectName,
                Duration = effectData.EffectDuration,
                Intensity = effectData.EffectIntensity,
                TickInterval = effectData.EffectTickInterval,
                Type = effectData.EffectType
            };
            
            if (_enemiesInRange.Count > 0)
            {
                var target = _enemiesInRange[0];
                target.AddEffect(effect);
            }
        }
    }
}