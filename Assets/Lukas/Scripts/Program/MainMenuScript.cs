using System.Collections;
using Scripts.Core;
using Scripts.Core.Events;
using Scripts.Core.Skills.SkillTree;
using Scripts.Core.System;
using TMPro;
using UnityEngine;

namespace Scripts.Program
{
    public class MainMenuScript : MonoBehaviour
    {
        [SerializeField] SaveGameSO saveGameSO;
        [SerializeField] SaveGameSO defaultSaveGameSO;
        [SerializeField] TextMeshProUGUI nameValue;
        [SerializeField] NotifyEvent notifyEvent;

        IEnumerator Start()
        {
            Screen.SetResolution(1920, 1080, true);
            while (!GameManager.Instance.FinishedLoading) yield return null;
            UpdatePlayerName();
        }

        void OnEnable()
        {
            notifyEvent.OnNotify += UpdatePlayerName;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        void OnDisable()
        {
            notifyEvent.OnNotify -= UpdatePlayerName;
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