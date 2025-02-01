using LL_Unity_Utils.Timers;
using Lukas.Scripts.Core.Modules;
using UnityEngine;

namespace Lukas.Scripts.Core.KI
{
    public class AttackState : AnimationState
    {
        readonly GameObject player;
        readonly Timer attackCooldown;
        readonly EnemyAIModule owner;

        public AttackState(GameObject _player, Timer _attackCooldown, Animator _animator, EnemyAIModule _owner) : base(_animator)
        {
            player = _player;
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
                player.GetComponent<HealthSystemModule>().TakeDamage(owner.CurrentAttackDamage);
                attackCooldown.StartTimer();
            }
        }
    }
}