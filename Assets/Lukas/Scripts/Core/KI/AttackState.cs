using LL_Unity_Utils.Timers;
using Scripts.Core.Modules;
using UnityEngine;

namespace Scripts.Core.KI
{
    public class AttackState : AnimationState
    {
        readonly Timer attackCooldown;
        readonly EnemyAIModule owner;

        public AttackState(AttackCollider _attackCollider, Timer _attackCooldown, Animator _animator, EnemyAIModule _owner) : base(_animator)
        {
            _attackCollider.OnHit += DealDamage;
            attackCooldown = _attackCooldown;
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

        public override void Tick()
        {
            if (attackCooldown.CheckTimer())
            {
                attackCooldown.StartTimer();
            }
        }

        void DealDamage(HealthSystemModule _target)
        {
            _target.TakeDamage(owner.CurrentAttackDamage);
        }
    }
}