using System.Collections.Generic;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    [CreateAssetMenu(menuName = "Scriptables/SkillTree/SkillTreeNodeRegistry")]
    public class SkillTreeNodeRegistry : ScriptableObject
    {
        public List<SkillTreeNodeDataSO> SkillTreeNodesData;
    }
}