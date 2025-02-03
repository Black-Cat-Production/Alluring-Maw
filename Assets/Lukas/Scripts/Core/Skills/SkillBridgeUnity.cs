using System.Collections.Generic;
using Lukas.Scripts.Core.Skills.Behaviors;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public abstract class SkillBridgeUnity : MonoBehaviour
    {
        [SerializeField] string skillName;
        [SerializeField] List<ESkillTag> tags;

        //why does this need to be SF? Otherwise, behavior adding does not work?
        [SerializeField] protected List<SkillBehaviorSO> behaviors = new();

        [SerializeField] float manaCost;
        [SerializeField] bool hasPreviewCast;
        public string SkillName => skillName;
        public float ManaCost => manaCost;
        public List<ESkillTag> Tags => tags;
        public bool HasPreviewCast => hasPreviewCast;

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

        public List<SkillBehaviorSO> GetBehaviors()
        {
            return behaviors;
        }

        public void ResetBehaviorList()
        {
            behaviors.Clear();
        }
    }
}