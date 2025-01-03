using UnityEngine;
using UnityEngine.InputSystem;

namespace Lukas.Scripts.Core.Skills
{
    public class SkillTreeUIManager : MonoBehaviour
    {
        [SerializeField] Canvas skillTreeUICanvas;
        bool isOpen;

        public void ToggleSkillTreeUI()
        {
            if (isOpen)
            {
                skillTreeUICanvas.gameObject.SetActive(false);
                isOpen = false;
            }
            else
            {
                skillTreeUICanvas.gameObject.SetActive(true);
                isOpen = true;
            }
        }
    }
}