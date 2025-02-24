using LL_Unity_Utils.Misc;
using LL_Unity_Utils.Timers;
using Scripts.Core.AudioScripts;
using Scripts.Core.KI;
using Scripts.Core.Skills.Effects;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Core.Modules
{
    public class BossAIModule : EnemyAIModule
    {
        [SerializeField] float normalAttackDamage;
        [SerializeField] float heavyAttackDamage;

        protected override void Awake()
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
            State meleeAttackState = new BossAttackState(attackCollider, animator, this, 0, normalAttackDamage, heavyAttackDamage);
            State jumpAttackState = new BossAttackState(attackCollider, animator, this, 1, normalAttackDamage, heavyAttackDamage);
            State deathState = new DeathState(animator);


            stateMachine = new StateMachine(idleState, gameObject, true);

            var anyToChase = new Transition(chaseState, FindTarget);
            var chaseToIdle = new Transition(idleState, () => !FindTarget());
            var chaseToMeleeAttack = new Transition(meleeAttackState, () => distanceToTarget < attackRange && GetRandomAttack() == 0);
            var chaseToJumpAttack = new Transition(jumpAttackState, () => distanceToTarget < attackRange && GetRandomAttack() == 1);
            var anyAttackToChase = new Transition(chaseState, () => distanceToTarget > attackRange && !inAttack);
            var meleeToJumpAttack = new Transition(jumpAttackState, () => !inAttack && distanceToTarget < attackRange && GetRandomAttack() == 1);
            var jumpToMeleeAttack = new Transition(meleeAttackState, () => !inAttack && distanceToTarget < attackRange && GetRandomAttack() == 0);
            var idleToPatrol = new Transition(patrolState, () => idleState.IsTimerFinished == true);
            var movingToIdle = new Transition(idleState, () => agent.remainingDistance < agent.stoppingDistance);
            var anyToDeath = new Transition(deathState, () => HealthSystemModule.IsDead == true);

            idleState.AddTransition(anyToDeath);
            idleState.AddTransition(anyToChase);
            idleState.AddTransition(idleToPatrol);

            chaseState.AddTransition(anyToDeath);
            chaseState.AddTransition(chaseToMeleeAttack);
            chaseState.AddTransition(chaseToJumpAttack);
            chaseState.AddTransition(chaseToIdle);

            patrolState.AddTransition(anyToDeath);
            patrolState.AddTransition(anyToChase);
            patrolState.AddTransition(movingToIdle);

            meleeAttackState.AddTransition(anyToDeath);
            meleeAttackState.AddTransition(anyAttackToChase);
            meleeAttackState.AddTransition(meleeToJumpAttack);

            jumpAttackState.AddTransition(anyToDeath);
            jumpAttackState.AddTransition(anyAttackToChase);
            jumpAttackState.AddTransition(jumpToMeleeAttack);

            StartCoroutine(PlayIdleSounds());
        }


        int GetRandomAttack()
        {
            return Random.Range(0, 2);
        }

        public void PlayWalkSound()
        {
            soundSystem.PlayWalkClip();
        }
    }
}