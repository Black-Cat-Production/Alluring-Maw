using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    [CreateAssetMenu(menuName = "Scriptables/Skill/SkillRegistry")]
    public class SkillRegistry : ScriptableObject
    {
        public List<SkillDescriptor> Skills;

        public SkillDescriptor GetSkillByName(string _name)
        {
            return Skills.FirstOrDefault(_skill => _skill.SkillName == _name);
        }
    }
}