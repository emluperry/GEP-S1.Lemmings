using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lemming_Movement : MonoBehaviour
{
    private Rigidbody m_RB;

    [Header("Lemming Properties")]
    [SerializeField] private float m_Speed = 5;

    private Vector3 m_direction = new Vector3(1, 0, 0);
    private bool m_isFalling = false;

    private void Start()
    {
        m_RB = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(!m_isFalling)
        {
            Vector2 NeededAcceleration = (m_Speed * m_direction - new Vector3(m_RB.velocity.x, 0, 0)) / Time.fixedDeltaTime;

            m_RB.AddForce(NeededAcceleration, ForceMode.Force);
        }
        if(m_RB.velocity.y < 0)
        {
            m_isFalling = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!m_isFalling)
        {
            m_direction *= -1;
        }
        else
        {
            m_isFalling = false;
        }
    }
}
