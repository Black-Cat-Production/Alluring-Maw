using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Core.Skills.SkillTree
{
    [CreateAssetMenu(menuName = "Scriptables/SkillTree/SkillTreeNodeRegistry")]
    public class SkillTreeNodeRegistry : ScriptableObject
    {
        public List<SkillTreeNodeDataSO> SkillTreeNodesData;
    }
}