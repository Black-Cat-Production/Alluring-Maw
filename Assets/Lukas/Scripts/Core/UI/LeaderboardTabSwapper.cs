using Lukas.Scripts.Core.System;
using TMPro;
using UnityEngine;

namespace Lukas.Scripts.Core.UI
{
    public class LeaderboardTabSwapper : MonoBehaviour
    {
        [SerializeField] LeaderboardUI leaderboardUI;
        [SerializeField] ELeaderboardType tabType;

        public void SwapTab()
        {
            leaderboardUI.DisplayLeaderboard(tabType);
        }
    }
}