using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public class SkillController : MonoBehaviour
    {
        public ISkill SelectedSkill { get; private set; }

        public void SetSkill(ISkill _skill)
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
            var selectedSkillMono = (Skill)SelectedSkill;
            var instance = Instantiate(selectedSkillMono, cameraTransform.position + cameraTransform.forward * _spawnDistance, cameraTransform.rotation);
            instance.GetComponent<Rigidbody>().AddForce(cameraTransform.transform.forward * selectedSkillMono.ShootingSpeed , ForceMode.Impulse);
        }
    }
}