using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_UIOnClickLevel : Button_UIOnClick, IPointerClickHandler
{
    [SerializeField][Min(0f)] private int m_SceneIndexToLoad = 0;
    new public event Action<int> OnClicked;

    public override void OnPointerClick(PointerEventData eventData)
    {
        OnClicked?.Invoke(m_SceneIndexToLoad);
    }
}