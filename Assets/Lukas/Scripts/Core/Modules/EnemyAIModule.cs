using LL_Unity_Utils.Misc;
using LL_Unity_Utils.Timers;
using Lukas.Scripts.Core.KI;
using Lukas.Scripts.Core.Rooms;
using Lukas.Scripts.Core.Skills.Effects;
using UnityEngine;
using UnityEngine.AI;

namespace Lukas.Scripts.Core.Modules
{
    [RequireComponent(typeof(HealthSystemModule))]
    public class EnemyAIModule : MonoBehaviour
    {
        HealthSystemModule healthSystemModule;

        RoomSpawner spawner;

        [SerializeField] float searchRadius;
        [SerializeField] LayerMask detectionMask;
        [SerializeField] LayerMask obstructionMask;
        [SerializeField] float idleDuration;
        [SerializeField] float PatrolRange;
        [SerializeField] float PatrolPointDistanceThreshhold;
        [SerializeField] float attackCooldown;
        [SerializeField] float attackRange;

        TargetComponent targetComponent;
        TargetComponent idleTargetComponent;
        StateMachine stateMachine;
        IdleState idleState;
        NavMeshAgent agent;
        Vector3 patrolRadiusCenter;
        
        float distanceToTarget => Vector3.Distance(transform.position, targetComponent.TargetPosition);
        
        
        GameObject player;

        void Awake()
        {
            patrolRadiusCenter = transform.position;
            targetComponent = new TargetComponent();
            idleTargetComponent = new TargetComponent();
            agent = GetComponent<NavMeshAgent>();
            healthSystemModule = GetComponent<HealthSystemModule>();
            healthSystemModule.RegisterEffectHandler(EffectType.DamageOverTime, new DamageOverTimeHandler());
            var idleTimer = new Timer(idleDuration);
            //State Creation
            idleState = new IdleState(idleTimer, agent);
            State chaseState = new WalkToPointState(agent, targetComponent);
            State patrolState = new PatrolState(agent, idleTargetComponent, RecalculatePatrolPoint);
            //DEBUG TESTING AREA. NEEDS TO GO
            player = GameObject.Find("Player");
            State attackState = new AttackState(player, new Timer(attackCooldown));
            
            //Setup StateMachine
            stateMachine = new StateMachine(idleState,gameObject,false);
            
            //Setup Transitions
            var anyToChase = new Transition(chaseState, FindTarget);
            var chaseToIdle = new Transition(idleState,()=> !FindTarget());
            var chaseToAttack = new Transition(attackState, () => distanceToTarget < attackRange);
            var attackToChase = new Transition(chaseState, () => distanceToTarget > attackRange);
            var idleToPatrol = new Transition(patrolState, () => idleState.IsTimerFinished == true);
            var movingToIdle = new Transition(idleState, () => agent.remainingDistance < agent.stoppingDistance);
            
            //Link Transitions
            idleState.AddTransition(anyToChase);
            idleState.AddTransition(idleToPatrol);
            
            chaseState.AddTransition(chaseToAttack);
            chaseState.AddTransition(chaseToIdle);

            patrolState.AddTransition(anyToChase);
            patrolState.AddTransition(movingToIdle);
            
            attackState.AddTransition(attackToChase);
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
            if (overlap.Length > 0 && !DetectObstruction(overlap[0].transform))
            {
                targetComponent.SetTarget(overlap[0].transform);
                return true;
            }

            return false;
        }

        bool DetectObstruction(Transform _target)
        {
            return Physics.Raycast(transform.position, (_target.position - transform.position).normalized, 10f, obstructionMask);
        }
        
        void RecalculatePatrolPoint()
        {
            Vector3 randomPoint;
            do
            {
                var unitSphere = Random.insideUnitSphere * PatrolRange;
                randomPoint = new Vector3(unitSphere.x, 0, unitSphere.z);
                randomPoint += patrolRadiusCenter;
            } while (!NavMesh.SamplePosition(randomPoint, out _, agent.radius * 2, agent.areaMask) || Vector3.Distance(transform.position, randomPoint) < PatrolPointDistanceThreshhold);

            idleTargetComponent.SetPoint(randomPoint);
        }
        
        void OnValidate()
        {
            PatrolPointDistanceThreshhold = Mathf.Clamp(PatrolPointDistanceThreshhold, 1, PatrolRange - 1);
        }
    }
}