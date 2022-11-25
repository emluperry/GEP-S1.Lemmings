using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Handler : MonoBehaviour
{
    [SerializeField] private float mousePrecisionRadius = 1f;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if(Physics.SphereCast(Camera.main.ScreenPointToRay(Input.mousePosition), mousePrecisionRadius, out hit))
            {
                Lemming_Movement lemming = hit.transform.gameObject.GetComponent<Lemming_Movement>();
                if (lemming)
                {
                    Debug.Log(lemming.LemmingID);
                }
            }
        }
    }
}
