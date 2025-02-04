using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Core.Skills.Effects;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Core.Modules
{
    public class HealthSystemModule : MonoBehaviour
    {
        [SerializeField] float maxHealth;
        bool isDead;

        EnemyAIModule owner;
        public float MaxHealth => maxHealth;
        public float CurrentHealth { get; private set; }

        public bool IsDead => isDead;

        public UnityEvent OnDeathEvent;
        public Action<int> OnDamageTaken;

        //Effect Management
        readonly List<Effect> activeEffects = new();
        readonly Dictionary<EffectType, IEffectHandler> effectHandlers = new();

        //Tag Management
        [SerializeField] bool isBoss;
        public bool IsBoss => isBoss;

        public void AddEffect(Effect _effect)
        {
            var existingEffect = activeEffects.FirstOrDefault((_checkedEffect) => _checkedEffect.Name == _effect.Name);
            if (existingEffect != null)
                existingEffect.Duration = _effect.Duration;
            //Debug.Log($"Refreshed effect: {_effect.Name} on enemy {gameObject.name}!");
            else
                activeEffects.Add(_effect);
            //Debug.Log($"Added effect: {_effect.Name} to enemy {gameObject.name}!");
        }

        public void RegisterEffectHandler(EffectType _type, IEffectHandler _handler)
        {
            effectHandlers.TryAdd(_type, _handler);
        }

        void Awake()
        {
            owner = GetComponent<EnemyAIModule>();
            CurrentHealth = maxHealth;
        }

        void FixedUpdate()
        {
            for (int i = activeEffects.Count - 1; i >= 0; i--)
            {
                var effect = activeEffects[i];
                effect.Duration -= Time.deltaTime;

                if (effectHandlers.TryGetValue(effect.Type, out var handler)) handler.ApplyEffect(owner, effect);

                if (effect.Duration <= 0) activeEffects.RemoveAt(i);
            }
        }

        public void TakeDamage(float _damageAmount)
        {
            CurrentHealth = Mathf.Max(0, CurrentHealth - _damageAmount);
            Debug.Log($"{CurrentHealth}");
            OnDamageTaken?.Invoke((int)_damageAmount);
            if (CurrentHealth != 0) return;
            isDead = true;
            OnDeathEvent.Invoke();
        }

        public float GetCurrentPercentageHealth()
        {
            return CurrentHealth / maxHealth * 100;
        }
    }
}