using Scripts.Core.System;
using UnityEngine;

namespace Scripts.Core.UI
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