using System;
using System.Collections;
using UnityEngine;

namespace Scripts.Core.UI
{
    public class TutorialUI : MonoBehaviour
    {
        [SerializeField] Canvas tutorialUICanvas;
        [SerializeField] float timeToShowTutorial = 10f;

        public void StartTutorial()
        {
            StartCoroutine(ShowTutorial());
        }

        IEnumerator ShowTutorial()
        {
            OpenTutorialUI();
            yield return new WaitForSeconds(timeToShowTutorial); 
            CloseTutorialUI();
        }

        void OpenTutorialUI()
        {
            tutorialUICanvas.gameObject.SetActive(true);
        }

        void CloseTutorialUI()
        {
            tutorialUICanvas.gameObject.SetActive(false);
        }
    }
}