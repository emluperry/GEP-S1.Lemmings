using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pointer_Handler : MonoBehaviour
{
    //[SerializeField] private float m_MousePrecisionRadius = 1f;

    //public event Action<int> onClickedLemming;

    //private void Update()
    //{
    //    if(Input.GetMouseButtonDown(0))
    //    {
    //        RaycastHit hit;

    //        Vector3 mousePos = Input.mousePosition;
    //        mousePos.z = Camera.main.nearClipPlane;
    //        Ray ray = Camera.main.ScreenPointToRay(mousePos);

    //        if (Physics.SphereCast(ray, m_MousePrecisionRadius, out hit))
    //        {
    //            Lemming_Movement lemming = hit.transform.gameObject.GetComponent<Lemming_Movement>();
    //            if (lemming)
    //            {
    //                onClickedLemming?.Invoke(lemming.LemmingID);
    //            }
    //        }
    //    }
    //}
}
