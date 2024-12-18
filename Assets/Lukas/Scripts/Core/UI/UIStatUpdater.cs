using System;
using TMPro;
using UnityEngine;

namespace Lukas.Scripts.Core.UI
{
    public class UIStatUpdater : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI totalKillStat;
        [SerializeField] TextMeshProUGUI currentRoomName;

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
    }
}