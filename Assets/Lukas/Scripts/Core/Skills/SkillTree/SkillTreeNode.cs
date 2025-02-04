using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Core.Skills.SkillTree
{
    public class SkillTreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] SkillTreeNodeDataSO nodeData;
        [SerializeField] string nodeName;
        [TextArea] [SerializeField] string description;

        public string NodeDescription => description;
        public string NodeName => nodeName;
        public SkillTreeNodeDataSO NodeData => nodeData;

        public Action OnClick;
        public static Action<SkillTreeNode> OnHoverEnter;
        public static Action OnHoverExit;

        public void Clicked()
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

        public void OnPointerEnter(PointerEventData _eventData)
        {
            OnHoverEnter.Invoke(this);
        }

        public void OnPointerExit(PointerEventData _eventData)
        {
            OnHoverExit.Invoke();
        }

        public bool IsUnlockableByCost()
        {
            return nodeData.Data.MemoryFragmentCost <= GameManager.Instance.MemoryFragmentsAmount;
        }
    }
}