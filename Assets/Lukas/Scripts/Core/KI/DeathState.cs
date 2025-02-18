using UnityEngine;

namespace Scripts.Core.KI
{
    public class DeathState : AnimationState
    {
        public DeathState(Animator _animator) : base(_animator)
        {
            animator = _animator;
        }

        public override void StateEnter()
        {
            animator.SetBool(HashedDeath, true);
        }
    }
}