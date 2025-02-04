using System.Collections.Generic;
using Scripts.Core.Skills.SkillTree;
using UnityEngine;

namespace Scripts.Core.Skills
{
    public class SkillSelector : MonoBehaviour
    {
        [SerializeField] List<SkillBridgeUnity> availableSkills = new();
        SkillController skillController;
        SkillTreeManager skillTreeManager;

        void Awake()
        {
            skillController = GetComponent<SkillController>();
            skillController.SetSkill(availableSkills[0]);
        }

        public void UpdateSelectedSkill(int _changeDirection)
        {
            int currentIndex = availableSkills.FindIndex((_a) => _a == skillController.SelectedSkill);
            currentIndex += _changeDirection;
            if (currentIndex < 0) currentIndex = availableSkills.Count - 1;
            else if (currentIndex > availableSkills.Count - 1) currentIndex = 0;
            skillController.SetSkill(availableSkills[currentIndex]);
            Debug.Log(availableSkills[currentIndex].SkillName);
        }


        public bool CanCastSpell()
        {
            if (!(skillController.SelectedSkill.ManaCost > skillController.ManaSystemModule.CurrentMana)) return true;
            Debug.Log("You dont have enough mana to cast!");
            return false;
        }

        public List<ESkillTag> GetSelectedSkillTags()
        {
            return skillController.SelectedSkill.Tags;
        }
    }
}