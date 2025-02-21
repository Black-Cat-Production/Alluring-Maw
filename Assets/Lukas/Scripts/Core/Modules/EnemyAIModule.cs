using System;
using System.Collections;
using System.Collections.Generic;
using LL_Unity_Utils.Misc;
using LL_Unity_Utils.Scriptables;
using Scripts.Core.AudioScripts;
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
        public HealthSystemModule HealthSystemModule { get; protected set; }

        RoomSpawner spawner;

        [SerializeField] protected float searchRadius;
        [SerializeField] protected LayerMask detectionMask;
        [SerializeField] protected LayerMask obstructionMask;
        [SerializeField] protected float idleDuration;
        [SerializeField] protected float PatrolRange;
        [SerializeField] protected float PatrolPointDistanceThreshhold;
        [SerializeField] protected float attackRange;
        [SerializeField] protected float baseAttackDamage;
        [SerializeField] protected float baseMoveSpeed;

        [Header("Drop Values")]
        [SerializeField] protected int memoryFragmentDropMin;

        [SerializeField] protected int memoryFragmentDropMax;

        [Header("Visuals")]
        [SerializeField] protected VFXSpawner lightHit;

        [SerializeField] protected VFXSpawner darkHit;

        [Header("Cleanup")]
        [SerializeField] protected float durationTillDespawnAfterDeath;

        protected AttackCollider attackCollider;
        protected TargetComponent targetComponent;
        protected TargetComponent idleTargetComponent;
        protected StateMachine stateMachine;
        protected IdleState idleState;
        protected NavMeshAgent agent;
        protected Vector3 patrolRadiusCenter;
        protected Animator animator;
        protected EnemySoundSystem soundSystem;

        protected Timer despawnTimer;
        public float CurrentAttackDamage { get; protected set; }

        [NonSerialized] public bool AllowedAggro;

        protected bool inAttack;
        public bool InAttack => inAttack;

        protected float distanceToTarget => Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(targetComponent.TargetPosition.x, 0, targetComponent.TargetPosition.z));

        public UnityEvent<int> OnDeathEvent;
        public UnityEvent<Vector3> OnDeathEffectEvent;

        protected virtual void Awake()
        {
            CurrentAttackDamage = baseAttackDamage;
            patrolRadiusCenter = transform.position;
            targetComponent = new TargetComponent();
            idleTargetComponent = new TargetComponent();
            agent = GetComponent<NavMeshAgent>();
            agent.speed = baseMoveSpeed;
            animator = GetComponent<Animator>();
            HealthSystemModule = GetComponent<HealthSystemModule>();
            soundSystem = GetComponent<EnemySoundSystem>();
            attackCollider = GetComponentInChildren<AttackCollider>();
            attackCollider.gameObject.SetActive(false);
            HealthSystemModule.RegisterEffectHandler(EffectType.DamageOverTime, new DamageOverTimeHandler());
            HealthSystemModule.RegisterEffectHandler(EffectType.Debuff, new RendTheFleshEffectHandler());
            HealthSystemModule.RegisterEffectHandler(EffectType.DamageOverTimeScaling, new DamageOverTimeScalingHandler());
            var idleTimer = new Timer(idleDuration);
            despawnTimer = new Timer(durationTillDespawnAfterDeath);


            idleState = new IdleState(idleTimer, animator);
            State chaseState = new WalkToPointState(agent, targetComponent, animator);
            State patrolState = new PatrolState(agent, idleTargetComponent, RecalculatePatrolPoint, animator);
            State attackState = new AttackState(attackCollider, animator, this);
            State deathState = new DeathState(animator);


            stateMachine = new StateMachine(idleState, gameObject, false);


            var anyToChase = new Transition(chaseState, FindTarget);
            var chaseToIdle = new Transition(idleState, () => !FindTarget());
            var chaseToAttack = new Transition(attackState, () => distanceToTarget < attackRange);
            var attackToChase = new Transition(chaseState, () => distanceToTarget > attackRange && !inAttack);
            var idleToPatrol = new Transition(patrolState, () => idleState.IsTimerFinished == true);
            var movingToIdle = new Transition(idleState, () => agent.remainingDistance < agent.stoppingDistance);
            var anyToDeath = new Transition(deathState, (() => HealthSystemModule.IsDead == true));


            idleState.AddTransition(anyToDeath);
            idleState.AddTransition(anyToChase);
            idleState.AddTransition(idleToPatrol);

            chaseState.AddTransition(anyToDeath);
            chaseState.AddTransition(chaseToAttack);
            chaseState.AddTransition(chaseToIdle);

            patrolState.AddTransition(anyToDeath);
            patrolState.AddTransition(anyToChase);
            patrolState.AddTransition(movingToIdle);

            attackState.AddTransition(anyToDeath);
            attackState.AddTransition(attackToChase);

            StartCoroutine(PlayIdleSounds());
        }

        protected IEnumerator PlayIdleSounds()
        {
            while (!HealthSystemModule.IsDead)
            {
                if(!soundSystem.GetIsPlaying()) soundSystem.PlayIdleClip();
                yield return new WaitForSeconds(5);
            }
        }

        void FixedUpdate()
        {
            if (HealthSystemModule.IsDead)
            {
                if (despawnTimer.CheckTimer()) Destroy(gameObject);
            }

            stateMachine.CheckSwapState();
        }

        public void CallHit(List<ESkillTag> _skillTags, Vector3 _positionToSpawn)
        {
            var skillTag = _skillTags.Contains(ESkillTag.Light) ? ESkillTag.Light : ESkillTag.Dark;
            switch (skillTag)
            {
                case ESkillTag.Light:
                    lightHit.Spawn(_positionToSpawn + transform.forward);
                    break;
                case ESkillTag.Dark:
                    darkHit.Spawn(_positionToSpawn + transform.forward);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void Die()
        {
            spawner.EnemyDied(this);
            OnDeathEvent.Invoke(CalculateDrop());
            OnDeathEffectEvent.Invoke(transform.position);
            despawnTimer.StartTimer();
        }

        int CalculateDrop()
        {
            return Random.Range(memoryFragmentDropMin, memoryFragmentDropMax + 1);
        }

        public void SetSpawner(RoomSpawner _spawner)
        {
            spawner = _spawner;
        }

        protected bool FindTarget()
        {
            if (!AllowedAggro) return false;
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

        protected void RecalculatePatrolPoint()
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

        // Every function down below is for the animation event based system
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
        
        public void TurnToTarget()
        {
            agent.transform.LookAt(targetComponent.TargetPosition);
        }
    }
}