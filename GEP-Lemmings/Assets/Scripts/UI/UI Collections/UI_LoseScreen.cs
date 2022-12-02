using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LoseScreen : UI_Abstract
{
    [SerializeField] private Button_UIOnClick m_RestartLevelButton;
    [SerializeField] private Button_UIOnClick m_QuitToTitleButton;

    private void Awake()
    {
        m_RestartLevelButton.OnClicked += ReloadScene;
        m_QuitToTitleButton.OnClicked += QuitToTitle;
    }

    private void OnDestroy()
    {
        m_RestartLevelButton.OnClicked -= ReloadScene;
        m_QuitToTitleButton.OnClicked -= QuitToTitle;
    }
}
