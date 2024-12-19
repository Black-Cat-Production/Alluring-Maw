using System;
using System.Collections.Generic;
using Lukas.Scripts.Core.Modules;
using Unity.VisualScripting;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public class DamageModifierDecorator : ISkill
    {
        readonly ISkill baseSkill;
        readonly float additionalDamage;

        public DamageModifierDecorator(ISkill _baseSkill, float _additionalDamage)
        {
            baseSkill = _baseSkill;
            additionalDamage = _additionalDamage;
        }

        public void Use(Vector3 _startPosition, List<HealthSystemModule> _enemiesInRange)
        {
            foreach (var enemy in _enemiesInRange)
            {
                enemy.OnBeforeTakeDamage += ModifyDamage;
            }
            
            baseSkill.Use(_startPosition, _enemiesInRange);

            foreach (var enemy in _enemiesInRange)
            {
                enemy.OnBeforeTakeDamage -= ModifyDamage;
            }
        }

        float ModifyDamage(float _baseDamage)
        {
            return _baseDamage + additionalDamage;
        }
    }
}