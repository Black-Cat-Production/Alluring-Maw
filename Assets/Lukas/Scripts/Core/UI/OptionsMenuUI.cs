using Scripts.Program;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Core.UI
{
    public class OptionsMenuUI : MonoBehaviour
    {
        [SerializeField] Canvas optionsMenuCanvas;
        [SerializeField] Slider mouseSensSlider;
        [SerializeField] TextMeshProUGUI mouseSensSliderValue;
        [SerializeField] OptionsSaveSO optionsSaveSO;

        public void OpenOptionsMenu()
        {
            optionsMenuCanvas.gameObject.SetActive(true);
            mouseSensSlider.value = optionsSaveSO.MouseSense;
            UpdateValue();
        }

        public void CloseOptionsMenu()
        {
            optionsMenuCanvas.gameObject.SetActive(false);
        }

        public void UpdateValue()
        {
            mouseSensSliderValue.text = mouseSensSlider.value.ToString();
            optionsSaveSO.MouseSense = mouseSensSlider.value;
        }
    }
}