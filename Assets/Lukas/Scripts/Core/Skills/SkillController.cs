using System.Collections;
using Lukas.Scripts.Core.Modules;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public class SkillController : MonoBehaviour
    {
        [SerializeField] SkillBridgeUnity defaultAttack;
        [SerializeField] float spawnDistance;
        [SerializeField] Camera playerCamera;
        [SerializeField] GameObject castPreviewPrefab;
        [SerializeField] LayerMask layerMask;
        public SkillBridgeUnity SelectedSkill { get; private set; }

        public ManaSystemModule ManaSystemModule { get; private set; }

        GameObject castPreviewPrefabInstance;
        bool castPreview;

        void Awake()
        {
            ManaSystemModule = GetComponent<ManaSystemModule>();
            // SkillTreeManager.Instance.UpdateBehaviors();
        }

        public void SetSkill(SkillBridgeUnity _skill)
        {
            SelectedSkill = _skill;
        }

        public void CastPreview()
        {
            if (castPreview) return;
            castPreview = true;
            if (!SelectedSkill.HasPreviewCast) return;
            StartCoroutine(HoldPreview());
        }

        void DestroyPreview()
        {
            castPreview = false;
            StopCoroutine(HoldPreview());
            if (castPreviewPrefabInstance != null) Destroy(castPreviewPrefabInstance.gameObject);
        }

        IEnumerator HoldPreview()
        {
            int debugSafety = 0;
            var cameraTransform = playerCamera.transform;
            while (castPreview)
            {
                bool hit = Physics.Raycast(cameraTransform.position + cameraTransform.forward * spawnDistance, cameraTransform.forward, out var rayHit, 10f);
                Debug.Log(hit);
                if (!hit)
                {
                    if (castPreviewPrefabInstance != null) Destroy(castPreviewPrefabInstance);
                    yield return null;
                }

                var spawnPosition = new Vector3(rayHit.point.x, 0.1f, rayHit.point.z);
                if (castPreviewPrefabInstance != null) castPreviewPrefabInstance.transform.position = spawnPosition;
                else castPreviewPrefabInstance = Instantiate(castPreviewPrefab, spawnPosition, Quaternion.identity);
                yield return null;
            }
        }

        public void UseSkill()
        {
            SkillBridgeUnity instance;
            if (SelectedSkill == null)
            {
                Debug.LogWarning("No skill assigned to SkillController!");
                return;
            }

            ManaSystemModule.ReduceMana(SelectedSkill.ManaCost);
            if (SelectedSkill.HasPreviewCast)
            {
                if (castPreviewPrefabInstance == null) return;
                instance = Instantiate(SelectedSkill, castPreviewPrefabInstance.transform.position + Vector3.up, Quaternion.identity);
                instance.GetComponent<Rigidbody>().AddForce(transform.up * 20f, ForceMode.Impulse);
                return;
            }

            var cameraTransform = playerCamera.transform;
            instance = Instantiate(SelectedSkill, cameraTransform.position + cameraTransform.forward * spawnDistance, cameraTransform.rotation);
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