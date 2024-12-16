using System;
using UnityEngine;
using UnityEngine.Events;

namespace Lukas.Scripts.Core
{
    public class HealthSystemModule : MonoBehaviour
    {
        [SerializeField] float maxHealth;
        bool isDead;

        public float MaxHealth => maxHealth;
        public float CurrentHealth { get; private set; }

        public bool IsDead => isDead;

        public UnityEvent OnDeathEvent;

        void Awake()
        {
            CurrentHealth = maxHealth;
        }

        public void TakeDamage(float _damageAmount)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, CurrentHealth - _damageAmount);
            Debug.Log(CurrentHealth);
            if (CurrentHealth != 0) return;
            isDead = true;
            OnDeathEvent.Invoke();
        }
    }
}