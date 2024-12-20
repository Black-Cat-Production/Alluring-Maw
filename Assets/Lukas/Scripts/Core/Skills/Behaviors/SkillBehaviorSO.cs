using System.Collections.Generic;
using Lukas.Scripts.Core.Modules;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public abstract class SkillBehaviorSO : ScriptableObject
    {
        public abstract string SpecificName { get;}
        public abstract List<string> Tags { get;}
        public abstract void Execute(Vector3 _startPosition, List<HealthSystemModule> _enemiesInRange, ref int _totalDamage);
    }
}