using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Core.UI
{
    public class SkillSelectionUI : MonoBehaviour
    {
        [SerializeField] List<Image> skillFrames;
        [SerializeField] List<Image> skillCooldownImages;
        [SerializeField] Sprite selectedFrame;
        [SerializeField] Sprite unselectedFrame;

        void Awake()
        {
            foreach (var cooldownImage in skillCooldownImages) cooldownImage.fillAmount = 0;
        }


        public void UpdateSkillUI(int _index)
        {
            foreach (var frame in skillFrames) frame.sprite = unselectedFrame;
            skillFrames[_index].sprite = selectedFrame;
        }

        public void UpdateCooldownUI(int _index, float _cooldownDuration)
        {
            StartCoroutine(DisplayCooldown(skillCooldownImages[_index], _cooldownDuration));
        }

        IEnumerator DisplayCooldown(Image _image, float _time)
        {
            _image.fillAmount = 1;
            float elapsedTime = 0f;
            while (elapsedTime < _time)
            {
                float t = elapsedTime / _time;
                float interpolationValue = Mathf.Lerp(1, 0, t);
                _image.fillAmount = interpolationValue;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _image.fillAmount = 0;
        }
    }
}