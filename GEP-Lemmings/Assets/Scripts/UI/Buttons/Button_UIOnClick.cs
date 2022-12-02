using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_UIOnClick : Button_OnClick, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Image m_Image;

    private Color m_OriginalColour;
    [SerializeField] private Color m_HoverColour = Color.white;

    private void Start()
    {
        m_Image = GetComponent<Image>();
        m_OriginalColour = m_Image.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_Image.color = m_HoverColour;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_Image.color = m_OriginalColour;
    }
}
