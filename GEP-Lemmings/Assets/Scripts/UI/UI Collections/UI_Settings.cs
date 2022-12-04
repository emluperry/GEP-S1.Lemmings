using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_Settings : UI_SimpleScreen
{
    [SerializeField] private AudioMixer m_AudioMixer;

    [Header("SOUND")]
    [SerializeField] private Slider m_MasterVolumeSlider;
    [SerializeField] private Slider m_MusicVolumeSlider;
    [SerializeField] private Slider m_SFXVolumeSlider;

    private void Awake()
    {
        m_MasterVolumeSlider.onValueChanged.AddListener(delegate { ChangeMasterVolume(); });
        m_MusicVolumeSlider.onValueChanged.AddListener(delegate { ChangeMusicVolume(); });
        m_SFXVolumeSlider.onValueChanged.AddListener(delegate { ChangeSFXVolume(); });
    }

    private void OnDestroy()
    {
        m_MasterVolumeSlider.onValueChanged.RemoveAllListeners();
    }

    private void ChangeMasterVolume()
    {
        Debug.Log("Master volume changed");
        m_AudioMixer.SetFloat("MasterVolume", m_MasterVolumeSlider.value);
    }

    private void ChangeMusicVolume()
    {
        m_AudioMixer.SetFloat("MusicVolume", m_MusicVolumeSlider.value);
    }

    private void ChangeSFXVolume()
    {
        m_AudioMixer.SetFloat("SFXVolume", m_SFXVolumeSlider.value);
    }
}
