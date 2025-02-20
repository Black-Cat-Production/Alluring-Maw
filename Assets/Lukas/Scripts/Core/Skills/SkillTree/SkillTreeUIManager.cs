using System;
using UnityEngine;

namespace Scripts.Core.Skills.SkillTree
{
    public class SkillTreeUIManager : MonoBehaviour
    {
        [SerializeField] Canvas skillTreeUICanvas;
        [SerializeField] CanvasGroup nodeDetailPanelGroup;
        [SerializeField] SkillTreeNodeDetailDisplayManager detailDisplayManager;

        AudioSource audioSource;
        
        public SkillTreeNode CurrentSelectedNode { get; private set; }

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        void OnEnable()
        {
            SkillTreeNode.OnHover += NodeHovered;
            SkillTreeNode.OnEndHover += NodeEndHover;
        }

        void OnDisable()
        {
            SkillTreeNode.OnHover -= NodeHovered;
            SkillTreeNode.OnEndHover -= NodeEndHover;
        }

        public void OpenSkillTreeUI()
        {
            skillTreeUICanvas.gameObject.SetActive(true);
        }

        public void CloseSkillTreeUI()
        {
            skillTreeUICanvas.gameObject.SetActive(false);
        }

        public void UpdateDetailDisplay(bool _showUnlockButton, SkillTreeNode _selectedNode)
        {
            detailDisplayManager.PopulateInformation(_selectedNode, _showUnlockButton);
        }

        void OpenNodeDetailPanel(SkillTreeNode _selectedNode, bool _showUnlockButton)
        {
            if (nodeDetailPanelGroup.gameObject.activeInHierarchy)
            {
                UpdateDetailDisplay(_showUnlockButton, _selectedNode);
            }
            else
            {
                nodeDetailPanelGroup.gameObject.SetActive(true);
                UpdateDetailDisplay(_showUnlockButton, _selectedNode);
            }
        }

        public void ButtonOpenNodeDetailPanel(SkillTreeNode _selectedNode)
        {
            CurrentSelectedNode = _selectedNode;
            OpenNodeDetailPanel(_selectedNode, true);
        }

        void NodeHovered(SkillTreeNode _skillTreeNode)
        {
            OpenNodeDetailPanel(_skillTreeNode, false);
        }

        void NodeEndHover(SkillTreeNode _skillTreeNode)
        {
            if(CurrentSelectedNode != null) UpdateDetailDisplay(true, CurrentSelectedNode);
            else CloseNodeDetailPanel();
        }

        public void CloseNodeDetailPanel()
        {
            nodeDetailPanelGroup.gameObject.SetActive(false);
            CurrentSelectedNode = null;
        }

        public void PlayUnlockSound()
        {
            audioSource.Stop();
            audioSource.Play();
        }
    }
}