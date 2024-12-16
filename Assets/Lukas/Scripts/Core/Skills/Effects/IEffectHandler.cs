namespace Lukas.Scripts.Core.Skills.Effects
{
    public interface IEffectHandler
    {
        public void ApplyEffect(HealthSystemModule _target, Effect _effect);
    }
}