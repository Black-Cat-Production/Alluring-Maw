using System;
using Lukas.Scripts.Core.Events;
using TMPro;
using UnityEngine;

namespace Lukas.Scripts.Core.UI
{
    public class PlayerNameInputUI : MonoBehaviour

    {
        [SerializeField] TMP_InputField inputField;
        [SerializeField] Canvas inputUICanvas;
        [SerializeField] NotifyEvent notifyEvent;

        void OnEnable()
        {
            notifyEvent.OnNotify += ShowInputUI;
        }

        void OnDisable()
        {
            notifyEvent.OnNotify -= ShowInputUI;
        }


        void ShowInputUI()
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