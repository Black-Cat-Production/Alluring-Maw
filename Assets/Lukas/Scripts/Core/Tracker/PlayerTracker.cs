using System.Diagnostics;
using Scripts.Core.Modules;
using Scripts.Core.Rooms;
using Scripts.Core.UI;
using UnityEngine;

namespace Scripts.Core.Tracker
{
    public class PlayerTracker : MonoBehaviour
    {
        [SerializeField] GameObject playerCharacter;
        [SerializeField] UIStatUpdater uiStatUpdater;
        public Vector3 PlayerPosition { get; private set; }

        float timeTaken;
        int damageTaken;
        Stopwatch stopwatch;

        public int EnemiesKilled { get; private set; }
        ManaSystemModule playerManaSystemModule;
        HealthSystemModule playerHealthSystemModule;

        //This should prob. be any form of ID or similar
        RoomSpawner currentRoom;


        void Awake()
        {
            damageTaken = 0;
            timeTaken = 0;
            playerManaSystemModule = playerCharacter.GetComponent<ManaSystemModule>();
            playerHealthSystemModule = playerCharacter.GetComponent<HealthSystemModule>();
            uiStatUpdater.UpdateManaUI(playerManaSystemModule.MaximumMana, playerManaSystemModule.MaximumMana);
        }

        void Update()
        {
            timeTaken = stopwatch.ElapsedMilliseconds;
        }

        void OnEnable()
        {
            GameManager.Instance.OnWinGetScores += SendScores;
            RoomSpawner.OnRoomEnter += SetCurrentRoom;
            RoomSpawner.OnEnemyKilled += CountUpEnemiesKilled;
            playerHealthSystemModule.OnDamageTaken += _damageTaken => damageTaken += _damageTaken;
            playerCharacter.GetComponent<ManaSystemModule>().OnManaChanged += uiStatUpdater.UpdateManaUI;
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        void OnDisable()
        {
            GameManager.Instance.OnWinGetScores -= SendScores;
            RoomSpawner.OnRoomEnter -= SetCurrentRoom;
            RoomSpawner.OnEnemyKilled -= CountUpEnemiesKilled;
            playerHealthSystemModule.OnDamageTaken -= _damageTaken => damageTaken += _damageTaken;
            if (playerCharacter == null || !playerCharacter.gameObject.activeInHierarchy) return;
            playerCharacter.GetComponent<ManaSystemModule>().OnManaChanged -= uiStatUpdater.UpdateManaUI;
        }

        void FixedUpdate()
        {
            PlayerPosition = playerCharacter.transform.position;
        }

        void SetCurrentRoom(RoomSpawner _newRoom)
        {
            currentRoom = _newRoom;
            uiStatUpdater.UpdateCurrentRoomName(currentRoom.RoomName);
        }

        void CountUpEnemiesKilled()
        {
            EnemiesKilled++;
            uiStatUpdater.UpdateKillsStat(EnemiesKilled);
        }

        void SendScores()
        {
            GameManager.Instance.SetLeaderboardScores(timeTaken, damageTaken);
        }
    }
}