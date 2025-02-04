using System.Collections.Generic;
using Scripts.Core.Modules;
using Scripts.Core.Skills.Effects;
using UnityEngine;

namespace Scripts.Core.Skills.Behaviors
{
    [CreateAssetMenu(menuName = "Scriptables/Skills/BehaviorSO/RendTheFleshDebuffBehaviorSO")]
    public class RendTheFleshDebuffBehaviorSO : SkillBehaviorSO
    {
        [SerializeField] string specificName;
        [SerializeField] List<ESkillTag> tags;
        [SerializeField] EffectData effectData;
        [SerializeField] float healAmount;
        public override string SpecificName => specificName;
        public override List<ESkillTag> Tags => tags;

        SkillContext savedContext;
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

            _context.OnEnemyKilled += HealPlayer;
            if (_context.Targets.Count > 0)
            {
                var target = _context.Targets[0];
                target.HealthSystemModule.AddEffect(effect);
            }

            savedContext = _context;
        }

        void HealPlayer(EnemyAIModule _killedEnemy)
        {
            savedContext.CasterHealthModule.TakeDamage(-healAmount);
        }
    }
}