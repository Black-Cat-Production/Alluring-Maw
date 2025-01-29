using LL_Unity_Utils.Timers;
using Lukas.Scripts.Core.Modules;
using UnityEngine;

namespace Lukas.Scripts.Core.KI
{
    public class AttackState : AnimationState
    {
        GameObject player;
        Timer attackCooldown;

        public AttackState(GameObject _player, Timer _attackCooldown, Animator _animator) : base(_animator)
        {
            player = _player;
            attackCooldown = _attackCooldown;
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
                player.GetComponent<HealthSystemModule>().TakeDamage(1);
                attackCooldown.StartTimer();
            }
        }
    }
}