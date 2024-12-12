using LL_Unity_Utils.Timers;
using UnityEngine;
using UnityEngine.AI;

namespace Lukas.Scripts.Core.KI
{
    public class IdleState : State
    {
        readonly Timer timer;
        readonly NavMeshAgent agent;
        public bool IsTimerFinished;

        public IdleState(Timer _timer, NavMeshAgent _agent)
        {
            timer = _timer;
            agent = _agent;
        }

        public override void StateEnter()
        {
            timer.StartTimer();
        }

        public override void StateExit()
        {
            IsTimerFinished = false;
        }

        public override void Tick()
        {
            IsTimerFinished = timer.CheckTimer();
        }
    }
}