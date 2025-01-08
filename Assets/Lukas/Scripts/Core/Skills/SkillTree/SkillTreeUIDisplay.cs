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
                switch (node.NodeData.Data.Status)
                {
                    case ESkillNodeStatus.Disabled:
                        imageComponent.sprite = lockedSkill;
                        break;
                    case ESkillNodeStatus.Locked:
                        imageComponent.sprite = lockedSkill;
                        break;
                    case ESkillNodeStatus.Unlocked:
                        imageComponent.sprite = unlockableSkill;
                        break;
                    case ESkillNodeStatus.Unlockable:
                        imageComponent.sprite = unlockableSkill;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (node.NodeData.Data.Status == ESkillNodeStatus.Unlocked) imageComponent.color = Color.green;
            }
        }

        void UpdateCurrentFragmentAmount()
        {
            currentFragmentAmountField.text = GameManager.Instance.MemoryFragmentsAmount.ToString();
        }
    }
}