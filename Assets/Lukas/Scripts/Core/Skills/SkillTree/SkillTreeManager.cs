using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Core.Skills.SkillTree
{
    public class SkillTreeManager : MonoBehaviour
    {
        [SerializeField] List<SkillBridgeUnity> playerSkills = new();
        [SerializeField] SkillTreeNodeRegistry nodeRegistry;

        public static SkillTreeManager Instance { get; private set; }

        IEnumerator Start()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
                while (!GameManager.Instance.FinishedLoading) yield return null;

                foreach (var skill in playerSkills) skill.ResetBehaviorList();

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

            if (GameManager.Instance.MemoryFragmentsAmount < _skillTreeNodeData.Data.MemoryFragmentCost && _useCost)
            {
                Debug.Log("You do not have enough memory fragments!");
                return;
            }

            if (_useCost) GameManager.Instance.DecreaseMemoryFragmentsAmount(_skillTreeNodeData.Data.MemoryFragmentCost);
            var behavior = _skillTreeNodeData.Data.Behavior;
            if (behavior.SpecificName != null)
                foreach (var playerSkill in playerSkills.Where(_playerSkill => _playerSkill.SkillName == behavior.SpecificName))
                {
                    playerSkill.AddBehavior(behavior);
                    _skillTreeNodeData.Data.ChangeStatus(ESkillNodeStatus.Unlocked);
                }
            else
                //This LINQ selects every playerSkill that matches the tags of the behavior and adds the behavior to it
                foreach (var playerSkill in from playerSkill in playerSkills from tag in behavior.Tags.Where(_tag => playerSkill.Tags.Contains(_tag)) select playerSkill)
                {
                    playerSkill.AddBehavior(behavior);
                    _skillTreeNodeData.Data.ChangeStatus(ESkillNodeStatus.Unlocked);
                }
            
            if(_skillTreeNodeData.Data.HasOnUnlockExecution) _skillTreeNodeData.Data.Behavior.OnUnlockExecute();

            UpdateTree();
        }

        void UpdateTree()
        {
            //First Foreach checks every node that is not unlocked, if the prerequisites are met to be able to be unlocked
            //Second Foreach checks every node that is not unlocked, if any of the nodes configured as "exclusive" are unlocked, and locks it accordingly
            foreach (var node in nodeRegistry.SkillTreeNodesData.Where(_node => _node.Data.Status != ESkillNodeStatus.Unlocked)) node.Data.ChangeStatus(node.Data.Prerequisites.TrueForAll((_node) => _node.Data.Status == ESkillNodeStatus.Unlocked) ? ESkillNodeStatus.Unlockable : ESkillNodeStatus.Locked);
            foreach (var node in nodeRegistry.SkillTreeNodesData.Where(_node => _node.Data.Status != ESkillNodeStatus.Unlocked)) node.Data.ChangeStatus(node.Data.Exclusives.Any((_node => _node.Data.Status == ESkillNodeStatus.Unlocked )) ? ESkillNodeStatus.Locked : ESkillNodeStatus.Unlockable);
        }

        void BuildSkillTree()
        {
            bool loadedNodes = false;
            foreach (var node in nodeRegistry.SkillTreeNodesData.Where(_node => _node.Data.Status == ESkillNodeStatus.Unlocked))
            {
                Debug.Log("Found unlocked node!");
                UnlockBehavior(node, false);
                loadedNodes = true;
            }

            if (loadedNodes) return;
            Debug.Log("No saved tree found!");
            foreach (var node in nodeRegistry.SkillTreeNodesData) node.Data.ChangeStatus(node.Data.Prerequisites.Count == 0 ? ESkillNodeStatus.Unlockable : ESkillNodeStatus.Locked);
        }

        public void ResetSkillTree()
        {
            foreach (var node in nodeRegistry.SkillTreeNodesData) node.Data.ChangeStatus(ESkillNodeStatus.Disabled);

            foreach (var skill in playerSkills) skill.ResetBehaviorList();

            BuildSkillTree();
        }
    }
}