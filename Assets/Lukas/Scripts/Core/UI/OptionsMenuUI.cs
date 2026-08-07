using Scripts.Program;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Scripts.Core.UI
{
    public class OptionsMenuUI : MonoBehaviour
    {
        [SerializeField] Canvas optionsMenuCanvas;
        [SerializeField] Slider mouseSensSlider;
        [SerializeField] Slider masterVolumeSlider;
        [SerializeField] Slider sfxVolumeSlider;
        [SerializeField] Slider musicVolumeSlider;
        [SerializeField] Slider voiceVolumeSlider;
        [SerializeField] TextMeshProUGUI mouseSensSliderValue;
        [SerializeField] TextMeshProUGUI masterVolumeSliderValue;
        [SerializeField] TextMeshProUGUI sfxVolumeSliderValue;
        [SerializeField] TextMeshProUGUI musicVolumeSliderValue;
        [SerializeField] TextMeshProUGUI voiceVolumeSliderValue;
        [SerializeField] OptionsSaveSO optionsSaveSO;

        [SerializeField] AudioMixer audioMixer;

        const float sliderMin = 0;
        const float sliderMax = 100;
        const float audioMin = -80;
        const float audioMax = 20;

        public void OpenOptionsMenu()
        {
            optionsMenuCanvas.gameObject.SetActive(true);
            mouseSensSlider.value = optionsSaveSO.MouseSense;
            masterVolumeSlider.value = optionsSaveSO.MasterVolume;
            sfxVolumeSlider.value = optionsSaveSO.SFXVolume;
            musicVolumeSlider.value = optionsSaveSO.MusicVolume;
            voiceVolumeSlider.value = optionsSaveSO.VoiceVolume;
            UpdateValue();
            PushUpdateToAudioMixer();
        }

        public void CloseOptionsMenu()
        {
            optionsSaveSO.MouseSense = mouseSensSlider.value;
            optionsSaveSO.MasterVolume = masterVolumeSlider.value;
            optionsSaveSO.SFXVolume = sfxVolumeSlider.value;
            optionsSaveSO.MusicVolume = musicVolumeSlider.value;
            optionsSaveSO.VoiceVolume = voiceVolumeSlider.value;
            optionsMenuCanvas.gameObject.SetActive(false);
            PushUpdateToAudioMixer();
        }

        public void UpdateValue()
        {
            mouseSensSliderValue.text = mouseSensSlider.value.ToString();
            masterVolumeSliderValue.text = masterVolumeSlider.value.ToString();
            sfxVolumeSliderValue.text = sfxVolumeSlider.value.ToString();
            musicVolumeSliderValue.text = musicVolumeSlider.value.ToString();
            voiceVolumeSliderValue.text = voiceVolumeSlider.value.ToString();
            PushUpdateToAudioMixer();
        }

        public void PushUpdateToAudioMixer()
        {
            //audioMixer.SetFloat("MasterVolume", Remap(optionsSaveSO.MasterVolume, sliderMin, sliderMax, audioMin, audioMax));
            //audioMixer.SetFloat("SFXVolume", Remap(optionsSaveSO.SFXVolume, sliderMin, sliderMax, audioMin, audioMax));
            //audioMixer.SetFloat("MusicVolume", Remap(optionsSaveSO.MusicVolume, sliderMin, sliderMax, audioMin, audioMax));
            //audioMixer.SetFloat("VoiceVolume", Remap(optionsSaveSO.VoiceVolume, sliderMin, sliderMax, audioMin, audioMax));
            AkSoundEngine.SetRTPCValue("Master_Volume", masterVolumeSlider.value);
            AkSoundEngine.SetRTPCValue("SFX_Volume", sfxVolumeSlider.value);
            AkSoundEngine.SetRTPCValue("Music_Volume", musicVolumeSlider.value);
            AkSoundEngine.SetRTPCValue("Voice_Volume", voiceVolumeSlider.value);
        }

        public void PushUpdateOnLoad()
        {
            AkSoundEngine.SetRTPCValue("Master_Volume", optionsSaveSO.MasterVolume);
            AkSoundEngine.SetRTPCValue("SFX_Volume", optionsSaveSO.SFXVolume);
            AkSoundEngine.SetRTPCValue("Music_Volume", optionsSaveSO.MusicVolume);
            AkSoundEngine.SetRTPCValue("Voice_Volume", optionsSaveSO.VoiceVolume);
        }

        float Remap(float _input, float _oldMin, float _oldMax, float _newMin, float _newMax)
        {
            return _newMin + (_input - _oldMin) * (_newMax - _newMin) / (_oldMax - _oldMin);
        }
    }
}