using System;
using UnityEngine;

namespace Lukas.Scripts.Core
{
    [RequireComponent(typeof(HealthSystem))]
    public class Enemy : MonoBehaviour
    {
        HealthSystem healthSystem;

        RoomSpawner spawner;
        
        void Awake()
        {
            healthSystem = GetComponent<HealthSystem>();
        }

        public void Die()
        {
            spawner.EnemyDied(this);
            Destroy(gameObject);
        }

        public void SetSpawner(RoomSpawner _spawner)
        {
            spawner = _spawner;
        }
    }
}