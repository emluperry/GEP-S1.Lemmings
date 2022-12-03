using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LevelSelect : UI_SimpleScreen
{
    [SerializeField] private FlexibleGridLayout m_LayoutGroup;
    private Button_UIOnClickLevel[] m_LevelButtons;

    private void Awake()
    {
        m_LevelButtons = GetComponentsInChildren<Button_UIOnClickLevel>();
        foreach(Button_UIOnClickLevel button in m_LevelButtons)
        {
            button.OnClicked += LoadLevel;
        }
    }

    private void OnDestroy()
    {
        foreach (Button_UIOnClickLevel button in m_LevelButtons)
        {
            button.OnClicked -= LoadLevel;
        }
    }
}
