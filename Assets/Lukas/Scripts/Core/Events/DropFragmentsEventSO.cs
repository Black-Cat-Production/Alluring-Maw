using UnityEngine;

namespace Scripts.Core.Events
{
    [CreateAssetMenu(menuName = "Scriptables/Events/DropFragmentsEvent")]
    public class DropFragmentsEventSO : ScriptableObject
    {
        public void Invoke(int _amount)
        {
            GameManager.Instance.IncreaseMemoryFragmentsAmount(_amount);
        }
    }
}