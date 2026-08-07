using Scripts.Core.Modules;
using UnityEngine;
using Event = AK.Wwise.Event;

namespace Scripts.Core.KI
{
    public class AttackState : AnimationState
    {
        protected readonly EnemyAIModule owner;

        protected BossAIModule bossOwner;
        protected float normalDamage;
        protected float heavyDamage;

        public AttackState(AttackCollider _attackCollider, Animator _animator, EnemyAIModule _owner) : base(_animator)
        {
            _attackCollider.OnHit += DealDamage;
            owner = _owner;
        }

        public override void StateEnter()
        {
            animator.SetBool(HashedAttack, true);
            owner.StartAttackSound();
        }

        public override void StateExit()
        {
            animator.SetBool(HashedAttack, false);
        }

        protected virtual void DealDamage(HealthSystemModule _target)
        {
            if (bossOwner == null) _target.TakeDamage(owner.CurrentAttackDamage);
        }
    }
}