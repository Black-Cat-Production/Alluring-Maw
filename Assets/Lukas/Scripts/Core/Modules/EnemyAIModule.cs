using System;
using System.Collections.Generic;
using LL_Unity_Utils.Misc;
using LL_Unity_Utils.Scriptables;
using Scripts.Core.KI;
using Scripts.Core.Rooms;
using Scripts.Core.Skills;
using Scripts.Core.Skills.Effects;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using State = Scripts.Core.KI.State;
using StateMachine = Scripts.Core.KI.StateMachine;
using Timer = LL_Unity_Utils.Timers.Timer;

namespace Scripts.Core.Modules
{
    [RequireComponent(typeof(HealthSystemModule))]
    public class EnemyAIModule : MonoBehaviour
    {
        public HealthSystemModule HealthSystemModule { get; private set; }

        RoomSpawner spawner;

        [SerializeField] float searchRadius;
        [SerializeField] LayerMask detectionMask;
        [SerializeField] LayerMask obstructionMask;
        [SerializeField] float idleDuration;
        [SerializeField] float PatrolRange;
        [SerializeField] float PatrolPointDistanceThreshhold;
        [SerializeField] float attackCooldown;
        [SerializeField] float attackRange;
        [SerializeField] float baseAttackDamage;
        [SerializeField] float baseMoveSpeed;

        [Header("Drop Values")]
        [SerializeField] int memoryFragmentDropMin;
        [SerializeField] int memoryFragmentDropMax;

        [Header("Visuals")]
        [SerializeField] VFXSpawner lightHit;
        [SerializeField] VFXSpawner darkHit;

        AttackCollider attackCollider;
        TargetComponent targetComponent;
        TargetComponent idleTargetComponent;
        StateMachine stateMachine;
        IdleState idleState;
        NavMeshAgent agent;
        Vector3 patrolRadiusCenter;
        Animator animator;
        public float CurrentAttackDamage { get; private set; }

        bool inAttack;

        float distanceToTarget => Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(targetComponent.TargetPosition.x, 0, targetComponent.TargetPosition.z));

        public UnityEvent<int> OnDeathEvent;

        void Awake()
        {
            CurrentAttackDamage = baseAttackDamage;
            patrolRadiusCenter = transform.position;
            targetComponent = new TargetComponent();
            idleTargetComponent = new TargetComponent();
            agent = GetComponent<NavMeshAgent>();
            agent.speed = baseMoveSpeed;
            animator = GetComponent<Animator>();
            HealthSystemModule = GetComponent<HealthSystemModule>();
            attackCollider = GetComponentInChildren<AttackCollider>();
            HealthSystemModule.RegisterEffectHandler(EffectType.DamageOverTime, new DamageOverTimeHandler());
            HealthSystemModule.RegisterEffectHandler(EffectType.Debuff, new RendTheFleshEffectHandler());
            HealthSystemModule.RegisterEffectHandler(EffectType.DamageOverTimeScaling, new DamageOverTimeScalingHandler());
            var idleTimer = new Timer(idleDuration);
            //State Creation
            idleState = new IdleState(idleTimer, agent,animator);
            State chaseState = new WalkToPointState(agent, targetComponent,animator);
            State patrolState = new PatrolState(agent, idleTargetComponent, RecalculatePatrolPoint,animator);
            State attackState = new AttackState(attackCollider, new Timer(attackCooldown),animator, this);

            //Setup StateMachine
            stateMachine = new StateMachine(idleState, gameObject, false);

            //Setup Transitions
            var anyToChase = new Transition(chaseState, FindTarget);
            var chaseToIdle = new Transition(idleState, () => !FindTarget());
            var chaseToAttack = new Transition(attackState, () => distanceToTarget < attackRange);
            var attackToChase = new Transition(chaseState, () => distanceToTarget > attackRange && !inAttack);
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

        public void CallHit(List<ESkillTag> _skillTags)
        {
            var skillTag = _skillTags.Contains(ESkillTag.Light) ? ESkillTag.Light : ESkillTag.Dark;
            switch (skillTag)
            {
                case ESkillTag.Light:
                    lightHit.Spawn(transform.position + new Vector3(0,transform.localScale.y * 0.5f,0));
                    break;
                case ESkillTag.Dark:
                    darkHit.Spawn(transform.position + new Vector3(0,transform.localScale.y * 0.5f,0));
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void Die()
        {
            spawner.EnemyDied(this);
            OnDeathEvent.Invoke(CalculateDrop());
            Destroy(gameObject);
        }

        int CalculateDrop()
        {
            return Random.Range(memoryFragmentDropMin, memoryFragmentDropMax + 1);
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
            return Physics.Raycast(transform.position, (_target.position - transform.position).normalized, Vector3.Distance(transform.position, _target.position), obstructionMask);
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

        public void UpdateMoveSpeed(float _newValue)
        {
            agent.speed = baseMoveSpeed + _newValue;
        }

        public void ResetMoveSpeed()
        {
            if (agent == null) return;
            agent.speed = baseMoveSpeed;
        }

        public void UpdateAttackDamage(float _newValue)
        {
            CurrentAttackDamage = baseAttackDamage + _newValue;
        }

        public void ResetAttackDamage()
        {
            CurrentAttackDamage = baseAttackDamage;
        }

        public void EnableAttackCollider()
        {
            attackCollider.gameObject.SetActive(true);
        }

        public void DisableAttackCollider()
        {
            attackCollider.gameObject.SetActive(false);
        }

        public void EnableAttackBool()
        {
            inAttack = true;
        }

        public void DisableAttackBool()
        {
            inAttack = false;
        }
    }
}