using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Exit_Object : MonoBehaviour
{
    public event Action<int> onLemmingExit;

    private void OnTriggerEnter(Collider other)
    {
        Lemming_Movement lemming = other.GetComponent<Lemming_Movement>();
        if(lemming)
            onLemmingExit?.Invoke(lemming.m_LemmingID);
    }
}
