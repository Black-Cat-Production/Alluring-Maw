using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lukas.Scripts.Core.Skills
{
    public class SkillTreeManager : MonoBehaviour
    {
        [SerializeField] List<SkillBridgeUnity> playerSkills = new();

        List<SkillTreeNode> nodes = new List<SkillTreeNode>();

        //DEBUG
        [SerializeField] SkillTreeNode testNodeForDebug;

        public static SkillTreeManager Instance { get; private set; }

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

        void UnlockBehavior(SkillTreeNode _skillTreeNode)
        {
            var behavior = _skillTreeNode.GetBehavior();
            if (behavior.SpecificName != null)
            {
                foreach (var playerSkill in playerSkills.Where(_playerSkill => _playerSkill.SkillName == behavior.SpecificName))
                {
                    playerSkill.AddBehavior(behavior);
                }

                return;
            }

            //This LINQ selects every playerSkill that matches the tags of the behavior and adds the behavior to it
            foreach (var playerSkill in from playerSkill in playerSkills from tag in behavior.Tags.Where(_tag => playerSkill.Tags.Contains(_tag)) select playerSkill)
            {
                playerSkill.AddBehavior(behavior);
            }
        }


        public void DebugUnlock(InputAction.CallbackContext _callbackContext)
        {
            if (!_callbackContext.started) return;
            UnlockBehavior(testNodeForDebug);
        }

        public void RegisterNode(SkillTreeNode _skillTreeNode)
        {
            nodes.Add(_skillTreeNode);
        }
    }
}