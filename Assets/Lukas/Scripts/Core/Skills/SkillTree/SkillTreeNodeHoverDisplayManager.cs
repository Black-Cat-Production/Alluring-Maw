using TMPro;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills.SkillTree
{
    public class SkillTreeNodeHoverDisplayManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI nodeNameField;
        [SerializeField] TextMeshProUGUI nodeDescField;
        [SerializeField] TextMeshProUGUI nodeCostValueField;

        public void PopulateInformation(SkillTreeNode _node)
        {
            if (_node == null)
            {
                Debug.LogError("Given node was invalid or got destroyed!");
                return;
            }
            nodeNameField.text = _node.NodeName;
            nodeDescField.text = _node.NodeDescription;
            if (_node.NodeData.Data.Status == ESkillNodeStatus.Unlocked) nodeDescField.text = _node.NodeDescription + "  --UNLOCKED--  ";
            nodeCostValueField.text = _node.NodeData.Data.MemoryFragmentCost.ToString();
            if (_node.NodeData.Data.Status == ESkillNodeStatus.Unlocked)
            {
                nodeCostValueField.color = Color.green;
                return;
            }
            nodeCostValueField.color = !_node.IsUnlockableByCost() ? Color.red : Color.black;
        }
    }
}