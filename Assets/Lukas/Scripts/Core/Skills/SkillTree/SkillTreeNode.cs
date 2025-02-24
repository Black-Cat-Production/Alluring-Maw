using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.Core.Skills.SkillTree
{
    public class SkillTreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
        public static Action<SkillTreeNode> OnHover;
        public static Action<SkillTreeNode> OnEndHover;

        public bool IsStaticNode;


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

        public void OnPointerEnter(PointerEventData _eventData)
        {
            OnHover?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData _eventData)
        {
            OnEndHover?.Invoke(this);
        }
    }
}