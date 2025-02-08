using System;
using Scripts.Core.Skills;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Scripts.Core.UI
{
    public class SkillSelectionUI : MonoBehaviour
    {
        [SerializeField] Image image;


        public void UpdateSkillUI(SkillController _skillController)
        {
            image.sprite = _skillController.SelectedSkill.SkillSprite;
        }
    }
}