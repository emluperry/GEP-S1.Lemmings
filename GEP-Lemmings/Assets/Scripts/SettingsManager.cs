using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lemmings.Enums;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer m_AudioMixer;

    private UI_Settings m_SettingsUI;

    private void Start()
    {
        RestoreSavedSettings();
    }

    public void RestoreSettingMenuValues()
    {
        m_SettingsUI.SetVolumeSliderValue(VOLUME_SLIDER.MASTERVOLUME, PlayerPrefs.GetFloat(VOLUME_SLIDER.MASTERVOLUME.ToString(), 0));
        m_SettingsUI.SetVolumeSliderValue(VOLUME_SLIDER.MUSICVOLUME, PlayerPrefs.GetFloat(VOLUME_SLIDER.MUSICVOLUME.ToString(), 0));
        m_SettingsUI.SetVolumeSliderValue(VOLUME_SLIDER.SFXVOLUME, PlayerPrefs.GetFloat(VOLUME_SLIDER.SFXVOLUME.ToString(), 0));
    }

    public void ListenForEvents(UI_Settings Settings)
    {
        m_SettingsUI = Settings;
        Settings.onSliderChanged += ChangeVolume;
        Settings.onSaveValues += SaveOptions;
    }

    public void StopListeningForEvents()
    {
        m_SettingsUI.onSliderChanged -= ChangeVolume;
        m_SettingsUI.onSaveValues -= SaveOptions;
    }

    public void RestoreSavedSettings()
    {
        m_AudioMixer.SetFloat(VOLUME_SLIDER.MASTERVOLUME.ToString(), PlayerPrefs.GetFloat(VOLUME_SLIDER.MASTERVOLUME.ToString(), 0));
        
        m_AudioMixer.SetFloat(VOLUME_SLIDER.MUSICVOLUME.ToString(), PlayerPrefs.GetFloat(VOLUME_SLIDER.MUSICVOLUME.ToString(), 0));

        m_AudioMixer.SetFloat(VOLUME_SLIDER.SFXVOLUME.ToString(), PlayerPrefs.GetFloat(VOLUME_SLIDER.SFXVOLUME.ToString(), 0));
    }

    private void SaveOptions()
    {
        float value;
        m_AudioMixer.GetFloat(VOLUME_SLIDER.MASTERVOLUME.ToString(), out value);
        PlayerPrefs.SetFloat(VOLUME_SLIDER.MASTERVOLUME.ToString(), value);

        m_AudioMixer.GetFloat(VOLUME_SLIDER.MUSICVOLUME.ToString(), out value);
        PlayerPrefs.SetFloat(VOLUME_SLIDER.MUSICVOLUME.ToString(), value);

        m_AudioMixer.GetFloat(VOLUME_SLIDER.SFXVOLUME.ToString(), out value);
        PlayerPrefs.SetFloat(VOLUME_SLIDER.SFXVOLUME.ToString(), value);
    }

    private void ChangeVolume(VOLUME_SLIDER slider, float value)
    {
        m_AudioMixer.SetFloat(slider.ToString(), value);
    }
}
