using System.Collections.Generic;
using System.Linq;
using Lukas.Scripts.Core.Modules;
using Lukas.Scripts.Core.Skills.Effects;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    [CreateAssetMenu(menuName = "Scriptables/Skills/BehaviorSO/ExplosionBehaviorSO")]
    public class ExplosionBehaviorSO : SkillBehaviorSO
    {
        [SerializeField] string specificName;
        [SerializeField] List<ESkillTag> tags;
        [SerializeField] int explosionDamage;
        [SerializeField] float explosionRange;
        public override string SpecificName => specificName;

        public override List<ESkillTag> Tags => tags;

        [SerializeField] EffectData savedContextEffect;
        
        public override void Execute(SkillContext _context, ref int _totalDamage)
        {
            _context.OnEnemyKilled += HandleEnemyDeath;
        }

        void HandleEnemyDeath(EnemyAIModule _target)
        {
            var nearbyEnemies = Physics.OverlapSphere(_target.transform.position, explosionRange)
                .Select(_collider => _collider.GetComponent<EnemyAIModule>())
                .Where(_newTarget => _newTarget != null && !_newTarget.HealthSystemModule.IsDead)
                .ToList();

            foreach (var target in nearbyEnemies)
            {
                target.HealthSystemModule.TakeDamage(explosionDamage);
                target.HealthSystemModule.AddEffect(new Effect(savedContextEffect));
            }
        }
    }
}