using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdatableValue : MonoBehaviour
{
    private TextMeshProUGUI m_TMP;

    private void Awake()
    {
        m_TMP = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateValue(int val)
    {
        m_TMP.text = val.ToString();
    }
}
