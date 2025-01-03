using System;
using System.Collections.Generic;
using System.Linq;
using Lukas.Scripts.Core.System;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public class SkillTreeManager : MonoBehaviour
    {
        [SerializeField] List<SkillBridgeUnity> playerSkills = new();
        [SerializeField] SaveGameSO saveGame;
        [SerializeField] SkillTreeNodeRegistry nodeRegistry;

        public static SkillTreeManager Instance { get; private set; }

        void Start()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
                foreach (var skill in playerSkills)
                {
                    skill.ResetBehaviorList();
                }
                BuildSkillTree();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void UnlockBehavior(SkillTreeNodeDataSO _skillTreeNodeData, bool _useCost)
        {
            if (GameManager.Instance == null)
            {
                Debug.LogError("No Game Manager found!");
                return;
            }

            if (_skillTreeNodeData == null || _skillTreeNodeData.Data == null)
            {
                Debug.LogError("No SkillNodeData Found!");
                return;
            }

            if (GameManager.Instance.MemoryFragmentsAmount < _skillTreeNodeData.Data.MemoryFragmentCost)
            {
                Debug.Log("You do not have enough memory fragments!");
                return;
            }

            if (_useCost) GameManager.Instance.DecreaseMemoryFragmentsAmount(_skillTreeNodeData.Data.MemoryFragmentCost);
            var behavior = _skillTreeNodeData.Data.Behavior;
            if (behavior.SpecificName != null)
            {
                foreach (var playerSkill in playerSkills.Where(_playerSkill => _playerSkill.SkillName == behavior.SpecificName))
                {
                    playerSkill.AddBehavior(behavior);
                    _skillTreeNodeData.Data.ChangeStatus(ESkillNodeStatus.Unlocked);
                }
            }
            else
            {
                //This LINQ selects every playerSkill that matches the tags of the behavior and adds the behavior to it
                foreach (var playerSkill in from playerSkill in playerSkills from tag in behavior.Tags.Where(_tag => playerSkill.Tags.Contains(_tag)) select playerSkill)
                {
                    playerSkill.AddBehavior(behavior);
                    _skillTreeNodeData.Data.ChangeStatus(ESkillNodeStatus.Unlocked);
                }
            }

            UpdateTree();
        }

        void UpdateTree()
        {
            foreach (var node in nodeRegistry.SkillTreeNodesData.Where(_node => _node.Data.Status != ESkillNodeStatus.Unlocked))
            {
                node.Data.ChangeStatus(node.Data.Prerequisites.TrueForAll((_node) => _node.Data.Status == ESkillNodeStatus.Unlocked) ? ESkillNodeStatus.Unlockable : ESkillNodeStatus.Locked);
            }
        }

        public void BuildSkillTree()
        {
            if (saveGame.HasSaved)
            {
                //LoadSkillTree(saveGame.SavedNodes);
                foreach (var node in nodeRegistry.SkillTreeNodesData.Where(_node => _node.Data.Status == ESkillNodeStatus.Unlocked))
                {
                    UnlockBehavior(node, false);
                }

                Debug.Log("Found Saved Skill Tree!");
            }
            else
            {
                Debug.Log("No saved tree found!");
                foreach (var node in nodeRegistry.SkillTreeNodesData)
                {
                    node.Data.ChangeStatus(node.Data.Prerequisites.Count == 0 ? ESkillNodeStatus.Unlockable : ESkillNodeStatus.Locked);
                }
            }
        }

        public void SaveSkillTree()
        {
            var savedNodeData = nodeRegistry.SkillTreeNodesData;
            saveGame.SaveNodes(savedNodeData);
        }

        public void ResetSkillTree()
        {
            foreach (var node in nodeRegistry.SkillTreeNodesData)
            {
                node.Data.ChangeStatus(ESkillNodeStatus.Disabled);
            }
            foreach (var skill in playerSkills)
            {
                skill.ResetBehaviorList();
            }

            BuildSkillTree();
        }

        void OnDisable()
        {
            SaveSkillTree();
        }
    }
}