using System;
using UnityEngine;
using UnityEngine.Events;

namespace Lukas.Scripts.Core
{
    public class HealthSystemModule : MonoBehaviour
    {
        [SerializeField] float maxHealth;
        float currentHealth;
        bool isDead;

        public float MaxHealth => maxHealth;
        public float CurrentHealth => currentHealth;
        public bool IsDead => isDead;

        public UnityEvent OnDeathEvent;

        void Awake()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float _damageAmount)
        {
            currentHealth = Mathf.Clamp(currentHealth, 0, currentHealth - _damageAmount);
            if (currentHealth != 0) return;
            isDead = true;
            OnDeathEvent.Invoke();
        }
    }
}