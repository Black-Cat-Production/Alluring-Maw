using System.Collections.Generic;
using Scripts.Core.Modules;
using UnityEngine;

namespace Scripts.Core.Skills
{
    public sealed class SU_ArcAttack : SkillBase
    {
        [Header("Skill Specific")]
        [SerializeField] GameObject darkVFXObject;
        [SerializeField] GameObject lightVFXObject;

        protected override void Awake()
        {
            base.Awake();
            if (Tags.Contains(ESkillTag.Dark))
            {
                if(lightVFXObject.activeInHierarchy) lightVFXObject.SetActive(false);
                darkVFXObject.SetActive(true);
            }
            else
            {
                if(darkVFXObject.activeInHierarchy) darkVFXObject.SetActive(false);
                lightVFXObject.SetActive(true);
            }
        }

        public override void OnTriggerEnter(Collider _collider)
        {
            if ((obstructionLayer.value & (1 << _collider.gameObject.layer)) != 0)
            {
                Destroy(gameObject);
                return;
            }

            if (!_collider.gameObject.CompareTag("HitBox")) return;
            var target = _collider.gameObject.GetComponentInParent<EnemyAIModule>();
            var context = new SkillContext(transform.position, new List<EnemyAIModule> { target }, null, this);
            Use(context);
        }
    }
}