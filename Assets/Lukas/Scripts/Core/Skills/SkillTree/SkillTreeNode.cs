using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public class SkillTreeNode : MonoBehaviour
    {
        [SerializeField] SkillTreeNodeDataSO nodeData;
        public SkillTreeNodeDataSO NodeData => nodeData;

        public Action OnClick;
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
    }
}