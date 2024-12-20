using System.Collections.Generic;
using Lukas.Scripts.Core.Modules;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public abstract class SkillBehaviorSO : ScriptableObject
    {
        public abstract string SpecificName { get;}
        public abstract List<string> Tags { get;}
        public abstract void Execute(SkillContext _context, ref int _totalDamage);
    }
}