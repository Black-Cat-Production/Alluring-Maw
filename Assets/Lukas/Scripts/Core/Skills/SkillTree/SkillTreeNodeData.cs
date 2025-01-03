using System;
using System.Collections.Generic;

namespace Lukas.Scripts.Core.Skills
{
    [Serializable]
    public class SkillTreeNodeData
    {
        public SkillBehaviorSO Behavior;
        public List<SkillTreeNodeDataSO> Prerequisites;
        public ESkillNodeStatus Status;
        public int MemoryFragmentCost;
        
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