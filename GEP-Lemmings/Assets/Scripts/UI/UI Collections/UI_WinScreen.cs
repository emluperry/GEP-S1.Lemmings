using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_WinScreen : UI_Abstract
{
    [SerializeField] private Button_UIOnClick m_NextLevelButton;
    [SerializeField] private Button_UIOnClick m_QuitToTitleButton;

    private void Awake()
    {
        m_NextLevelButton.OnClicked += LoadNextLevel;
        m_QuitToTitleButton.OnClicked += QuitToTitle;
    }

    private void OnDestroy()
    {
        m_NextLevelButton.OnClicked -= LoadNextLevel;
        m_QuitToTitleButton.OnClicked -= QuitToTitle;
    }
}
