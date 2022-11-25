using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lemmings.Enums;

public class Lemming_Movement : MonoBehaviour
{
    private Rigidbody m_RB;

    [Header("Lemming Properties")]
    [SerializeField][Min(0f)] private float m_Speed = 5;
    [SerializeField][Min(0f)] private float m_FloatSpeed = 0.5f;
    [SerializeField][Min(0f)] private float m_MinimumFallSpeed = 0.0000000001f;
    [SerializeField][Min(0f)] private float m_CoyoteTime = 0.5f;
    private float m_CurrentCoyoteTime = 0f;
    [SerializeField][Min(0f)] private float m_DeadlyFallTime = 2f;
    private float m_CurrentFallTime = 0f;

    public int m_LemmingID = -1;

    private Vector3 m_direction = new Vector3(1, 0, 0);

    private LEMMING_STATE m_state;
    private LEMMING_JOB m_job = LEMMING_JOB.NONE;

    private bool m_hasFallReducedVelocity = false;

    private Button_OnClick m_LemmingButton;
    public Action<int> onLemmingClicked;

    //actions
    public Action onWalking;
    public Action onFalling;
    public Action onFloating;
    public Action onDead;

    private void Awake()
    {
        m_LemmingButton = GetComponentInChildren<Button_OnClick>();
    }

    private void OnEnable()
    {
        m_LemmingButton.OnClicked += LemmingClicked;
    }

    private void OnDisable()
    {
        m_LemmingButton.OnClicked -= LemmingClicked;
    }

    private void Start()
    {
        m_RB = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject)
        {
            if (m_state != LEMMING_STATE.FALLING)
                m_state = LEMMING_STATE.TURNING;
        }
    }

    private void LemmingClicked()
    {
        onLemmingClicked?.Invoke(m_LemmingID);
    }

    public void SetJobState(LEMMING_JOB job)
    {
        m_job = job;

        if (m_job == LEMMING_JOB.FLOATING)
            onFloating?.Invoke();

        Debug.Log("Current job: " + m_job);
    }

    void FixedUpdate()
    {
        switch (m_state)
        {
            case LEMMING_STATE.WALKING:
                Walking();
                break;

            case LEMMING_STATE.FALLING:
                Falling();
                break;
            case LEMMING_STATE.TURNING:
                TurnAround();
                m_state = LEMMING_STATE.WALKING;
                break;
        }
    }

    private void Walking()
    {
        Vector2 NeededAcceleration = (m_Speed * m_direction - new Vector3(m_RB.velocity.x, 0, 0)) / Time.fixedDeltaTime;

        m_RB.AddForce(NeededAcceleration, ForceMode.Force);

        if (m_RB.velocity.y < -m_MinimumFallSpeed)
        {
            m_CurrentCoyoteTime += Time.fixedDeltaTime;
            if (m_CurrentCoyoteTime >= m_CoyoteTime)
            {
                m_state = LEMMING_STATE.FALLING;
                m_CurrentCoyoteTime = 0;
            }
        }
        else
            m_CurrentCoyoteTime = 0;
    }

    private void Falling()
    {
        if (!m_hasFallReducedVelocity)
        {
            onFalling?.Invoke();

            m_RB.velocity = new Vector3(0, m_RB.velocity.y, 0);

            m_hasFallReducedVelocity = true;
            return;
        }

        if (m_job == LEMMING_JOB.FLOATING)
        {
            Vector2 NeededAcceleration = (-m_FloatSpeed * Vector3.up - new Vector3(0, m_RB.velocity.y, 0)) / Time.fixedDeltaTime;

            m_RB.AddForce(NeededAcceleration, ForceMode.Force);
        }
        else
        {
            m_CurrentFallTime += Time.fixedDeltaTime;
        }

        if (m_RB.velocity.y > -m_MinimumFallSpeed)
        {
            m_RB.velocity = new Vector2(0, 0);
            m_hasFallReducedVelocity = false;

            if(m_CurrentFallTime >= m_DeadlyFallTime)
            {
                onDead?.Invoke();
                m_state = LEMMING_STATE.DEAD;
            }
            else
            {
                m_CurrentFallTime = 0;
                m_state = LEMMING_STATE.WALKING;
                onWalking?.Invoke();
            }
        }
    }

    private void TurnAround()
    {
        m_direction *= -1;
        m_RB.velocity = new Vector2(m_RB.velocity.x * -1, 0);
    }
}
