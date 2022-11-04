using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lemming_Movement : MonoBehaviour
{
    private Rigidbody m_RB;

    [Header("Lemming Properties")]
    [SerializeField][Min(0f)] private float m_Speed = 5;
    [SerializeField][Min(0f)] private float m_SpeedDecreaseModifier = 0.1f;

    private Vector3 m_direction = new Vector3(1, 0, 0);
    private bool m_isFalling = false;
    private bool m_hasFallReducedHorizontalVelocity = false;

    private void Start()
    {
        m_RB = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (m_RB.velocity.y < -Mathf.Epsilon)
        {
            m_isFalling = true;
            if(!m_hasFallReducedHorizontalVelocity)
            {
                float newSpeed = m_RB.velocity.x * m_SpeedDecreaseModifier;
                m_RB.velocity = new Vector3(newSpeed, m_RB.velocity.y, 0);
                m_hasFallReducedHorizontalVelocity = true;
            }
        }

        if (!m_isFalling)
        {
            Vector2 NeededAcceleration = (m_Speed * m_direction - new Vector3(m_RB.velocity.x, 0, 0)) / Time.fixedDeltaTime;

            m_RB.AddForce(NeededAcceleration, ForceMode.Force);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!m_isFalling)
        {
            m_direction *= -1;
            m_RB.velocity = Vector3.zero;
        }
        else
        {
            m_isFalling = false;
            m_hasFallReducedHorizontalVelocity = false;
        }
    }
}
