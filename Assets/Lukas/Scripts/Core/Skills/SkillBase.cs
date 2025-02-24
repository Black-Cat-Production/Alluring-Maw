using System.Collections.Generic;
using LL_Unity_Utils.Timers;
using Scripts.Core.Modules;
using Scripts.Core.Skills.Behaviors;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Core.Skills
{
    public abstract class SkillBase : SkillBridgeUnity
    {
        [SerializeField] float despawnTimerDuration;
        [SerializeField] protected int baseSkillHitDamage;
        [SerializeField] protected LayerMask obstructionLayer;
        [SerializeField] protected List<SkillBehaviorSO> baseBehaviours;
        Timer despawnTimer;

        [SerializeField] protected UnityEvent<Vector3> OnSpawn;

        protected virtual void Awake()
        {
            despawnTimer = new Timer(despawnTimerDuration);
            despawnTimer.StartTimer();
        }

        void Start()
        {
            foreach (var behaviour in baseBehaviours) AddBehavior(behaviour);
            OnSpawn?.Invoke(gameObject.transform.position);
        }


        void FixedUpdate()
        {
            if (despawnTimer.CheckTimer()) Destroy(gameObject);
        }

        protected virtual void Use(SkillContext _context, Vector3 _hitPosition)
        {
            int totalDamage = baseSkillHitDamage;
            foreach (var behavior in behaviors) behavior.Execute(_context, ref totalDamage);

            if (_context.Targets is not { Count: > 0 }) return;
            var target = _context.Targets[0];
            target.HealthSystemModule.TakeDamage(totalDamage);
            target.CallHit(Tags, _hitPosition);
            if (target.HealthSystemModule.IsDead) _context.TriggerEnemyKilled(target);
        }

        public virtual void OnTriggerEnter(Collider _collider)
        {
            if (_collider.gameObject.CompareTag("ProjectileKillBox"))
            {
                Destroy(gameObject);
                return;
            }

            if (_collider.gameObject.CompareTag("HitBox"))
            {
                var target = _collider.gameObject.GetComponentInParent<EnemyAIModule>();
                var context = new SkillContext(transform.position, new List<EnemyAIModule> { target }, null, this);
                var collidingPoint = _collider.ClosestPointOnBounds(transform.position);
                Use(context, collidingPoint);
            }

            Destroy(gameObject);
        }
    }
}