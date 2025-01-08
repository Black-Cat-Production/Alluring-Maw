using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lukas.Scripts.Core.Skills
{
    public class SkillTreeUIManager : MonoBehaviour
    {
        [SerializeField] Canvas skillTreeUICanvas;
        [SerializeField] CanvasGroup hoveredNodePanelGroup;
        [SerializeField] SkillTreeNodeHoverDisplayManager hoverDisplayManager;

        void OnEnable()
        {
            SkillTreeNode.OnHoverStatusChange += ToggleHoveredNodePanel;
        }

        void OnDisable()
        {
            SkillTreeNode.OnHoverStatusChange -= ToggleHoveredNodePanel;
        }

        public void OpenSkillTreeUI()
        {
            skillTreeUICanvas.gameObject.SetActive(true);
        }

        public void CloseSkillTreeUI()
        {
            skillTreeUICanvas.gameObject.SetActive(false);
        }

        void ToggleHoveredNodePanel(SkillTreeNode _hoveredNode)
        {
            if (hoveredNodePanelGroup.isActiveAndEnabled)
            {
                hoveredNodePanelGroup.gameObject.SetActive(false);
            }
            else
            {
                hoverDisplayManager.PopulateInformation(_hoveredNode);
                hoveredNodePanelGroup.gameObject.SetActive(true);
            }
        }
    }
}