using System;
using Lukas.Scripts.Core.Modules;
using Lukas.Scripts.Core.Rooms;
using Lukas.Scripts.Core.SceneHandler;
using Lukas.Scripts.Core.System;
using UnityEditor;
using UnityEngine;

namespace Lukas.Scripts.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] SaveGameSO saveGame;
        [SerializeField] SceneLoader mainMenuSceneLoader;
        public static GameManager Instance { get; private set; }
        public int MemoryFragmentsAmount { get; private set; }
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
                LoadMemoryFragmentsAmount();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void RegisterLastRoom(RoomSpawner _lastRoom)
        {
            _lastRoom.OnFinalRoomCleared += TriggerWin;
        }

        void TriggerWin()
        {
            Debug.Log("You won the game!!");
            saveGame.SaveMemoryFragmentsAmount(MemoryFragmentsAmount);
            mainMenuSceneLoader.LoadAsync();
        }

        public void TriggerLoss()
        {
            Debug.Log("You lost!");
            RetreatToMainMenu();
        }

        public void RetreatToMainMenu()
        {
            MemoryFragmentsAmount = saveGame.MemoryFragmentsAmount;
            mainMenuSceneLoader.LoadAsync();
        }

        public void IncreaseMemoryFragmentsAmount(int _amount)
        {
            MemoryFragmentsAmount += _amount;
            switch (_amount)
            {
                case > 0:
                    Debug.Log($"You gained {_amount} memory fragments!");
                    break;
                case < 0:
                    Debug.Log($"You spent {_amount} memory fragments!");
                    break;
            }
            saveGame.SaveMemoryFragmentsAmount(MemoryFragmentsAmount);
        }

        public void DecreaseMemoryFragmentsAmount(int _amount)
        {
            IncreaseMemoryFragmentsAmount(-_amount);
        }

        public void LoadMemoryFragmentsAmount()
        {
            MemoryFragmentsAmount = saveGame.MemoryFragmentsAmount;
        }
    }
}