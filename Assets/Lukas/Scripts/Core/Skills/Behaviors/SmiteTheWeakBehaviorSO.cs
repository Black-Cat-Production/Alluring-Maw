using System.Collections.Generic;
using System.Linq;
using Scripts.Core.Modules;
using UnityEngine;

namespace Scripts.Core.Skills.Behaviors
{
    [CreateAssetMenu(menuName = "Scriptables/Skills/BehaviorSO/SmiteTheWeakBehaviorSO")]
    public class SmiteTheWeakBehaviorSO : SkillBehaviorSO
    {
        [SerializeField] string specificName;
        [SerializeField] List<ESkillTag> tags;
        [SerializeField] float damageIncrease;
        [SerializeField] float damageIncreaseStep;

        [Header("Explosion Config")]
        [SerializeField] float explosionRange;
        [SerializeField] float explosionDamage;
        public override string SpecificName => specificName;

        public override List<ESkillTag> Tags => tags;
        public override void Execute(SkillContext _context, ref int _totalDamage)
        {
            if (_context.Targets.Count > 0)
            {
                var target = _context.Targets[0];
                float percentageValue = target.HealthSystemModule.GetCurrentPercentageHealth();
                float missingPercentageValue = 100f - percentageValue;
                float calculatedDamageStep = missingPercentageValue / damageIncreaseStep;
                _totalDamage += Mathf.FloorToInt(damageIncrease * calculatedDamageStep);
                _context.OnEnemyKilled += ExplosionEffect;
            }
        }

        void ExplosionEffect(EnemyAIModule _target)
        {
            var nearbyEnemies = Physics.OverlapSphere(_target.transform.position, explosionRange)
                .Select(_collider => _collider.GetComponent<EnemyAIModule>())
                .Where(_newTarget => _newTarget != null && !_newTarget.HealthSystemModule.IsDead)
                .ToList();

            foreach (var target in nearbyEnemies)
            {
                target.HealthSystemModule.TakeDamage(explosionDamage);
            }
        }
    }
}