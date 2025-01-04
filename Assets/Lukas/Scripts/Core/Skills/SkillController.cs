using System;
using Lukas.Scripts.Core.Modules;
using UnityEditor.Rendering;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public class SkillController : MonoBehaviour
    {
        [SerializeField] SkillBridgeUnity defaultAttack;
        [SerializeField] float spawnDistance;
        [SerializeField] Camera playerCamera;
        public SkillBridgeUnity SelectedSkill { get; private set; }

        ManaSystemModule manaSystemModule;

        void Awake()
        {
            manaSystemModule = GetComponent<ManaSystemModule>();
            // SkillTreeManager.Instance.UpdateBehaviors();
        }

        public void SetSkill(SkillBridgeUnity _skill)
        {
            SelectedSkill = _skill;
        }

        public void UseSkill()
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
            var cameraTransform = playerCamera.transform;
            var instance = Instantiate(SelectedSkill, cameraTransform.position + cameraTransform.forward * spawnDistance, cameraTransform.rotation);
            instance.GetComponent<Rigidbody>().AddForce(cameraTransform.transform.forward * 50f, ForceMode.Impulse);
        }

        public void CastDefaultAttack()
        {
            var cameraTransform = playerCamera.transform;
            var instance = Instantiate(defaultAttack, cameraTransform.position + cameraTransform.forward * spawnDistance, cameraTransform.rotation);
            instance.GetComponent<Rigidbody>().AddForce(cameraTransform.transform.forward * 50f, ForceMode.Impulse);
        }
    }
}