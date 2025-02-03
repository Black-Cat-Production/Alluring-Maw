using System;
using System.Collections.Generic;
using Lukas.Scripts.Core.Skills.Behaviors;
using Newtonsoft.Json;

namespace Lukas.Scripts.Core.Skills.SkillTree
{
    [Serializable]
    public class SkillTreeNodeData
    {
        [JsonIgnore] public SkillBehaviorSO Behavior;
        [JsonIgnore] public List<SkillTreeNodeDataSO> Prerequisites;
        [JsonIgnore] public List<SkillTreeNodeDataSO> Exclusives;
        [JsonIgnore] public int MemoryFragmentCost;

        public ESkillNodeStatus Status;

        public void ChangeStatus(ESkillNodeStatus _newStatus)
        {
            Status = _newStatus;
        }

        public void SetNodeData(SkillTreeNodeData _data)
        {
            Behavior = _data.Behavior;
            Prerequisites = _data.Prerequisites;
            Status = _data.Status;
        }
    }
}