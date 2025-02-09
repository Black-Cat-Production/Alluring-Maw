using LL_Unity_Utils.Scriptables;

namespace Scripts.Core.Skills.Effects
{
    public class Effect
    {
        public string Name { get; set; }
        public float Duration { get; set; }
        public float Intensity { get; set; }
        public float IntensityIncrease { get; set; }
        public float TickInterval { get; set; }
        public EffectType Type { get; set; }
        public SkillContext Context { get; set; }
        public VFXSpawner VFXSpawner { get; set; }

        public bool IsRunning { get; set; }

        public Effect()
        {
        }

        public Effect(Effect _effect)
        {
            Name = _effect.Name;
            Duration = _effect.Duration;
            Intensity = _effect.Intensity;
            TickInterval = _effect.TickInterval;
            Type = _effect.Type;
            VFXSpawner = _effect.VFXSpawner;
            IsRunning = false;
        }

        public Effect(EffectData _effectData)
        {
            Name = _effectData.EffectName;
            Duration = _effectData.EffectDuration;
            Intensity = _effectData.EffectIntensity;
            TickInterval = _effectData.EffectTickInterval;
            Type = _effectData.EffectType;
            IntensityIncrease = _effectData.EffectIntensityIncrease;
            VFXSpawner = _effectData.EffectSpawner;
        }
    }
}