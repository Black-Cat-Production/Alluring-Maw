using UnityEngine;

namespace Scripts.Core.Skills.SkillTree
{
    public class SkillTreeUIManager : MonoBehaviour
    {
        [SerializeField] Canvas skillTreeUICanvas;
        [SerializeField] CanvasGroup nodeDetailPanelGroup;
        [SerializeField] SkillTreeNodeDetailDisplayManager detailDisplayManager;

        public SkillTreeNode CurrentSelectedNode { get; private set; }
        
        public void OpenSkillTreeUI()
        {
            skillTreeUICanvas.gameObject.SetActive(true);
        }

        public void CloseSkillTreeUI()
        {
            skillTreeUICanvas.gameObject.SetActive(false);
        }

        public void UpdateDetailDisplay()
        {
            detailDisplayManager.PopulateInformation(CurrentSelectedNode);
        }

        public void OpenNodeDetailPanel(SkillTreeNode _selectedNode)
        {
            CurrentSelectedNode = _selectedNode;
            if (nodeDetailPanelGroup.gameObject.activeInHierarchy)
            {
                UpdateDetailDisplay();
            }
            else
            {
                nodeDetailPanelGroup.gameObject.SetActive(true);
                UpdateDetailDisplay();
            }
        }

        public void CloseNodeDetailPanel()
        {
            nodeDetailPanelGroup.gameObject.SetActive(false);
            CurrentSelectedNode = null;
        }
    }
}