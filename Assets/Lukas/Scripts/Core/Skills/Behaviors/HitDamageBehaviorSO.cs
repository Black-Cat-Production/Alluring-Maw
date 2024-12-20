using System;
using System.Collections.Generic;
using Lukas.Scripts.Core.Modules;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    [CreateAssetMenu(menuName = "Scriptables/Skills/BehaviorSO/HitDamageBehavior")]
    public class HitDamageBehaviorSO : SkillBehaviorSO
    {
        [SerializeField] string specificName;
        [SerializeField] List<string> tags;
        public int damageIncrease;

        public override string SpecificName => specificName;

        public override List<string> Tags => tags;

        public override void Execute(Vector3 _startPosition, List<HealthSystemModule> _enemiesInRange, ref int _totalDamage)
        {
            _totalDamage += damageIncrease;
        }
    }
}