using System;
using System.Collections;
using Scripts.Core.Events;
using Scripts.Core.Rooms;
using Scripts.Core.SceneHandler;
using Scripts.Core.System;
using Scripts.Program;
using UnityEditor;
using UnityEngine;

namespace Scripts.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] SaveGameSO saveGame;
        [SerializeField] SceneLoader mainMenuSceneLoader;
        [SerializeField] SaveGameManager saveGameManager;
        [SerializeField] NotifyEvent notifyPlayerInputUI;
        [SerializeField] NotifyEvent notifyMainMenu;
        [SerializeField] NotifyEvent notifyLeaderboardToSet;
        [SerializeField] NotifyEvent notifyLeaderboardOnNameChange;
        public static GameManager Instance { get; private set; }
        public int MemoryFragmentsAmount { get; private set; }
        public Action OnWinGetScores;

        public float TimeScore { get; private set; }
        public int DamageTakenScore { get; private set; }

        public bool FinishedLoading { get; private set; }

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
            while (!saveGameManager.SavePathsCreated && ticks < 200)
            {
                yield return null;
                ticks++;
            }

            LoadGame();
            if (ticks >= 200) Debug.LogError("Startup failed due to max ticks reached!");
            if (string.IsNullOrEmpty(saveGame.PlayerName)) PromptNameInput();
            LoadMemoryFragmentsAmount();
            yield return null;
            FinishedLoading = true;
        }

        public void ResetPlayerName()
        {
            saveGame.PlayerName = "";
            PromptNameInput();
        }

        public void SetPlayerName(string _name)
        {
            saveGame.PlayerName = _name;
            notifyLeaderboardOnNameChange.Invoke();
            notifyMainMenu.Invoke();
        }

        void PromptNameInput()
        {
            notifyPlayerInputUI.Invoke();
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
            notifyLeaderboardToSet.Invoke();
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