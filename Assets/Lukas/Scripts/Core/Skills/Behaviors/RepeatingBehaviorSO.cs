using System.Collections;
using System.Collections.Generic;
using Scripts.Core.Skills.Effects;
using UnityEngine;

namespace Scripts.Core.Skills.Behaviors
{
    [CreateAssetMenu(menuName = "Scriptables/Skills/BehaviorSO/RepeatingBehaviorSO")]
    public class RepeatingBehaviorSO: SkillBehaviorSO
    {
        [SerializeField] string specificName;
        [SerializeField] List<ESkillTag> tags;

        [SerializeField] float repeatDelay;
        [SerializeField] SkillBridgeUnity skillToRepeat;

        public override string SpecificName => specificName;

        public override List<ESkillTag> Tags => tags;

        
        public override void Execute(SkillContext _context, ref int _totalDamage)
        {
            if (_context.timesCalled >= 1) return;
            _context.timesCalled += 1;
            EffectRunner.Instance.StartCoroutine(Repeat(_context));
        }

        IEnumerator Repeat(SkillContext _context)
        {
            yield return new WaitForSeconds(repeatDelay);
            var repeatedSkill = Instantiate(skillToRepeat, _context.StartingPosition, Quaternion.identity);
            repeatedSkill.ResetBehaviorList();
            repeatedSkill.GetComponent<Rigidbody>().AddForce(repeatedSkill.transform.up * 20f, ForceMode.Impulse);
            yield return null;
        }
    }
}