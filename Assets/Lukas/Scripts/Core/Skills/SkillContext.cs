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
        public SkillBridgeUnity OriginalSkill;
        public List<EnemyAIModule> Targets;
        public Action<EnemyAIModule> OnEnemyKilled;
        public Effect Effect;
        public readonly HealthSystemModule CasterHealthModule;
        public int timesCalled;

        public SkillContext(Vector3 _startingPosition, List<EnemyAIModule> _targets, Effect _effect, SkillBridgeUnity _skill)
        {
            StartingPosition = _startingPosition;
            Targets = _targets;
            Effect = _effect;
            CasterHealthModule = GameObject.Find("Player").GetComponent<HealthSystemModule>();
            OriginalSkill = _skill;
        }
        
        public void TriggerEnemyKilled(EnemyAIModule _target)
        {
            OnEnemyKilled?.Invoke(_target);
        }
    }
}