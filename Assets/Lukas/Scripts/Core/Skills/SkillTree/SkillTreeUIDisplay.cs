using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lukas.Scripts.Core.Skills
{
    public class SkillTreeUIDisplay : MonoBehaviour
    {
        [SerializeField] List<SkillTreeNode> skillTreeNode;

        void Start()
        {
            foreach (var node in skillTreeNode)
            {
                node.OnClick += ColorNodes;
            }
        }

        void OnEnable()
        {
            ColorNodes();
        }

        void ColorNodes()
        {
            foreach (var node in skillTreeNode)
            {
                var imageComponent =  node.GetComponent<Image>();
                imageComponent.color = node.NodeData.Data.Status switch
                {
                    ESkillNodeStatus.Disabled => Color.gray,
                    ESkillNodeStatus.Locked => Color.red,
                    ESkillNodeStatus.Unlocked => Color.green,
                    ESkillNodeStatus.Unlockable => Color.white,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
    }
}