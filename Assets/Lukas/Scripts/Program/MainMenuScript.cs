using System.Collections;
using Scripts.Core;
using Scripts.Core.Events;
using Scripts.Core.Skills.SkillTree;
using Scripts.Core.System;
using Scripts.Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Program
{
    public class MainMenuScript : MonoBehaviour
    {
        [SerializeField] SaveGameSO saveGameSO;
        [SerializeField] SaveGameSO defaultSaveGameSO;
        [SerializeField] TextMeshProUGUI nameValue;
        [SerializeField] NotifyEvent notifyEvent;
        [SerializeField] OptionsMenuUI optionsMenuUI;

        [SerializeField] Button saveGameButton;
        
        IEnumerator Start()
        {
            Screen.SetResolution(1920, 1080, true);
            while (!GameManager.Instance.FinishedLoading) yield return null;
            UpdatePlayerName();
            optionsMenuUI.PushUpdateOnLoad();
        }

        void OnEnable()
        {
            notifyEvent.OnNotify += UpdatePlayerName;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            saveGameButton.onClick.AddListener(GameManager.Instance.SaveGame);;
        }

        void OnDisable()
        {
            notifyEvent.OnNotify -= UpdatePlayerName;
            saveGameButton.onClick.RemoveListener(GameManager.Instance.SaveGame);
        }

        public void ResetSaveGame()
        {
            saveGameSO.HasSaved = defaultSaveGameSO.HasSaved;
            saveGameSO.MemoryFragmentsAmount = defaultSaveGameSO.MemoryFragmentsAmount;
            GameManager.Instance.ResetPlayerName();
            GameManager.Instance.LoadMemoryFragmentsAmount();
            SkillTreeManager.Instance.ResetSkillTree();
        }

        public void SaveGame()
        {
            GameManager.Instance.SaveGame();
        }

        void UpdatePlayerName()
        {
            nameValue.text = GameManager.Instance.GetPlayerName();
        }
    }
}