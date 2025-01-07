using UnityEngine;

namespace Lukas.Scripts.Core.Events
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