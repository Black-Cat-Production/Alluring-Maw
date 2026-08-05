using System;
using Scripts.Core.Skills.SkillTree;
using UnityEngine;
using Event = AK.Wwise.Event;

namespace WWISE_Integration_Scripts
{
    public class AudioSkillUnlockScript : MonoBehaviour
    {
        [SerializeField] SkillTreeUIManager skillTreeUIManager;
        [SerializeField] Event eventToPlay;

        public void OnClicked()
        {
            if (!skillTreeUIManager.CurrentSelectedNode.IsUnlockableByCost()) return;
            AkSoundEngine.PostEvent(eventToPlay.Name, gameObject);
        }
    }
}