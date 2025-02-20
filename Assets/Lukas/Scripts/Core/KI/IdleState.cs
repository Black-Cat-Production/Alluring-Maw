using LL_Unity_Utils.Timers;
using Scripts.Core.AudioScripts;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Core.KI
{
    public class IdleState : AnimationState
    {
        readonly Timer timer;
        public bool IsTimerFinished;
 
        public IdleState(Timer _timer, Animator _animator) : base(_animator)
        {
            timer = _timer;
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