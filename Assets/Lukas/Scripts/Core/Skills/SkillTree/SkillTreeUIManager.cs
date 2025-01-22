using UnityEngine;

namespace Lukas.Scripts.Core.Skills.SkillTree
{
    public class SkillTreeUIManager : MonoBehaviour
    {
        [SerializeField] Canvas skillTreeUICanvas;
        [SerializeField] CanvasGroup hoveredNodePanelGroup;
        [SerializeField] SkillTreeNodeHoverDisplayManager hoverDisplayManager;

        void OnEnable()
        {
            SkillTreeNode.OnHoverEnter += OpenHoveredNodePanel;
            SkillTreeNode.OnHoverExit += CloseHoveredNodePanel;
        }

        void OnDisable()
        {
            SkillTreeNode.OnHoverEnter -= OpenHoveredNodePanel;
            SkillTreeNode.OnHoverExit -= CloseHoveredNodePanel;
        }

        public void OpenSkillTreeUI()
        {
            skillTreeUICanvas.gameObject.SetActive(true);
        }

        public void CloseSkillTreeUI()
        {
            skillTreeUICanvas.gameObject.SetActive(false);
        }

        void OpenHoveredNodePanel(SkillTreeNode _hoveredNode)
        {
            hoverDisplayManager.PopulateInformation(_hoveredNode);
            hoveredNodePanelGroup.gameObject.SetActive(true);
        }

        public void CloseHoveredNodePanel()
        {
            hoveredNodePanelGroup.gameObject.SetActive(false);
        }
    }
}