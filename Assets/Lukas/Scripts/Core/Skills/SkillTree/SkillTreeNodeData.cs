using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Scripts.Core.Skills.Behaviors;

namespace Scripts.Core.Skills.SkillTree
{
    [Serializable]
    public class SkillTreeNodeData
    {
        [JsonIgnore] public SkillBehaviorSO Behavior;
        [JsonIgnore] public List<SkillTreeNodeDataSO> Prerequisites;
        [JsonIgnore] public List<SkillTreeNodeDataSO> Exclusives;
        [JsonIgnore] public int MemoryFragmentCost;
        [JsonIgnore] public bool HasOnUnlockExecution;

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