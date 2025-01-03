using LL_Unity_Utils.Misc;
using UnityEngine;
using UnityEngine.AI;

namespace Lukas.Scripts.Core.KI
{
    //Add structure for AnimatedState -> Inherit from AnimatedState
    public class WalkToPointState : State
    {
        readonly NavMeshAgent agent;
        readonly TargetComponent target;

        public WalkToPointState(NavMeshAgent _agent, TargetComponent _target) : base()
        {
            agent = _agent;
            target = _target;
        }

        public override void StateEnter()
        {
            agent.transform.LookAt(target.TargetPosition);
            agent.SetDestination(target.TargetPosition);
        }

        public override void StateExit()
        {
            agent.SetDestination(agent.transform.position + (agent.transform.forward * 0.15f));
        }

        public override void Tick()
        {
            if (!agent.isActiveAndEnabled) return;
            if (!(Vector3.Distance(agent.destination, target.TargetPosition) >= 0.5f)) return;
            agent.SetDestination(target.TargetPosition);
            agent.transform.LookAt(target.TargetPosition);
        }
    }
}