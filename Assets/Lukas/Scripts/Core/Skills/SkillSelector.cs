using System;
using System.Collections.Generic;
using LL_Unity_Utils.Timers;
using Scripts.Core.Skills.SkillTree;
using Scripts.Core.UI;
using UnityEngine;

namespace Scripts.Core.Skills
{
    public class SkillSelector : MonoBehaviour
    {
        [SerializeField] List<SkillBridgeUnity> availableSkills = new();
        [SerializeField] SkillSelectionUI skillSelectionUI;
        [SerializeField] UIStatUpdater statUI;
        SkillController skillController;
        SkillTreeManager skillTreeManager;

        public Action OnSkillGotCast;
        
        void Awake()
        {
            foreach (var skill in availableSkills)
            {
                skill.CooldownTimer = new Timer(skill.SkillCooldown);
                Debug.Log($"Skill {skill.SkillName} was given a timer with a duration of {skill.SkillCooldown}");
            }
            skillController = GetComponent<SkillController>();
            skillController.SetSkill(availableSkills[0]);
            skillSelectionUI.UpdateSkillUI(0);
        }

        void OnEnable()
        {
            skillController.SkillWasCast += TriggerCooldownUIUpdate;
        }

        void OnDisable()
        {
            skillController.SkillWasCast -= TriggerCooldownUIUpdate;
        }

        public void UpdateSelectedSkill(int _changeDirection)
        {
            int currentIndex = availableSkills.FindIndex((_a) => _a == skillController.SelectedSkill);
            currentIndex += _changeDirection;
            if (currentIndex < 0) currentIndex = availableSkills.Count - 1;
            else if (currentIndex > availableSkills.Count - 1) currentIndex = 0;
            skillController.SetSkill(availableSkills[currentIndex]);
            skillSelectionUI.UpdateSkillUI(currentIndex);
            Debug.Log(availableSkills[currentIndex].SkillName);
        }


        public bool CanCastSpell()
        {
            if (!(skillController.SelectedSkill.ManaCost > skillController.ManaSystemModule.CurrentMana) && !skillController.SelectedSkill.GetIsOnCooldown()) return true;
            if (!(skillController.SelectedSkill.ManaCost > skillController.ManaSystemModule.CurrentMana)) return false;
            statUI.ShowNotEnoughMana();
            return false;
        }

        public List<ESkillTag> GetSelectedSkillTags()
        {
            return skillController.SelectedSkill.Tags;
        }

        void TriggerCooldownUIUpdate()
        {
            skillSelectionUI.UpdateCooldownUI(availableSkills.FindIndex((_a) => _a == skillController.SelectedSkill), skillController.SelectedSkill.SkillCooldown);
            OnSkillGotCast?.Invoke();
        }
    }
}