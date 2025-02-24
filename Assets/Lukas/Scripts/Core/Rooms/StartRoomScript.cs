using System.Collections;
using Scripts.Core.AnimationScripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Core.Rooms
{
    public class StartRoomScript : MonoBehaviour
    {
        [SerializeField] float cameraBlendDuration = 1f;
        [SerializeField] GameObject mainUIObject;

        bool isTriggered;

        void OnTriggerEnter(Collider _collider)
        {
            if (!_collider.gameObject.CompareTag("Player") || isTriggered) return;
            var playerAnim = _collider.gameObject.GetComponent<Animator>();
            var playerInput = _collider.gameObject.GetComponent<PlayerInput>();
            StartCoroutine(PlayInspectCutscene(playerAnim, playerInput));
        }

        IEnumerator PlayInspectCutscene(Animator _playerAnimator, PlayerInput _playerInput)
        {
            mainUIObject.SetActive(false);
            isTriggered = true;
            _playerInput.DeactivateInput();
            var startRotation = _playerInput.gameObject.transform.rotation;
            var direction = transform.position + Vector3.up - _playerInput.gameObject.transform.position;
            var targetRotation = Quaternion.LookRotation(direction);
            float elapsedTime = 0f;
            while (elapsedTime < cameraBlendDuration)
            {
                _playerInput.gameObject.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / cameraBlendDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _playerAnimator.SetTrigger("Inspect");
            yield return new WaitForSeconds(5.5f);
            _playerInput.gameObject.transform.rotation = startRotation;
            _playerInput.ActivateInput();
            mainUIObject.SetActive(true);
            GetComponent<Door>().Open();
        }
    }
}