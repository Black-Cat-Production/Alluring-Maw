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
        [SerializeField] EnemyAIModule enemyAIModulePrefab;

        [SerializeField] int spawnAmount;

        //TODO: Replace with a list of doors!!
        [SerializeField] List<Door> doors;
        [SerializeField] bool isLastRoom;
        public string RoomName;

        List<EnemyAIModule> spawnedEnemies;

        //Tracker Events
        public static Action<RoomSpawner> OnRoomEnter;
        public static Action OnEnemyKilled;
        public Action OnFinalRoomCleared;

        void Start()
        {
            spawnedEnemies = new List<EnemyAIModule>();
            for (int i = 0; i < spawnAmount; i++) spawnedEnemies.Add(SpawnEnemy());
            if (isLastRoom) GameManager.Instance.RegisterLastRoom(this);
        }

        EnemyAIModule SpawnEnemy()
        {
            var randomPos = Random.insideUnitSphere * 5;
            var instantiatePos = new Vector3(randomPos.x, 0, randomPos.z);
            instantiatePos += new Vector3(transform.position.x, 0, transform.position.z);
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
            OpenDoors();
            Debug.Log("Room Cleared");
            if (isLastRoom) OnFinalRoomCleared?.Invoke();
        }

        public void TriggerRoomEntered()
        {
            if (spawnedEnemies.Count <= 0) return;
            OnRoomEnter.Invoke(this);
            CloseDoors();
            
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
    }
}