using System;
using Lukas.Scripts.Core.Modules;
using Lukas.Scripts.Core.Rooms;
using Lukas.Scripts.Core.UI;
using UnityEngine;

namespace Lukas.Scripts.Core.Tracker
{
    public class PlayerTracker : MonoBehaviour
    {
        [SerializeField] GameObject playerCharacter;
        [SerializeField] UIStatUpdater uiStatUpdater;
        public Vector3 PlayerPosition { get; private set; }

        public int EnemiesKilled { get; private set; }
        ManaSystemModule playerManaSystemModule;
        
        //This should prob. be any form of ID or similar
        RoomSpawner currentRoom;

        void Awake()
        {
            playerManaSystemModule = playerCharacter.GetComponent<ManaSystemModule>();
            uiStatUpdater.UpdateManaUI(playerManaSystemModule.MaximumMana, playerManaSystemModule.MaximumMana);
        }

        void OnEnable()
        {
            RoomSpawner.OnRoomEnter += SetCurrentRoom;
            RoomSpawner.OnEnemyKilled += CountUpEnemiesKilled;
            playerCharacter.GetComponent<ManaSystemModule>().OnManaChanged += uiStatUpdater.UpdateManaUI;
        }

        void OnDisable()
        {
            RoomSpawner.OnRoomEnter -= SetCurrentRoom;
            RoomSpawner.OnEnemyKilled -= CountUpEnemiesKilled;
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
    }
}