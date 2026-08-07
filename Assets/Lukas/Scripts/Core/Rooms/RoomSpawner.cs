using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Core.AnimationScripts;
using Scripts.Core.Modules;
using Scripts.Core.Visual;
using UnityEngine;
using Event = AK.Wwise.Event;
using Random = UnityEngine.Random;

namespace Scripts.Core.Rooms
{
    public class RoomSpawner : MonoBehaviour
    {
        [SerializeField] EnemyListSO enemyList;
        [SerializeField] int spawnAmount;
        [SerializeField] List<Door> doors;
        [SerializeField] float spawnRadius = 5f;
        [SerializeField] GoodJobArm goodJobArm;
        [SerializeField] Event fightMusicEvent;
        [SerializeField] Event stopFightMusicEvent;

        [Header("Boss Room Additional Configuration")]
        [SerializeField] EnemyListSO additionalEnemyList;

        [SerializeField] float additionalSpawnAmount;

        List<EnemyAIModule> spawnedEnemies;
        
        bool alreadyEntered;
        RoomCollider sideEnteredFrom;

        //Tracker Events
        public static Action<RoomSpawner> OnRoomEnter;
        public static Action OnEnemyKilled;
        public static Action OnRoomCleared;


        void Start()
        {
            spawnedEnemies = new List<EnemyAIModule>();
            for (int i = 0; i < spawnAmount; i++) spawnedEnemies.Add(SpawnEnemy(enemyList));
            if (additionalEnemyList == null) return;
            for (int i = 0; i < additionalSpawnAmount; i++) spawnedEnemies.Add(SpawnEnemy(additionalEnemyList));
        }

        EnemyAIModule SpawnEnemy(EnemyListSO _enemyListSO)
        {
            var randomPos = Random.insideUnitSphere * spawnRadius;
            var instantiatePos = new Vector3(randomPos.x, 0, randomPos.z);
            instantiatePos += new Vector3(transform.position.x, 0, transform.position.z);
            var spawnedEnemy = Instantiate(_enemyListSO.GetRandomModuleFromList(), instantiatePos, Quaternion.identity);
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
            OnRoomCleared.Invoke();
            AkSoundEngine.PostEvent(stopFightMusicEvent.Name, gameObject);
        }

        public void TriggerRoomEntered()
        {
            if (spawnedEnemies.Count <= 0 || alreadyEntered) return;
            alreadyEntered = true;
            OnRoomEnter.Invoke(this);
            CloseDoors();
            SetAllEnemiesAggro();
            AkSoundEngine.PostEvent(fightMusicEvent.Name, gameObject);
        }

        void SetAllEnemiesAggro()
        {
            foreach (var enemy in spawnedEnemies) enemy.AllowedAggro = true;
        }

        void OpenDoors()
        {
            foreach (var door in doors) door.Open();
        }

        void CloseDoors()
        {
            foreach (var door in doors) door.Close();
        }

        public void SetSideEnteredFrom(RoomCollider _roomCollider)
        {
            if (sideEnteredFrom != null) return;
            sideEnteredFrom = _roomCollider;
        }

        public void FlawlessRoomTrigger()
        {
            var armObject = Instantiate(goodJobArm, transform.position, Quaternion.identity);
            StartCoroutine(DestroyArm(armObject.gameObject));
        }

        IEnumerator DestroyArm(GameObject _armObject)
        {
            yield return new WaitForSeconds(5);
            Destroy(_armObject);
        }
    }
}