using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

using Lemmings.Enums;

public class UI_Settings : UI_Abstract
{
    [SerializeField] private Button_UIOnClick m_BackButton;

    [Header("SOUND")]
    [SerializeField] private Slider m_MasterVolumeSlider;
    [SerializeField] private Slider m_MusicVolumeSlider;
    [SerializeField] private Slider m_SFXVolumeSlider;

    public Action<VOLUME_SLIDER, float> onSliderChanged;
    public Action onSaveValues;

    private void Awake()
    {
        m_BackButton.OnClicked += BackButton;

        m_MasterVolumeSlider.onValueChanged.AddListener(delegate { ChangeMasterVolume(); });
        m_MusicVolumeSlider.onValueChanged.AddListener(delegate { ChangeMusicVolume(); });
        m_SFXVolumeSlider.onValueChanged.AddListener(delegate { ChangeSFXVolume(); });
    }

    private void OnDestroy()
    {
        m_MasterVolumeSlider.onValueChanged.RemoveAllListeners();

        m_BackButton.OnClicked -= BackButton;
    }

    protected override void BackButton()
    {
        base.BackButton();
        onSaveValues?.Invoke();
    }

    public void SetVolumeSliderValue(VOLUME_SLIDER slider, float value)
    {
        switch (slider)
        {
            case VOLUME_SLIDER.MASTERVOLUME:
                m_MasterVolumeSlider.value = value;
                break;

            case VOLUME_SLIDER.MUSICVOLUME:
                m_MusicVolumeSlider.value = value;
                break;
            case VOLUME_SLIDER.SFXVOLUME:
                m_SFXVolumeSlider.value = value;
                break;
        }
    }

    private void ChangeMasterVolume()
    {
        onSliderChanged?.Invoke(VOLUME_SLIDER.MASTERVOLUME, m_MasterVolumeSlider.value);
    }

    private void ChangeMusicVolume()
    {
        onSliderChanged?.Invoke(VOLUME_SLIDER.MUSICVOLUME, m_MusicVolumeSlider.value);
    }

    private void ChangeSFXVolume()
    {
        onSliderChanged?.Invoke(VOLUME_SLIDER.SFXVOLUME, m_SFXVolumeSlider.value);
    }
}
