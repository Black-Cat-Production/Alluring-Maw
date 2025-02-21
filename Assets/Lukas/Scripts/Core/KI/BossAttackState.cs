using Scripts.Core.Modules;
using UnityEngine;

namespace Scripts.Core.KI
{
    public class BossAttackState : AttackState
    {
        int chosenAttackIndexIndex;
        static readonly int meleeAttack = Animator.StringToHash("MeleeAttack");
        static readonly int jumpAttack = Animator.StringToHash("JumpAttack");

        public BossAttackState(AttackCollider _attackCollider, Animator _animator, EnemyAIModule _owner, int _attackIndex) : base(_attackCollider, _animator, _owner)
        {
            chosenAttackIndexIndex = _attackIndex;
        }

        public override void StateEnter()
        {
            switch (chosenAttackIndexIndex)
            {
                case 0:
                    animator.SetBool(meleeAttack, true);
                    break;
                case 1:
                    animator.SetBool(jumpAttack, true);
                    break;
            }
        }

        public override void StateExit()
        {
            switch (chosenAttackIndexIndex)
            {
                case 0:
                    animator.SetBool(meleeAttack, false);
                    break;
                case 1:
                    animator.SetBool(jumpAttack, false);
                    break;
            }
        }

        public override void Tick()
        {
            if(!owner.InAttack) owner.TurnToTarget();
        }
    }
}