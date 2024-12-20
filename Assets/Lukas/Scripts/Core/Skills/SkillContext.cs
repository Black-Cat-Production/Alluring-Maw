using System;
using System.Collections.Generic;
using Lukas.Scripts.Core.Modules;
using Lukas.Scripts.Core.Skills.Effects;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public class SkillContext
    {
        public Vector3 StartingPosition;
        public List<HealthSystemModule> Targets;
        public Action<HealthSystemModule> OnEnemyKilled;
        public Effect Effect;

        public SkillContext(Vector3 _startingPosition, List<HealthSystemModule> _targets, Effect _effect)
        {
            StartingPosition = _startingPosition;
            Targets = _targets;
            Effect = _effect;
        }

        public void TriggerEnemyKilled(HealthSystemModule _target)
        {
            OnEnemyKilled?.Invoke(_target);
        }
    }
}