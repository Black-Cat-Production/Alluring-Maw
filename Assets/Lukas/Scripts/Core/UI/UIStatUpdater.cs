using TMPro;
using UnityEngine;

namespace Scripts.Core.UI
{
    public class UIStatUpdater : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI totalKillStat;
        [SerializeField] TextMeshProUGUI currentRoomName;
        [SerializeField] TextMeshProUGUI currentMana;
        [SerializeField] TextMeshProUGUI maximumMana;
        [SerializeField] Material manaOrbMaterial;
        static readonly int liquidAmount = Shader.PropertyToID("_LiquidAmount");

        void Awake()
        {
            totalKillStat.text = 0.ToString();
        }

        public void UpdateKillsStat(int _newTotal)
        {
            totalKillStat.text = _newTotal.ToString();
        }

        public void UpdateCurrentRoomName(string _name)
        {
            currentRoomName.text = _name;
        }

        public void UpdateManaUI(float _currentMana, int _maximumMana)
        {
            //currentMana.text = _currentMana.ToString("0");
            //maximumMana.text = _maximumMana.ToString();
            float percentageMana = _currentMana / _maximumMana;
            manaOrbMaterial.SetFloat(liquidAmount, percentageMana);
        }
    }
}