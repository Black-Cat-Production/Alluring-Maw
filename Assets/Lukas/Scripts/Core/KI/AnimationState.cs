using UnityEngine;

namespace Lukas.Scripts.Core.KI
{
    public class AnimationState : State

    {
        protected Animator animator;

        protected readonly int HashedAttack = Animator.StringToHash("IsAttacking");
        protected readonly int HashedIdle = Animator.StringToHash("IsIdle");
        protected readonly int HashedWalk = Animator.StringToHash("IsWalking");
        
        protected AnimationState(Animator _animator)
        {
            animator = _animator;
        }
    }
}