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

        public List<SkillTreeNode> Prerequisites => prerequisites;
        public ESkillNodeStatus status { get; private set; }

        bool isRegistered;

        void Start()
        {
            SkillTreeManager.Instance.RegisterNode(this);
        }

        public void ChangeStatus(ESkillNodeStatus _newStatus)
        {
            status = _newStatus;
        }

        public void Clicked()
        {
            switch (status)
            {
                case ESkillNodeStatus.Unlockable:
                    SkillTreeManager.Instance.UnlockBehavior(this);
                    break;
                case ESkillNodeStatus.Unlocked:
                    Debug.Log("You already have this skill!");
                    break;
                default:
                    Debug.Log("You cannot unlock this skill!");
                    break;
            }
        }

        public SkillBehaviorSO GetBehavior()
        {
            return behavior;
        }
    }
}