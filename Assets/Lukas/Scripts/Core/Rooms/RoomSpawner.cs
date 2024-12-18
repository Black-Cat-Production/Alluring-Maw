using System;
using System.Collections.Generic;
using Lukas.Scripts.Core.Modules;
using Lukas.Scripts.Core.Tracker;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lukas.Scripts.Core.Rooms
{
    public class RoomSpawner : MonoBehaviour
    {
        [SerializeField] EnemyAIModule enemyAIModulePrefab;
        [SerializeField] int spawnAmount;
        [SerializeField] GameObject door;
        public string RoomName;
        
        List<EnemyAIModule> spawnedEnemies;
        
        //Tracker Events
        public static Action<RoomSpawner> OnRoomEnter;
        public static Action OnEnemyKilled;

        void Awake()
        {
            spawnedEnemies = new List<EnemyAIModule>();
            for (int i = 0; i < spawnAmount; i++)
            {
                spawnedEnemies.Add(SpawnEnemy());
            }
        }

        EnemyAIModule SpawnEnemy()
        {
            var randomPos = Random.insideUnitSphere * 5;
            var instantiatePos = new Vector3(randomPos.x, 0, randomPos.z);
            instantiatePos += new Vector3(transform.position.x, 1, transform.position.z);
            var spawnedEnemy = Instantiate(enemyAIModulePrefab, instantiatePos, Quaternion.identity);
            spawnedEnemy.SetSpawner(this);
            return spawnedEnemy;
        }

        public void EnemyDied(EnemyAIModule _enemyAIModule)
        {
            spawnedEnemies.Remove(_enemyAIModule);
            OnEnemyKilled.Invoke();
            if (spawnedEnemies.Count == 0) TriggerRoomCleared();
        }

        void TriggerRoomCleared()
        {
            Destroy(door.gameObject);
            Debug.Log("Room Cleared");
        }

        void OnTriggerEnter(Collider _collider)
        {
            if (_collider.gameObject.name == "Player")
            {
                OnRoomEnter.Invoke(this);
            }
        }
    }
}