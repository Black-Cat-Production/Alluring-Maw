using System.Collections.Generic;
using Lukas.Scripts.Core.Modules;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public class ArcAttack : ISkill
    {
        public string Name { get; private set; }
        public float Damage { get; private set; }

        public ArcAttack(string _name, float _damage)
        {
            Name = _name;
            Damage = _damage;
        }
        public virtual void Use(Vector3 _startPosition, List<HealthSystemModule> _enemiesInRange)
        {
            if (_enemiesInRange.Count > 0)
            {
                var target = _enemiesInRange[0];
                target.TakeDamage(Damage);
                
            }
        }
    }
}