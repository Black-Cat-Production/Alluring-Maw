using System;
using System.Collections;
using UnityEngine;

namespace Scripts.Core.Modules
{
    public class ManaSystemModule : MonoBehaviour
    {
        [SerializeField] int maximumMana;
        [SerializeField] float manaRegenerationPerTick;
        [SerializeField] float manaRegenerationTickTimeInSeconds;

        float currentMana;
        public Action<float, int> OnManaChanged;

        public float CurrentMana
        {
            get => currentMana;
            private set
            {
                if (value == currentMana) return;
                OnManaChanged.Invoke(value, MaximumMana);
                Debug.Log("Updated Mana");
                currentMana = value;
            }
        }

        public int MaximumMana => maximumMana;

        void Awake()
        {
            currentMana = maximumMana;
            StartCoroutine(RegenerateMana());
        }

        public void AddMana(float _amount)
        {
            CurrentMana = Mathf.Min(currentMana + _amount, maximumMana);
        }

        public void ReduceMana(float _amount)
        {
            AddMana(-_amount);
        }

        public void SetManaRegeneration(float _value)
        {
            manaRegenerationPerTick = _value;
        }

        public void AddManaRegeneration(float _value)
        {
            manaRegenerationPerTick += _value;
        }

        public void ReduceManaRegeneration(float _value)
        {
            AddManaRegeneration(-_value);
        }

        IEnumerator RegenerateMana()
        {
            while (gameObject.activeSelf)
            {
                CurrentMana = Mathf.Min(maximumMana, currentMana + manaRegenerationPerTick);
                yield return new WaitForSeconds(manaRegenerationTickTimeInSeconds);
            }
        }
    }
}