using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public class SkillTreeManager : MonoBehaviour
    {
        [SerializeField] List<SkillBridgeUnity> playerSkills = new();

        List<SkillTreeNode> nodes = new List<SkillTreeNode>();

        //DEBUG
        // [SerializeField] SkillTreeNode testNodeForDebug;

        public static SkillTreeManager Instance { get; private set; }

        // public Action OnBuildingComplete;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            foreach (var skill in playerSkills)
            {
                skill.ResetBehaviorList();
            }
        }

        public void UnlockBehavior(SkillTreeNode _skillTreeNode)
        {
            var behavior = _skillTreeNode.GetBehavior();
            if (behavior.SpecificName != null)
            {
                foreach (var playerSkill in playerSkills.Where(_playerSkill => _playerSkill.SkillName == behavior.SpecificName))
                {
                    playerSkill.AddBehavior(behavior);
                    _skillTreeNode.ChangeStatus(ESkillNodeStatus.Unlocked);
                }
            }
            else
            {
                //This LINQ selects every playerSkill that matches the tags of the behavior and adds the behavior to it
                foreach (var playerSkill in from playerSkill in playerSkills from tag in behavior.Tags.Where(_tag => playerSkill.Tags.Contains(_tag)) select playerSkill)
                {
                    playerSkill.AddBehavior(behavior);
                    _skillTreeNode.ChangeStatus(ESkillNodeStatus.Unlocked);
                }
            }

            UpdateTree();
        }

        public void RegisterNode(SkillTreeNode _skillTreeNode)
        {
            nodes.Add(_skillTreeNode);
            BuildSkillTree();
        }

        void UpdateTree()
        {
            foreach (var node in nodes.Where(_node => _node.status != ESkillNodeStatus.Unlocked))
            {
                node.ChangeStatus(node.Prerequisites.TrueForAll((_node) => _node.status == ESkillNodeStatus.Unlocked) ? ESkillNodeStatus.Unlockable : ESkillNodeStatus.Locked);
            }
        }

        public void BuildSkillTree()
        {
            foreach (var node in nodes)
            {
                node.ChangeStatus(node.Prerequisites.Count == 0 ? ESkillNodeStatus.Unlockable : ESkillNodeStatus.Locked);
            }
        }
    }
}