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

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.SphereCast(ray, mousePrecisionRadius, out hit))
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
