using System;
using System.Collections.Generic;
using System.Numerics;
using Lukas.Scripts.Core.Modules;

namespace Lukas.Scripts.Core.Skills
{
    public class SkillContext
    {
        public Vector3 StartingPosition;
        public List<HealthSystemModule> Targets;
        public Action<HealthSystemModule> OnEnemyKilled;

        public SkillContext(Vector3 _startingPosition, List<HealthSystemModule> _targets, Action<HealthSystemModule> _onEnemyKilled)
        {
            StartingPosition = _startingPosition;
            Targets = _targets;
            OnEnemyKilled = _onEnemyKilled;
        }

        public void TriggerEnemyKilled(HealthSystemModule _target)
        {
            OnEnemyKilled?.Invoke(_target);
        }
    }
}