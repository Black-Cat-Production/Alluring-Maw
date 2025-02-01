using System.Collections;
using Lukas.Scripts.Core.Modules;
using Lukas.Scripts.Core.Skills.Effects;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills.Behaviors
{
    public class RendTheFleshEffectHandler : IEffectHandler
    {
        public void ApplyEffect(EnemyAIModule _target, Effect _effect)
        {
            if (_effect.IsRunning) return;
            _effect.IsRunning = true;
            EffectRunner.Instance.StartCoroutine(ApplyDebuff(_target, _effect));

        }

        IEnumerator ApplyDebuff(EnemyAIModule _target, Effect _effect)
        {
            while (_effect.Duration > 0 && !_target.HealthSystemModule.IsDead)
            {
                _target.UpdateMoveSpeed(-_effect.Intensity);
                _target.UpdateAttackDamage(-_effect.Intensity);
                yield return new WaitForSeconds(_effect.TickInterval);
            }
            _target.ResetAttackDamage();
            _target.ResetMoveSpeed();
            _effect.Context.TriggerEnemyKilled(_target);
        }
    }
}