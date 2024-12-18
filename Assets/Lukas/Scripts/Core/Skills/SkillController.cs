using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public class SkillController : MonoBehaviour
    {
        public ISkill SelectedSkill { get; private set; }

        public void SetSkill(ISkill _skill)
        {
            SelectedSkill = _skill;
        }
        
        public void CastSkill()
        {
            if (SelectedSkill == null)
            {
                Debug.LogWarning("No skill assigned to SkillController!");
                return;
            }
        }
    }
}