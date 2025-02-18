using System;
using Scripts.Core.Events;
using Scripts.Core.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Core.UI
{
    public class PlayerInputUI : MonoBehaviour

    {
        [SerializeField] TMP_InputField nameInputField;
        [SerializeField] Canvas nameInputUICanvas;
        [SerializeField] NotifyEvent notifyEvent;

        [SerializeField] Canvas alignmentInputUICanvas;

        bool inputUIOpen;
        
        void OnEnable()
        {
            notifyEvent.OnNotify += ShowInputUI;
        }

        void OnDisable()
        {
            notifyEvent.OnNotify -= ShowInputUI;
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                SubmitEntry();
            }
        }

        void ShowInputUI()
        {
            nameInputField.text = string.Empty;
            nameInputUICanvas.gameObject.SetActive(true);
            inputUIOpen = true;
        }

        void HideInputUI()
        {
            nameInputUICanvas.gameObject.SetActive(false);
            inputUIOpen = false;
        }

        public void SubmitEntry()
        {
            if (nameInputField.text == string.Empty || !inputUIOpen) return;
            GameManager.Instance.SetPlayerName(nameInputField.text);
            HideInputUI();
            ShowAlignmentUI();
        }

        void ShowAlignmentUI()
        {
            alignmentInputUICanvas.gameObject.SetActive(true);
        }

        void HideAlignmentUI()
        {
            alignmentInputUICanvas.gameObject.SetActive(false);
        }

        public void ChooseDarkAlignment()
        {
            GameManager.Instance.SetBasicAlignment(ESkillTag.Dark);
            HideAlignmentUI();
        }
        public void ChooseLightAlignment()
        {
            GameManager.Instance.SetBasicAlignment(ESkillTag.Light);
            HideAlignmentUI();
        }
    }
}