using System;
using UnityEngine;

namespace Scripts.Core.Events
{
    [CreateAssetMenu(menuName = "Scriptables/Events/NotifyEvent")]
    public class NotifyEvent : ScriptableObject
    {
        public Action OnNotify;

        public void Invoke()
        {
            OnNotify?.Invoke();
        }
    }
}