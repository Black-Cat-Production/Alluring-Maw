using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Scripts.Core.UI
{
    public class PresentTexts : MonoBehaviour
    {
        [SerializeField] List<string> introText;
        [SerializeField] List<string> outroText;
        [SerializeField] TextMeshProUGUI text;
        [SerializeField] Canvas uiCanvas;


        AsyncOperation loadRoutine;

        void FixedUpdate()
        {
            if (Input.GetKeyUp(KeyCode.X))
            {
                StopAllCoroutines();
                if (loadRoutine != null) loadRoutine.allowSceneActivation = true;
                uiCanvas.gameObject.SetActive(false);
            }
        }

        public void PresentIntroText(AsyncOperation _loadRoutine)
        {
            uiCanvas.gameObject.SetActive(true);
            StartCoroutine(ShowText(_loadRoutine, introText));
        }

        public void PresentOutroText(AsyncOperation _loadRoutine)
        {
            uiCanvas.gameObject.SetActive(true);
            StartCoroutine(ShowText(_loadRoutine, outroText));
        }

        IEnumerator ShowText(AsyncOperation _loadRoutine, List<string> _text)
        {
            loadRoutine = _loadRoutine;
            foreach (string msg in _text)
            {
                text.alpha = 0;
                text.text = msg;
                Tween fadeTween = text.DOFade(1, 2.3f);
                yield return fadeTween.WaitForCompletion();
                fadeTween = text.DOFade(0, 2.3f);
                yield return fadeTween.WaitForCompletion();
            }

            uiCanvas.gameObject.SetActive(false);
            yield return null;
            _loadRoutine.allowSceneActivation = true;
        }
    }
}