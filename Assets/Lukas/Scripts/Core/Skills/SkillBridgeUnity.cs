using System;
using System.Collections.Generic;
using LL_Unity_Utils.Timers;
using Scripts.Core.Skills.Behaviors;
using UnityEngine;

namespace Scripts.Core.Skills
{
    public abstract class SkillBridgeUnity : MonoBehaviour
    {
        [SerializeField] string skillName;
        [SerializeField] List<ESkillTag> tags;

        
        [SerializeField] protected List<SkillBehaviorSO> behaviors = new();

        [SerializeField] float manaCost;
        [SerializeField] protected float cooldown;
        [SerializeField] bool hasPreviewCast;
        [SerializeField] List<ESkillTag> defaultTags;
        public string SkillName => skillName;
        public float ManaCost => manaCost;
        public List<ESkillTag> Tags => tags;
        public bool HasPreviewCast => hasPreviewCast;

        public float SkillCooldown => cooldown;

        public Timer CooldownTimer;

        void Awake()
        {
            CooldownTimer = new Timer(cooldown);
        }


        public void AddBehavior(SkillBehaviorSO _behavior)
        {
            behaviors.Add(_behavior);
        }

        public void RemoveBehavior(SkillBehaviorSO _behavior)
        {
            behaviors.Remove(_behavior);
        }

        public bool HasBehavior(SkillBehaviorSO _behavior)
        {
            return behaviors.Contains(_behavior);
        }

        public List<SkillBehaviorSO> GetBehaviors()
        {
            return behaviors;
        }

        public void ResetBehaviorList()
        {
            behaviors.Clear();
            tags.Clear();
            foreach (var defaultTag in defaultTags) tags.Add(defaultTag);
        }

        public void StartCooldown()
        {
            CooldownTimer?.StartTimer();
        }

        public bool GetIsOnCooldown()
        {
            return !CooldownTimer.CheckTimer();
        }
    }
}