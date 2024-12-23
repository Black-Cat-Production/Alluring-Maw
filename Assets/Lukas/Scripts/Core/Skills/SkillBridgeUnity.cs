using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public abstract class SkillBridgeUnity : MonoBehaviour
    {
        [SerializeField] string skillName;
        [SerializeField] List<ESkillTag> tags;
        [SerializeField] protected List<SkillBehaviorSO> behaviors = new();
        [SerializeField] float manaCost;
        public string SkillName => skillName;
        public float ManaCost => manaCost;
        public List<ESkillTag> Tags => tags;
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

        public void ResetBehaviorList()
        {
            behaviors.Clear();
        }
    }
}