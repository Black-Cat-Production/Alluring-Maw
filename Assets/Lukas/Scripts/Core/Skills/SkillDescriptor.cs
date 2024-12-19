using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    [CreateAssetMenu(menuName = "Scriptables/Skill/SkillDescriptor")]
    public class SkillDescriptor : ScriptableObject
    {
        public string SkillName;
        public GameObject skillPrefab;
    }
}