using System;
using System.Collections.Generic;
using Scripts.Core.AnimationScripts;
using Scripts.Core.Modules;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Core.Rooms
{
    public class RoomSpawner : MonoBehaviour
    {
        [SerializeField] EnemyListSO enemyList;

        [SerializeField] int spawnAmount;
        [SerializeField] List<Door> doors;
        [SerializeField] float spawnRadius = 5f;

        List<EnemyAIModule> spawnedEnemies;

        //Tracker Events
        public static Action<RoomSpawner> OnRoomEnter;
        public static Action OnEnemyKilled;

        bool alreadyEntered;

        RoomCollider sideEnteredFrom;

        void Start()
        {
            spawnedEnemies = new List<EnemyAIModule>();
            for (int i = 0; i < spawnAmount; i++) spawnedEnemies.Add(SpawnEnemy());
        }

        EnemyAIModule SpawnEnemy()
        {
            var randomPos = Random.insideUnitSphere * spawnRadius;
            var instantiatePos = new Vector3(randomPos.x, 0, randomPos.z);
            instantiatePos += new Vector3(transform.position.x, 0, transform.position.z);
            var spawnedEnemy = Instantiate(enemyList.GetRandomModuleFromList(), instantiatePos, Quaternion.identity);
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
            OpenDoors();
            sideEnteredFrom.DisableTorches();
        }

        public void TriggerRoomEntered()
        {
            if (spawnedEnemies.Count <= 0 || alreadyEntered) return;
            alreadyEntered = true;
            OnRoomEnter.Invoke(this);
            CloseDoors();
            SetAllEnemiesAggro();
        }

        void SetAllEnemiesAggro()
        {
            foreach (var enemy in spawnedEnemies)
            {
                enemy.AllowedAggro = true;
            }
        }

        void OpenDoors()
        {
            foreach (var door in doors)
            {
                door.Open();
            }
        }

        void CloseDoors()
        {
            foreach (var door in doors)
            {
                door.Close();
            }
        }

        public void SetSideEnteredFrom(RoomCollider _roomCollider)
        {
            if (sideEnteredFrom != null) return;
            sideEnteredFrom = _roomCollider;
        }
    }
}