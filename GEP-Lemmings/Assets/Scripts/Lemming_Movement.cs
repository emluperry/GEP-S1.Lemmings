using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lemming_Movement : MonoBehaviour
{
    private Rigidbody m_RB;

    [SerializeField] private float m_Speed = 5;
    [SerializeField] private float m_MaxSpeed = 50;

    private Vector3 m_direction = new Vector3(1, 0, 0);

    private void Start()
    {
        m_RB = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        m_RB.AddForce(m_direction * (m_Speed * Time.deltaTime), ForceMode.Force);

        //f = m*a
        //a = v/t
        //v = s/t
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_direction *= -1;
        
    }
}
