using UnityEngine;

namespace Scripts.Core.System
{
    [CreateAssetMenu(menuName = "Scriptables/SaveGame/SaveGameSO")]
    public class SaveGameSO : ScriptableObject
    {
        public int MemoryFragmentsAmount;

        public bool HasSaved;

        public string PlayerName;

        public void SaveMemoryFragmentsAmount(int _amount)
        {
            MemoryFragmentsAmount = 0 + _amount;
        }
    }
}