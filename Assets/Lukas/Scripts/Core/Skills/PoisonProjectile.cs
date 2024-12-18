using Lukas.Scripts.Core.Modules;
using Lukas.Scripts.Core.Skills.Effects;

namespace Lukas.Scripts.Core.Skills
{
    public class PoisonProjectile : Projectile
    {
        Effect effect;
        protected void Start()
        {
            effect = new Effect()
            {
                Name = effectData.EffectName,
                Duration = effectData.EffectDuration,
                Intensity = effectData.EffectIntensity,
                TickInterval = effectData.EffectTickInterval,
                Type = effectData.EffectType
            };
        }

        protected override void ApplyToTarget(HealthSystemModule _target)
        {
            _target.AddEffect(effect);
        }
    }
}