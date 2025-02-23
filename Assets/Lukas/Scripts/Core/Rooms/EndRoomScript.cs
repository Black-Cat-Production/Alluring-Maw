using System.Collections;
using DG.Tweening;
using Scripts.Core.SceneHandler;
using Scripts.Core.UI;
using Scripts.Core.Visual;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Scripts.Core.Rooms
{
    public class EndRoomScript : MonoBehaviour
    {
        [SerializeField] GameObject mainUIObject;
        [SerializeField] WakeUpEffect wakeUpEffect;
        [SerializeField] PresentTexts presentTexts;

        bool isTriggered;

        void OnTriggerEnter(Collider _collider)
        {
            if (!_collider.gameObject.CompareTag("Player") || isTriggered) return;
            var playerInput = _collider.gameObject.GetComponent<PlayerInput>();
            StartCoroutine(PlayEndGameCutscene(playerInput));
        }

        IEnumerator PlayEndGameCutscene(PlayerInput _playerInput)
        {
            mainUIObject.SetActive(false);
            isTriggered = true;
            _playerInput.DeactivateInput();
            wakeUpEffect.Blackout();
            yield return new WaitUntil(() => wakeUpEffect.IsDoneBlackout);
            var loadRoutine = SceneManager.LoadSceneAsync((int)EScenes.MainMenu);
            loadRoutine.allowSceneActivation = false;
            presentTexts.PresentOutroText(loadRoutine);
        }
    }
}