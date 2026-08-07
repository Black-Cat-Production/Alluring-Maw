using System;
using UnityEngine;
using Event = AK.Wwise.Event;

namespace Scripts.Core.Visual
{
    public class GoodJobArm : MonoBehaviour
    {
        [SerializeField] Event spawnSoundEvent;

        void Start()
        {
            AkSoundEngine.PostEvent(spawnSoundEvent.Name, gameObject);
        }
    }
}