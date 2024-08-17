using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider mouseSensitivitySlider;

    private void Awake()
    {
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeSliderValueChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderValueChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeSliderValueChanged);
        mouseSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivitySliderValueChanged);

        masterVolumeSlider.value = SerializeManager.GetMasterVolume();
        sfxVolumeSlider.value = SerializeManager.GetSFXVolume();
        musicVolumeSlider.value = SerializeManager.GetMusicVolume();
        mouseSensitivitySlider.value = SerializeManager.GetMouseSensitivity();
    }

    private void OnMasterVolumeSliderValueChanged(float value)
    {
        SerializeManager.SetMasterVolume(value);
    }

    private void OnSfxVolumeSliderValueChanged(float value)
    {
        SerializeManager.SetSFXVolume(value);
    }

    private void OnMusicVolumeSliderValueChanged(float value)
    {
        SerializeManager.SetMusicVolume(value);
    }

    private void OnMouseSensitivitySliderValueChanged(float value)
    {
        SerializeManager.SetMouseSensitivity(value);
    }
}