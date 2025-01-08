using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lukas.Scripts.Core.Skills
{
    public class SkillTreeUIDisplay : MonoBehaviour
    {
        [SerializeField] List<SkillTreeNode> skillTreeNode;
        [SerializeField] TextMeshProUGUI currentFragmentAmountField;
        [SerializeField] Sprite lockedSkill;
        [SerializeField] Sprite unlockableSkill;

        void Start()
        {
            foreach (var node in skillTreeNode)
            {
                node.OnClick += UpdateNodeDisplay;
                node.OnClick += UpdateCurrentFragmentAmount;
            }
        }

        void OnEnable()
        {
            UpdateNodeDisplay();
            UpdateCurrentFragmentAmount();
        }

        void UpdateNodeDisplay()
        {
            foreach (var node in skillTreeNode)
            {
                var imageComponent =  node.GetComponent<Image>();
                imageComponent.sprite = node.NodeData.Data.Status switch
                {
                    ESkillNodeStatus.Disabled => lockedSkill,
                    ESkillNodeStatus.Locked => lockedSkill,
                    ESkillNodeStatus.Unlocked => unlockableSkill,
                    ESkillNodeStatus.Unlockable => unlockableSkill,
                    _ => throw new ArgumentOutOfRangeException()
                };

                imageComponent.color = node.NodeData.Data.Status == ESkillNodeStatus.Unlocked ? Color.green : Color.white;
            }
        }

        void UpdateCurrentFragmentAmount()
        {
            currentFragmentAmountField.text = GameManager.Instance.MemoryFragmentsAmount.ToString();
        }
    }
}