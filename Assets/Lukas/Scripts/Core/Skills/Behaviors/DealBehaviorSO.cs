using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Core.Skills.Behaviors
{
    [CreateAssetMenu(menuName = "Scriptables/Skills/BehaviorSO/DealBehaviorSO")]
    public class DealBehaviorSO : SkillBehaviorSO
    {
        [SerializeField] string specificName;
        [SerializeField] List<ESkillTag> tags;

        [Header("OnUnlock Config")]
        [SerializeField] ESkillTag tagToChangeTo;

        [SerializeField] SkillBridgeUnity prefabToChange;

        [Header("OnSkillCast Config")]
        [SerializeField] SkillBridgeUnity prefabToCopyBehaviorFrom;

        public override string SpecificName => specificName;

        public override List<ESkillTag> Tags => tags;

        public override void Execute(SkillContext _context, ref int _totalDamage)
        {
            var copiedBehaviors = prefabToCopyBehaviorFrom.GetBehaviors();
            foreach (var behavior in _context.Targets.SelectMany(_ => copiedBehaviors)) behavior.Execute(_context, ref _totalDamage);
        }

        public override void OnUnlockExecute()
        {
            switch (tagToChangeTo)
            {
                case ESkillTag.Dark:
                    prefabToChange.Tags.Remove(ESkillTag.Light);
                    prefabToChange.Tags.Add(ESkillTag.Dark);
                    break;
                case ESkillTag.Light:
                    prefabToChange.Tags.Remove(ESkillTag.Dark);
                    prefabToChange.Tags.Add(ESkillTag.Light);
                    break;
            }
        }
    }
}