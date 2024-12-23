using Lukas.Scripts.Core.Modules;

namespace Lukas.Scripts.Core.Skills.Effects
{
    public interface IEffectHandler
    {
        public void ApplyEffect(EnemyAIModule _target, Effect _effect);
    }
}