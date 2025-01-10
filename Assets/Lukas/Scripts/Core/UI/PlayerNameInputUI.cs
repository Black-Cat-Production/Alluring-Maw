using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lukas.Scripts.Core.UI
{
    public class PlayerNameInputUI : MonoBehaviour

    {
        [SerializeField] TMP_InputField inputField;
        [SerializeField] Canvas inputUICanvas;
        

        public void ShowInputUI()
        {
            inputField.text = string.Empty;
            inputUICanvas.gameObject.SetActive(true);
        }

        void HideInputUI()
        {
            inputUICanvas.gameObject.SetActive(false);
        }
        
        public void SubmitEntry()
        {
            if (inputField.text == string.Empty) return;
            GameManager.Instance.SetPlayerName(inputField.text);
            HideInputUI();
        }
    }
}