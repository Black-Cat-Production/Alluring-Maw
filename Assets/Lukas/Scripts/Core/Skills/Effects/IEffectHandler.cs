using Scripts.Core.Modules;

namespace Scripts.Core.Skills.Effects
{
    public interface IEffectHandler
    {
        public void ApplyEffect(EnemyAIModule _target, Effect _effect);
    }
}