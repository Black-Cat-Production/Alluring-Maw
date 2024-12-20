using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public class SkillController : MonoBehaviour
    {
        public SkillBridgeUnity SelectedSkill { get; private set; }
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
            var cameraTransform = _playerCamera.transform;
            var instance = Instantiate(SelectedSkill, cameraTransform.position + cameraTransform.forward * _spawnDistance, cameraTransform.rotation);
            instance.GetComponent<Rigidbody>().AddForce(cameraTransform.transform.forward * 50f, ForceMode.Impulse);
        }
    }
}