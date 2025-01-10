using System;
using System.Collections;
using Lukas.Scripts.Core.Events;
using Lukas.Scripts.Core.Rooms;
using Lukas.Scripts.Core.SceneHandler;
using Lukas.Scripts.Core.System;
using Lukas.Scripts.Core.UI;
using Lukas.Scripts.Program;
using UnityEditor;
using UnityEngine;

namespace Lukas.Scripts.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] SaveGameSO saveGame;
        [SerializeField] SceneLoader mainMenuSceneLoader;
        [SerializeField] SaveGameManager saveGameManager;
        [SerializeField] PlayerNameInputUI playerNameInputUI;
        [SerializeField] MainMenuScript mainMenuScript;
        [SerializeField] NotifyLeaderboardEvent notifyEvent;
        public static GameManager Instance { get; private set; }
        public int MemoryFragmentsAmount { get; private set; }
        public Action OnWinGetScores;

        public float TimeScore { get; private set; }
        public int DamageTakenScore{ get; private set; }
        
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
                StartCoroutine(Startup());
            }
            else
            {
                Destroy(gameObject);
            }
        }

        IEnumerator Startup()
        {
            int ticks = 0;
            while (!saveGameManager.SavePathsCreated || ticks > 25000)
            {
                yield return null;
                ticks++;
            }
            if(string.IsNullOrEmpty(saveGame.PlayerName)) PromptNameInput();
            LoadGame();
            LoadMemoryFragmentsAmount();
        }

        public void ResetPlayerName()
        {
            saveGame.PlayerName = "";
            PromptNameInput();
        }

        public void SetPlayerName(string _name)
        {
            saveGame.PlayerName = _name;
            mainMenuScript.UpdatePlayerName(GetPlayerName());
        }

        void PromptNameInput()
        {
            playerNameInputUI.ShowInputUI();
        }

        public void RegisterLastRoom(RoomSpawner _lastRoom)
        {
            _lastRoom.OnFinalRoomCleared += TriggerWin;
        }


        void TriggerWin()
        {
            OnWinGetScores.Invoke();
            Debug.Log("You won the game!!");
            saveGame.SaveMemoryFragmentsAmount(MemoryFragmentsAmount);
            notifyEvent.Invoke();
            mainMenuSceneLoader.LoadAsync();
        }

        public void SetLeaderboardScores(float _timeTakenInMil, int _damageTaken)
        {
            TimeScore = _timeTakenInMil;
            DamageTakenScore = _damageTaken;
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
                    Debug.Log($"You spent {-_amount} memory fragments!");
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

        public void SaveGame()
        {
            saveGameManager.Save();
            #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
            #endif
            Application.Quit();
        }

        void LoadGame()
        {
            saveGameManager.Load();
        }

        public string GetPlayerName()
        {
            return saveGame.PlayerName;
        }
    }
}