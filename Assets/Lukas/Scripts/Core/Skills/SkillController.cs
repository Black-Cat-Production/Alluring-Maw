using System;
using Lukas.Scripts.Core.Modules;
using UnityEditor.Rendering;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public class SkillController : MonoBehaviour
    {
        public SkillBridgeUnity SelectedSkill { get; private set; }

        ManaSystemModule manaSystemModule;

        void Awake()
        {
            manaSystemModule = GetComponent<ManaSystemModule>();
        }

        public void SetSkill(SkillBridgeUnity _skill)
        {
            SelectedSkill = _skill;
        }

        public void CastSkill(Camera _playerCamera, float _spawnDistance)
        {
            if (SelectedSkill == null)
            {
                Debug.LogWarning("No skill assigned to SkillController!");
                return;
            }

            if (SelectedSkill.ManaCost >= manaSystemModule.CurrentMana)
            {
                Debug.Log("You dont have enough mana to cast!");
                return;
            }
            manaSystemModule.ReduceMana(SelectedSkill.ManaCost);
            var cameraTransform = _playerCamera.transform;
            var instance = Instantiate(SelectedSkill, cameraTransform.position + cameraTransform.forward * _spawnDistance, cameraTransform.rotation);
            instance.GetComponent<Rigidbody>().AddForce(cameraTransform.transform.forward * 50f, ForceMode.Impulse);
        }
    }
}