using System;
using System.Collections;
using UnityEngine;

namespace Lukas.Scripts.Core
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