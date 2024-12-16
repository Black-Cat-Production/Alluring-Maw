using Lukas.Scripts.Core.Skills.Effects;

namespace Lukas.Scripts.Core.Skills
{
    public class PoisonProjectile : Projectile
    {

        readonly Effect poisonEffect = new Effect()
        {
            Name = "Poison",
            Duration = 5f,
            Intensity = 1f,
            TickInterval = 1f,
            Type = EffectType.DamageOverTime
        };

        protected override void ApplyToTarget(HealthSystemModule _target)
        {
            _target.AddEffect(poisonEffect);
        }
    }
}