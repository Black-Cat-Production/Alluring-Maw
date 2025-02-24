using UnityEngine;

namespace Scripts.Core.Skills
{
    public sealed class SU_DefaultAttack : SkillBase
    {
        [Header("Skill Specific")]
        [SerializeField] GameObject darkVFXObject;

        [SerializeField] GameObject lightVFXObject;

        protected override void Awake()
        {
            base.Awake();
            if (Tags.Contains(ESkillTag.Dark))
            {
                if (lightVFXObject.activeInHierarchy) lightVFXObject.SetActive(false);
                darkVFXObject.SetActive(true);
            }
            else
            {
                if (darkVFXObject.activeInHierarchy) darkVFXObject.SetActive(false);
                lightVFXObject.SetActive(true);
            }
        }
    }
}