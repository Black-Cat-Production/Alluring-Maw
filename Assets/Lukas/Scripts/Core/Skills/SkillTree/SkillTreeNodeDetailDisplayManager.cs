using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Core.Skills.SkillTree
{
    public class SkillTreeNodeDetailDisplayManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI nodeNameField;
        [SerializeField] TextMeshProUGUI nodeDescField;
        [SerializeField] TextMeshProUGUI nodeCostValueField;
        [SerializeField] Button unlockButton;

        public void PopulateInformation(SkillTreeNode _node, bool _showButton)
        {
            if(_node.IsStaticNode || _node.NodeData.Data.Status == ESkillNodeStatus.Disabled || _node.NodeData.Data.Status == ESkillNodeStatus.Locked || _node.NodeData.Data.Status == ESkillNodeStatus.Unlocked || !_showButton) unlockButton.gameObject.SetActive(false);
            else unlockButton.gameObject.SetActive(true);
            if (_node == null)
            {
                Debug.LogError("Given node was invalid or got destroyed!");
                return;
            }

            nodeNameField.text = _node.NodeName;
            nodeDescField.text = _node.NodeDescription;
            if (_node.IsStaticNode)
            {
                nodeCostValueField.text = "0";
                nodeCostValueField.color = Color.black;
                return;
            }
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