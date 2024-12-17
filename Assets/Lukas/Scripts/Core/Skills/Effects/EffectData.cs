using UnityEngine;

namespace Lukas.Scripts.Core.Skills.Effects
{
    [CreateAssetMenu(menuName = "Scriptables/EffectData")]
    public class EffectData : ScriptableObject
    {
        public string EffectName;
        public float EffectDuration;
        public float EffectIntensity;
        public float EffectTickInterval;
        public EffectType EffectType;
    }
}