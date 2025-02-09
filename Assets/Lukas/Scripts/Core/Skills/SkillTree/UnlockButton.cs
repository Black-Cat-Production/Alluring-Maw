using UnityEngine;

namespace Scripts.Core.Skills.SkillTree
{
    public class UnlockButton : MonoBehaviour
    {
        [SerializeField] SkillTreeUIManager skillTreeUIManager;
        public void UnlockSelectedNode()
        {
            if (skillTreeUIManager.CurrentSelectedNode == null)
            {
                Debug.LogError("No Skill Tree Node selected! Something went wrong!");
            }
            skillTreeUIManager.CurrentSelectedNode.Unlock();
            skillTreeUIManager.UpdateDetailDisplay();
        }
    }
}