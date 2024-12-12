using System;
using LL_Unity_Utils.Misc;
using LL_Unity_Utils.Timers;
using UnityEngine;
using UnityEngine.AI;

namespace Lukas.Scripts.Core.KI
{
    [RequireComponent(typeof(HealthSystem))]
    public class Enemy : MonoBehaviour
    {
        HealthSystem healthSystem;

        RoomSpawner spawner;

        [SerializeField] float searchRadius;
        [SerializeField] LayerMask detectionMask;
        [SerializeField] float idleDuration;

        TargetComponent targetComponent;
        StateMachine stateMachine;
        State idleState;
        NavMeshAgent agent;

        void Awake()
        {
            targetComponent = new TargetComponent();
            agent = GetComponent<NavMeshAgent>();
            healthSystem = GetComponent<HealthSystem>();
            var idleTimer = new Timer(idleDuration);
            //State Creation
            idleState = new IdleState(idleTimer, agent);
            State chaseState = new WalkToPointState(agent, targetComponent);
            
            //Setup StateMachine
            stateMachine = new StateMachine(idleState,gameObject,false);
            
            //Setup Transitions
            var anyToChase = new Transition(chaseState, FindTarget);
            var chaseToIdle = new Transition(idleState,()=> !FindTarget());
            
            //Link Transitions
            idleState.AddTransition(anyToChase);
            
            chaseState.AddTransition(chaseToIdle);
        }

        void FixedUpdate()
        {
            stateMachine.CheckSwapState();
        }

        public void Die()
        {
            spawner.EnemyDied(this);
            Destroy(gameObject);
        }

        public void SetSpawner(RoomSpawner _spawner)
        {
            spawner = _spawner;
        }

        bool FindTarget()
        {
            var overlap = Physics.OverlapSphere(transform.position, searchRadius, detectionMask);
            if (overlap.Length > 0)
            {
                targetComponent.SetTarget(overlap[0].transform);
                return true;
            }

            return false;
        }
    }
}