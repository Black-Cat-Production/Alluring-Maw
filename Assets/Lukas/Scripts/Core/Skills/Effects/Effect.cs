namespace Lukas.Scripts.Core.Skills.Effects
{
    public class Effect
    {
        public string Name { get; set; }
        public float Duration { get; set; }
        public float Intensity { get; set; }
        public float TickInterval { get; set; }
        public EffectType Type { get; set; }

        public bool IsRunning { get; set; }
    }
}