using System;
using UnityEngine;

namespace Lukas.Scripts.Core.Events
{
    [CreateAssetMenu(menuName = "Scriptables/Events/NotifyLeaderboardEvent")] 
    public class NotifyLeaderboardEvent : ScriptableObject
    {
        public Action OnNotify;

        public void Invoke()
        {
            OnNotify?.Invoke();
        }
    }
}