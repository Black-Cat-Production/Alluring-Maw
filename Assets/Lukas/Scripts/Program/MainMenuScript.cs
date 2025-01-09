using System;
using Lukas.Scripts.Core;
using Lukas.Scripts.Core.Skills;
using Lukas.Scripts.Core.System;
using UnityEngine;

namespace Lukas.Scripts.Program
{
    public class MainMenuScript : MonoBehaviour
    {
        [SerializeField] SaveGameSO saveGameSO;
        [SerializeField] SaveGameSO defaultSaveGameSO;

        private void Start()
        {
            Screen.SetResolution(1920, 1080, true);
        }

        void OnEnable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void ResetSaveGame()
        {
            saveGameSO.HasSaved = defaultSaveGameSO.HasSaved;
            saveGameSO.MemoryFragmentsAmount = defaultSaveGameSO.MemoryFragmentsAmount;
            GameManager.Instance.LoadMemoryFragmentsAmount();
            SkillTreeManager.Instance.ResetSkillTree();
        }

        public void SaveGame()
        {
            GameManager.Instance.SaveGame();
        }
        
    }
}