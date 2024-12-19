using System.Collections.Generic;
using Lukas.Scripts.Core.Modules;
using Lukas.Scripts.Core.Skills.Effects;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public class PoisonAttack : ISkill
    {
        readonly Effect effect;

        public string Name { get; private set; }
        public float Damage { get; private set; }

        public PoisonAttack(string _name, float _damage, EffectData _effectData)
        {
            Name = _name;
            Damage = _damage;
            effect = new Effect()
            {
                Name = _effectData.EffectName,
                Duration = _effectData.EffectDuration,
                Intensity = _effectData.EffectIntensity,
                TickInterval = _effectData.EffectTickInterval,
                Type = _effectData.EffectType
            };
        }

        public void Use(Vector3 _startPosition, List<HealthSystemModule> _enemiesInRange)
        {
            if (_enemiesInRange.Count > 0)
            {
                var target = _enemiesInRange[0];
                target.AddEffect(effect);
            }
        }
    }
}