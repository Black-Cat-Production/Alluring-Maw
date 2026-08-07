using Scripts.Core.Modules;
using UnityEngine;

namespace Scripts.Core.KI
{
    public class BossAttackState : AttackState
    {
        readonly int chosenAttackIndexIndex;
        static readonly int meleeAttack = Animator.StringToHash("MeleeAttack");
        static readonly int jumpAttack = Animator.StringToHash("JumpAttack");

        public BossAttackState(AttackCollider _attackCollider, Animator _animator, BossAIModule _owner, int _attackIndex, float _normalDamage, float _heavyDamage) : base(_attackCollider, _animator, _owner)
        {
            chosenAttackIndexIndex = _attackIndex;
            bossOwner = _owner;
            normalDamage = _normalDamage;
            heavyDamage = _heavyDamage;
        }

        public override void StateEnter()
        {
            switch (chosenAttackIndexIndex)
            {
                case 0:
                    animator.SetBool(meleeAttack, true);
                    bossOwner.StartAttackSound();
                    break;
                case 1:
                    animator.SetBool(jumpAttack, true);
                    bossOwner.PlayJumpAttackSound();
                    break;
            }
        }

        public override void StateExit()
        {
            switch (chosenAttackIndexIndex)
            {
                case 0:
                    animator.SetBool(meleeAttack, false);
                    break;
                case 1:
                    animator.SetBool(jumpAttack, false);
                    break;
            }
        }

        public override void Tick()
        {
            if (!owner.InAttack) owner.TurnToTarget();
        }

        protected override void DealDamage(HealthSystemModule _target)
        {
            if (animator.GetBool(meleeAttack)) _target.TakeDamage(normalDamage);
            if (animator.GetBool(jumpAttack)) _target.TakeDamage(heavyDamage);
        }
    }
}