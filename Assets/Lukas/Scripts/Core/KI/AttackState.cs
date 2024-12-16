using LL_Unity_Utils.Timers;
using UnityEngine;

namespace Lukas.Scripts.Core.KI
{
    public class AttackState : State
    {
        GameObject player;
        Timer attackCooldown;
        public AttackState(GameObject _player, Timer _attackCooldown)
        {
            player = _player;
            attackCooldown = _attackCooldown;
        }

        public override void StateEnter()
        {
            base.StateEnter();
        }

        public override void StateExit()
        {
            base.StateExit();
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