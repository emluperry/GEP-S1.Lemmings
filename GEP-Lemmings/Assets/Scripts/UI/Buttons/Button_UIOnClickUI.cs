using System;
using UnityEngine;
using UnityEngine.EventSystems;

using Lemmings.Enums;

public class Button_UIOnClickUI : Button_UIOnClick, IPointerClickHandler
{
    [SerializeField] private UI_STATE m_UIToLoad = UI_STATE.NONE;
    new public event Action<UI_STATE> OnClicked;

    public override void OnPointerClick(PointerEventData eventData)
    {
        OnClicked?.Invoke(m_UIToLoad);
    }
}