using UnityEngine;

namespace Scripts.Core.Skills.Effects
{
    [CreateAssetMenu(menuName = "Scriptables/EffectData")]
    public class EffectData : ScriptableObject
    {
        public string EffectName;
        public float EffectDuration;
        public float EffectIntensity;

        public float EffectTickInterval;
        public EffectType EffectType;

        [Header("Only for DamageOverTimeScaling type")]
        [Tooltip("This only works with the 'DamageOverTimeScaling' EffectType")]
        public float EffectIntensityIncrease;
    }
}