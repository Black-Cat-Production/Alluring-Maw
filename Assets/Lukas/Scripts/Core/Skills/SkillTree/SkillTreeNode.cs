using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public class SkillTreeNode : MonoBehaviour
    {
        [SerializeField] SkillBehaviorSO behavior;
        [SerializeField] List<SkillTreeNode> prerequisites;

        ESkillNodeStatus status;
        
        void Start()
        {
            SkillTreeManager.Instance.RegisterNode(this);
        }

        public void ChangeStatus(ESkillNodeStatus _newStatus)
        {
            status = _newStatus;
        }

        public SkillBehaviorSO GetBehavior()
        {
            return behavior;
        }
    }
}