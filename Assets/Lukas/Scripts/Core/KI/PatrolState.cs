using System;
using LL_Unity_Utils.Misc;
using UnityEngine;
using UnityEngine.AI;

namespace Lukas.Scripts.Core.KI
{
    public class PatrolState : WalkToPointState
    {
        readonly Action calculatePathAction;

        public PatrolState(NavMeshAgent _agent, TargetComponent _target, Action _calculatePathAction, Animator _animator) : base(_agent, _target, _animator)
        {
            calculatePathAction = _calculatePathAction;
        }

        public override void StateEnter()
        {
            calculatePathAction();
            base.StateEnter();
        }
    }
}