using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    public enum FitType
    {
        UNIFORM,
        WIDTH,
        HEIGHT,
        FIXEDROWS,
        FIXEDCOLUMNS
    }

    public FitType m_FitType;

    public int m_Rows, m_Columns;
    public Vector2 m_CellSize, m_Spacing;
    public bool m_FitX, m_FitY;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        
        if(m_FitType == FitType.WIDTH || m_FitType == FitType.HEIGHT || m_FitType == FitType.UNIFORM)
        {
            m_FitX = true;
            m_FitY = true;

            float sqrRt = Mathf.Sqrt(transform.childCount);
            m_Rows = Mathf.CeilToInt(sqrRt);
            m_Columns = Mathf.CeilToInt(sqrRt);
        }

        if(m_FitType == FitType.WIDTH || m_FitType == FitType.FIXEDCOLUMNS)
        {
            m_Rows = Mathf.CeilToInt(transform.childCount / (float)m_Columns);
        }

        if(m_FitType == FitType.HEIGHT || m_FitType == FitType.FIXEDROWS)
        {
            m_Columns = Mathf.CeilToInt(transform.childCount / (float)m_Rows);
        }

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = (parentWidth / (float)m_Columns) - m_Spacing.x - (padding.left / (float)m_Columns) - (padding.right / (float)m_Columns);
        float cellHeight = (parentHeight / (float)m_Rows) - m_Spacing.y - (padding.top / (float)m_Rows) - (padding.bottom / (float)m_Rows);

        m_CellSize.x = m_FitX ? cellWidth : m_CellSize.x;
        m_CellSize.y = m_FitY ? cellHeight : m_CellSize.y;

        int colCount, rowCount = 0;
        for(int ItChild = 0; ItChild < rectTransform.childCount; ItChild++)
        {
            rowCount = ItChild / m_Columns;
            colCount = ItChild % m_Columns;

            RectTransform item = rectChildren[ItChild];

            float xPos = (m_CellSize.x * colCount) + (m_Spacing.x * colCount) + padding.left + (m_Spacing.x * 0.5f);
            float yPos = (m_CellSize.y * rowCount) + (m_Spacing.y * rowCount) + padding.top + (m_Spacing.y * 0.5f);

            SetChildAlongAxis(item, 0, xPos, m_CellSize.x);
            SetChildAlongAxis(item, 1, yPos, m_CellSize.y);
        }
    }

    #region not used
    public override void CalculateLayoutInputVertical()
    {
    }

    public override void SetLayoutHorizontal()
    {
    }

    public override void SetLayoutVertical()
    {
    }
    #endregion
}
