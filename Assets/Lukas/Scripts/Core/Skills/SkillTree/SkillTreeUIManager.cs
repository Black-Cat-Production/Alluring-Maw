using UnityEngine;
using UnityEngine.InputSystem;

namespace Lukas.Scripts.Core.Skills
{
    public class SkillTreeUIManager : MonoBehaviour
    {
        [SerializeField] Canvas skillTreeUICanvas;
        [SerializeField] PlayerInput playerInput;
        bool isOpen;


        void Start()
        {
          //  SkillTreeManager.Instance.OnBuildingComplete += () => skillTreeUICanvas.gameObject.SetActive(false);
          //  if (skillTreeUICanvas.gameObject.activeSelf) return;
          //  skillTreeUICanvas.gameObject.SetActive(true);
          //  SkillTreeManager.Instance.BuildSkillTree();
        }

        public void ToggleSkillTreeUI(InputAction.CallbackContext _callbackContext)
        {
            if (_callbackContext.phase != InputActionPhase.Started) return;
            if (isOpen)
            {
                skillTreeUICanvas.gameObject.SetActive(false);
                isOpen = false;
                playerInput.SwitchCurrentActionMap("Player");
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                skillTreeUICanvas.gameObject.SetActive(true);
                isOpen = true;
                playerInput.SwitchCurrentActionMap("SkillTree");
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
        }
    }
}