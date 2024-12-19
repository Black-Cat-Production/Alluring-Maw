using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public abstract class SkillBridgeUnity : MonoBehaviour
    {
        [SerializeField] string skillName;
        public string SkillName => skillName;
    }
}