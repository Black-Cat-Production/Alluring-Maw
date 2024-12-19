namespace Lukas.Scripts.Core.Skills
{
    public sealed class SU_DefaultAttack : GenericSkill<DefaultAttack>
    {
        protected override void Awake()
        {
            Skill = new DefaultAttack(name, baseSkillHitDamage);
            skillLogic = Skill;
            base.Awake();
        }
    }
}