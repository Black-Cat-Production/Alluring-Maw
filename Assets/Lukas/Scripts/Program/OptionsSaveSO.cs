using UnityEngine;

namespace Scripts.Program
{
    [CreateAssetMenu(menuName = "Scriptables/SaveGame/OptionsSaveSO")]
    public class OptionsSaveSO : ScriptableObject
    {
        public float MouseSense;
        public float MasterVolume;
        public float SFXVolume;
        public float MusicVolume;
        public float VoiceVolume;
    }
}