using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Pause : UI_Abstract
{
    [SerializeField] private Button_UIOnClick m_ContinueButton;
    [SerializeField] private Button_UIOnClickUI m_ControlsButton;
    [SerializeField] private Button_UIOnClickUI m_SettingsButton;
    [SerializeField] private Button_UIOnClick m_QuitToTitleButton;

    private void Awake()
    {
        m_ContinueButton.OnClicked += LoadNextLevel;
        m_ControlsButton.OnClicked += LoadUI;
        m_SettingsButton.OnClicked += LoadUI;
        m_QuitToTitleButton.OnClicked += QuitToTitle;
    }

    private void OnDestroy()
    {
        m_ContinueButton.OnClicked -= LoadNextLevel;
        m_ControlsButton.OnClicked -= LoadUI;
        m_SettingsButton.OnClicked -= LoadUI;
        m_QuitToTitleButton.OnClicked -= QuitToTitle;
    }
}
