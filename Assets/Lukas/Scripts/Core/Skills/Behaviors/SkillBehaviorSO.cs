using System.Collections.Generic;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Scripts.Core.Skills.Behaviors
{
    public abstract class SkillBehaviorSO : ScriptableObject
    {
        public abstract string SpecificName { get; }
        public abstract List<ESkillTag> Tags { get; }
        public abstract void Execute(SkillContext _context, ref int _totalDamage);

        public virtual void OnUnlockExecute()
        {
            throw new NotImplementedException();
        }
    }
}