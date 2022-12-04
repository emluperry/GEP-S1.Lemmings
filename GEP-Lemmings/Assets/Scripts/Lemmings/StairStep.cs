using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StairStep : MonoBehaviour
{
    [SerializeField] private Vector3 direction = Vector3.zero;

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    public Vector3 GetDirection()
    {
        return direction;
    }
}