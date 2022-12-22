using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SimpleScreen : UI_Abstract
{
    [SerializeField] private Button_UIOnClick m_BackButton;

    protected virtual void Awake()
    {
        m_BackButton.OnClicked += BackButton;
    }

    protected virtual void OnDestroy()
    {
        m_BackButton.OnClicked -= BackButton;
    }
}
