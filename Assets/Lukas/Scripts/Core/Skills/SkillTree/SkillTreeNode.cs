using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Core.Skills.SkillTree
{
    public class SkillTreeNode : MonoBehaviour
    {
        [SerializeField] SkillTreeNodeDataSO nodeData;
        [SerializeField] string nodeName;
        [TextArea] [SerializeField] string description;
        [SerializeField] Image connectionToNode;
        
        
        public string NodeDescription => description;
        public string NodeName => nodeName;
        public SkillTreeNodeDataSO NodeData => nodeData;
        public Image ConnectionToNode => connectionToNode;

        public Action OnClick;

        public bool IsStaticNode;

        public bool ISLOCKEDFORTESTING;


        public void Unlock()
        {
            switch (nodeData.Data.Status)
            {
                case ESkillNodeStatus.Unlockable:
                    SkillTreeManager.Instance.UnlockBehavior(nodeData, true);
                    break;
                case ESkillNodeStatus.Unlocked:
                    Debug.Log("You already have this skill!");
                    break;
                default:
                    Debug.Log("You cannot unlock this skill!");
                    break;
            }

            OnClick.Invoke();
        }

        public bool IsUnlockableByCost()
        {
            return nodeData.Data.MemoryFragmentCost <= GameManager.Instance.MemoryFragmentsAmount;
        }
    }
}