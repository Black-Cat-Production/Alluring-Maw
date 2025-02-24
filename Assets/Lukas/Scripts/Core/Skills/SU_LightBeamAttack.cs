using System.Collections.Generic;
using LL_Unity_Utils.Scriptables;
using Scripts.Core.Modules;
using UnityEngine;

namespace Scripts.Core.Skills
{
    public class SU_LightBeamAttack : SkillBase
    {
        SkillContext context;
        bool hitEnemy;
        bool hitGround;

        [SerializeField] VFXSpawner darkVFXSpawner;
        [SerializeField] VFXSpawner lightVFXSpawner;


        protected void Start()
        {
            context = new SkillContext(transform.position, null, null, this);
            OnSpawn.Invoke(gameObject.transform.position);
        }

        public override void OnTriggerEnter(Collider _collider)
        {
            if (_collider.gameObject.CompareTag("HitBox"))
            {
                var target = _collider.gameObject.GetComponentInParent<EnemyAIModule>();
                context.Targets = new List<EnemyAIModule> { target };
            }
            else
            {
                context.Targets = null;
            }

            var collidingPoint = _collider.ClosestPointOnBounds(transform.position);
            Use(context, collidingPoint);
        }

        public void SpawnVFX(Vector3 _position)
        {
            if (Tags.Contains(ESkillTag.Dark))
                darkVFXSpawner.Spawn(_position);
            else
                lightVFXSpawner.Spawn(_position);
        }
    }
}