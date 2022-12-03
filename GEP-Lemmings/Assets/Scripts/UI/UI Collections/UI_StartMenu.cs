using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

using Lemmings.Enums;

public class UI_StartMenu : UI_Abstract
{
    [SerializeField] private Button_UIOnClickLevel m_StartButton;
    [SerializeField] private Button_UIOnClick m_HowToPlayButton;
    [SerializeField] private Button_UIOnClick m_QuitGameButton;

    private void Awake()
    {
        m_StartButton.OnClicked += LoadLevel;
        m_QuitGameButton.OnClicked += Quit;
        m_HowToPlayButton.OnClicked += HowTo;
    }

    private void OnDestroy()
    {
        m_StartButton.OnClicked -= LoadLevel;
        m_QuitGameButton.OnClicked -= Quit;
        m_HowToPlayButton.OnClicked -= HowTo;
    }

    private void HowTo()
    {
        CallLoadUI?.Invoke(UI_STATE.HOWTOPLAY);
    }
}
