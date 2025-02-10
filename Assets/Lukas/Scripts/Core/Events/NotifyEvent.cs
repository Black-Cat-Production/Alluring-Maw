using System;
using UnityEngine;

namespace Scripts.Core.Events
{
    [CreateAssetMenu(menuName = "Scriptables/Events/NotifyEvent")]
    public class NotifyEvent : ScriptableObject
    {
        public Action OnNotify;
        public Action<bool> OnNotifyBool;

        public void Invoke()
        {
            OnNotify?.Invoke();
        }

        public void Invoke(bool _bool)
        {
            OnNotifyBool?.Invoke(_bool);
        }
    }
}