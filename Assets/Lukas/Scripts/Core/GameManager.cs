using System;
using System.Collections;
using Scripts.Core.Events;
using Scripts.Core.SceneHandler;
using Scripts.Core.Skills;
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
        [SerializeField] NotifyEvent notifyPause;
        [SerializeField] SkillBridgeUnity basicAttackSkill;
        public static GameManager Instance { get; private set; }
        public int MemoryFragmentsAmount { get; private set; }
        public Action OnWinGetScores;

        public float TimeScore { get; private set; }
        public int DamageTakenScore { get; private set; }

        public bool FinishedLoading { get; private set; }

        public bool IsPaused { get; private set; }

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
            notifyPause.OnNotifyBool += PauseGame;
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


        public void TriggerWin()
        {
            OnWinGetScores.Invoke();
            saveGame.SaveMemoryFragmentsAmount(MemoryFragmentsAmount);
            notifyLeaderboardToSet.Invoke();
        }

        public void SetLeaderboardScores(float _timeTakenInMil, int _damageTaken)
        {
            TimeScore = _timeTakenInMil;
            DamageTakenScore = _damageTaken;
        }

        public void TriggerLoss(Canvas _deathScreenUI)
        {
            _deathScreenUI.gameObject.SetActive(true);
            saveGame.SaveMemoryFragmentsAmount(MemoryFragmentsAmount);
            PauseGame(true);
        }

        public void RetreatToMainMenu()
        {
            MemoryFragmentsAmount = saveGame.MemoryFragmentsAmount;
            PauseGame(false);
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

        public void SetBasicAlignment(ESkillTag _skillTag)
        {
            switch (_skillTag)
            {
                case ESkillTag.Dark:
                    if (basicAttackSkill.Tags.Contains(ESkillTag.Light)) basicAttackSkill.Tags.Remove(ESkillTag.Light);
                    if (basicAttackSkill.Tags.Contains(ESkillTag.Dark)) return;
                    basicAttackSkill.Tags.Add(ESkillTag.Dark);
                    break;
                case ESkillTag.Light:
                    if (basicAttackSkill.Tags.Contains(ESkillTag.Dark)) basicAttackSkill.Tags.Remove(ESkillTag.Dark);
                    if (basicAttackSkill.Tags.Contains(ESkillTag.Light)) return;
                    basicAttackSkill.Tags.Add(ESkillTag.Light);
                    break;
            }
        }

        void PauseGame(bool _value)
        {
            if (_value)
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                IsPaused = true;
            }
            else
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                IsPaused = false;
            }
        }
    }
}