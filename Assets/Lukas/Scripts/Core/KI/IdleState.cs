using LL_Unity_Utils.Timers;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Core.KI
{
    public class IdleState : AnimationState
    {
        readonly Timer timer;
        readonly NavMeshAgent agent;
        public bool IsTimerFinished;
 
        public IdleState(Timer _timer, NavMeshAgent _agent, Animator _animator) : base(_animator)
        {
            timer = _timer;
            agent = _agent;
        }

        public override void StateEnter()
        {
            timer.StartTimer();
            animator.SetBool(HashedIdle, true);
        }

        public override void StateExit()
        {
            IsTimerFinished = false;
            animator.SetBool(HashedIdle, false);
        }

        public override void Tick()
        {
            IsTimerFinished = timer.CheckTimer();
        }
    }
}