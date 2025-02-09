using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Core.Skills.SkillTree
{
    public class SkillTreeUIDisplay : MonoBehaviour
    {
        [SerializeField] List<SkillTreeNode> skillTreeNode;
        [SerializeField] TextMeshProUGUI currentFragmentAmountField;
        [SerializeField] Sprite lockedFrame;
        [SerializeField] Sprite lockedFrameHover;
        [SerializeField] Sprite unlockedFrame;
        [SerializeField] Sprite unlockedFrameHover;
        [SerializeField] Sprite lockedConnection;
        [SerializeField] Sprite unlockedConnection;

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
                var imageComponent = node.GetComponent<Image>();
                var buttonComponent = node.GetComponent<Button>();

                if (node.NodeData.Data.Status == ESkillNodeStatus.Unlocked)
                {
                    imageComponent.sprite = unlockedFrame;
                    buttonComponent.spriteState = new SpriteState()
                    {
                        highlightedSprite = unlockedFrameHover,
                        selectedSprite = unlockedFrameHover
                    };
                    node.ConnectionToNode.sprite = unlockedConnection;
                }
                else
                {
                    imageComponent.sprite = lockedFrame;
                    buttonComponent.spriteState = new SpriteState()
                    {
                        highlightedSprite = lockedFrameHover,
                        selectedSprite = lockedFrameHover
                    };
                    node.ConnectionToNode.sprite = lockedConnection;
                }
            }
        }

        void UpdateCurrentFragmentAmount()
        {
            currentFragmentAmountField.text = GameManager.Instance.MemoryFragmentsAmount.ToString();
        }
    }
}