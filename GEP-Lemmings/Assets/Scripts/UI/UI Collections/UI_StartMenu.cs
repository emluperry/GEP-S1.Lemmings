using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

using Lemmings.Enums;

public class UI_StartMenu : UI_Abstract
{
    [SerializeField] private Button_UIOnClickUI m_StartButton;
    [SerializeField] private Button_UIOnClickUI m_HowToPlayButton;
    [SerializeField] private Button_UIOnClick m_QuitGameButton;

    private void Awake()
    {
        m_StartButton.OnClicked += LoadUI;
        m_QuitGameButton.OnClicked += Quit;
        m_HowToPlayButton.OnClicked += LoadUI;
    }

    private void OnDestroy()
    {
        m_StartButton.OnClicked -= LoadUI;
        m_QuitGameButton.OnClicked -= Quit;
        m_HowToPlayButton.OnClicked -= LoadUI;
    }
}
