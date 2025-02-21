using LL_Unity_Utils.Timers;
using Scripts.Core.Modules;
using UnityEngine;

namespace Scripts.Core.KI
{
    public class AttackState : AnimationState
    {
        protected readonly EnemyAIModule owner;

        public AttackState(AttackCollider _attackCollider, Animator _animator, EnemyAIModule _owner) : base(_animator)
        {
            _attackCollider.OnHit += DealDamage;
            owner = _owner;
        }

        public override void StateEnter()
        {
            animator.SetBool(HashedAttack, true);
        }

        public override void StateExit()
        {
            animator.SetBool(HashedAttack, false);
        }

        void DealDamage(HealthSystemModule _target)
        {
            _target.TakeDamage(owner.CurrentAttackDamage);
        }
    }
}