using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public class SkillSelector : MonoBehaviour
    {
        [SerializeField] List<Skill> availableSkills;

        public Skill CurrentSelectedSkill { get; private set; }

        void Awake()
        {
            CurrentSelectedSkill = availableSkills[0];
        }

        public void UpdateSelectedSkill(int _changeDirection)
        {
            int currentIndex = availableSkills.FindIndex((_a) => _a == CurrentSelectedSkill);
            currentIndex = currentIndex + _changeDirection;
            if (currentIndex < 0) currentIndex = availableSkills.Count - 1;
            else if (currentIndex > availableSkills.Count - 1) currentIndex = 0;
            CurrentSelectedSkill = availableSkills[currentIndex];
        }
    }
}