using System.Collections.Generic;
using Lukas.Scripts.Core.Skills;
using UnityEngine;

namespace Lukas.Scripts.Core.System
{
    [CreateAssetMenu(menuName = "Scriptables/SaveGame/SaveGameSO")]
    public class SaveGameSO : ScriptableObject
    {
        public int MemoryFragmentsAmount;

        public bool HasSaved;

        public void SaveNodes(List<SkillTreeNodeDataSO> _nodeList)
        {
            if (!HasSaved) HasSaved = true;
        }

        public void SaveMemoryFragmentsAmount(int _amount)
        {
            MemoryFragmentsAmount = 0 + _amount;
        }
    }
}