using System.Collections.Generic;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public abstract class SkillBridgeUnity : MonoBehaviour
    {
        [SerializeField] string skillName;
        [SerializeField] List<string> tags;
        [SerializeField] protected List<SkillBehaviorSO> behaviors = new();
        public string SkillName => skillName;
        public List<string> Tags => tags;


        public void AddBehavior(SkillBehaviorSO _behavior)
        {
            behaviors.Add(_behavior);
        }

        public void RemoveBehavior(SkillBehaviorSO _behavior)
        {
            behaviors.Remove(_behavior);
        }

        public bool HasBehavior(SkillBehaviorSO _behavior)
        {
            return behaviors.Contains(_behavior);
        }
    }
}