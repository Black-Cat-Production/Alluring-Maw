using System.Collections;
using Lukas.Scripts.Core.Modules;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills.Effects
{
    public class DamageOverTimeHandler : IEffectHandler
    {
        public void ApplyEffect(HealthSystemModule _target, Effect _effect)
        {
            if (!_effect.IsRunning)
            {
                _effect.IsRunning = true;
                EffectRunner.Instance.StartCoroutine(ApplyPeriodicDamage(_target, _effect));
            }
        }
        
        IEnumerator ApplyPeriodicDamage(HealthSystemModule _target, Effect _effect)
        {
            while (_effect.Duration > 0 && !_target.IsDead)
            {
                _target.TakeDamage(_effect.Intensity);
                if (_target == null || !_target.isActiveAndEnabled) break;
                if(_target.IsDead && _effect.Context != null) _effect.Context.OnEnemyKilled.Invoke(_target);
                yield return new WaitForSeconds(_effect.TickInterval);
            }

            _effect.IsRunning = false;
            Debug.Log("Coroutine Ended! Effect ran out and/or target died!");
        }
    }
}