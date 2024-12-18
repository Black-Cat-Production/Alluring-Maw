using UnityEngine;

namespace Lukas.Scripts.Core.Skills.Effects
{
    public class EffectRunner : MonoBehaviour
    {
        public static EffectRunner Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}