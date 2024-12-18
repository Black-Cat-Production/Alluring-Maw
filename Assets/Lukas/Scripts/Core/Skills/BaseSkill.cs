namespace Lukas.Scripts.Core.Skills
{
    public class BaseSkill : ISkill
    {
        public string Name { get; private set; }
        public float Damage { get; private set; }

        public BaseSkill(string _name, float _damage)
        {
            Name = _name;
            Damage = _damage;
        }
    }
}