using System.Collections;
using Scripts.Core.Modules;
using UnityEngine;

namespace Scripts.Core.Skills.Effects
{
    public class DamageOverTimeScalingHandler : IEffectHandler
    {
        public void ApplyEffect(EnemyAIModule _target, Effect _effect)
        {
            if (_effect.IsRunning) return;
            _effect.IsRunning = true;
            EffectRunner.Instance.StartCoroutine(ApplyPeriodicDamage(_target, _effect));
        }

        IEnumerator ApplyPeriodicDamage(EnemyAIModule _target, Effect _effect)
        {
            float actualDamage = _effect.Intensity;
            while (_effect.Duration > 0 && !_target.HealthSystemModule.IsDead)
            {
                _target.HealthSystemModule.TakeDamage(actualDamage);
                if (_target.HealthSystemModule.IsDead && _effect.Context != null) _effect.Context.TriggerEnemyKilled(_target);
                if (_target == null || !_target.isActiveAndEnabled) break;
                yield return new WaitForSeconds(_effect.TickInterval);
                actualDamage += _effect.IntensityIncrease;
            }

            _effect.IsRunning = false;
            Debug.Log("Coroutine Ended! Effect ran out and/or target died!");
        }
    }
}