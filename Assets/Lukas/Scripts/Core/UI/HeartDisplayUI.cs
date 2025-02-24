using System;
using System.Collections.Generic;
using Scripts.Core.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Core.UI
{
    public class HeartDisplayUI : MonoBehaviour
    {
        [SerializeField] List<Image> hearts;
        [SerializeField] Sprite fullHeart;
        [SerializeField] Sprite emptyHeart;
        [SerializeField] HealthSystemModule playerHealthSystem;


        public void Start()
        {
            playerHealthSystem.OnDamageTaken += OnDamageTaken;
            UpdateHeartDisplay();
        }

        void OnDamageTaken(int _obj)
        {
            UpdateHeartDisplay();
        }

        void UpdateHeartDisplay()
        {
            int currentHearts = (int)playerHealthSystem.CurrentHealth;
            int maxHearts = (int)playerHealthSystem.MaxHealth;

            for (int i = 0; i < currentHearts; i++) hearts[i].sprite = fullHeart;

            for (int i = currentHearts; i < maxHearts; i++) hearts[i].sprite = emptyHeart;
        }
    }
}