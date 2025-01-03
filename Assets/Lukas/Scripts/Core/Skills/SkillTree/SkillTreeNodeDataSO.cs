using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    [CreateAssetMenu(menuName = "Scriptables/SkillTree/SkillTreeNodeData")]
    public class SkillTreeNodeDataSO : ScriptableObject
    {
        public SkillTreeNodeData Data;
    }
}