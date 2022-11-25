using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum LEMMING_STATE
{
    WALKING,
    FALLING,
    TURNING,
    FLOATING,
    BUILDING,
    BLOCKING,
    EXPLODING
}

public class Lemming_Movement : MonoBehaviour
{
    private Rigidbody m_RB;

    [Header("Lemming Properties")]
    [SerializeField][Min(0f)] private float m_Speed = 5;
    [SerializeField][Min(0f)] private float m_SpeedDecreaseModifier = 0.1f;
    [SerializeField][Min(0f)] private float m_MinimumSpeed = 0.0000000001f;

    public int m_LemmingID = -1;
    private Vector3 m_direction = new Vector3(1, 0, 0);
    private LEMMING_STATE m_state;
    private bool m_hasFallReducedHorizontalVelocity = false;

    private Button_OnClick m_LemmingButton;
    public Action<int> onLemmingClicked;

    //actions
    public Action onWalking;
    public Action onFalling;
    public Action onFloating;

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

    public void SetJobState(int index)
    {
        if (index == -1)
            m_state = LEMMING_STATE.WALKING;
        else
            m_state = (LEMMING_STATE)(index + 3);
    }

    void FixedUpdate()
    {
        switch (m_state)
        {
            case LEMMING_STATE.WALKING:
                Walking();
                if (m_RB.velocity.y < -m_MinimumSpeed)
                {
                    m_state = LEMMING_STATE.FALLING;
                }
                break;

            case LEMMING_STATE.FALLING:
                Falling();
                if (m_RB.velocity.y > -m_MinimumSpeed)
                {
                    m_RB.velocity = new Vector2(0, 0);
                    m_hasFallReducedHorizontalVelocity = false;
                    m_state = LEMMING_STATE.WALKING;
                    onWalking?.Invoke();
                }
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
    }

    private void Falling()
    {
        if (!m_hasFallReducedHorizontalVelocity)
        {
            float newSpeed = m_RB.velocity.x * m_SpeedDecreaseModifier;
            m_RB.velocity = new Vector3(newSpeed, m_RB.velocity.y, 0);
            m_hasFallReducedHorizontalVelocity = true;
            onFalling?.Invoke();
        }
    }

    private void TurnAround()
    {
        m_direction *= -1;
        m_RB.velocity = new Vector2(m_RB.velocity.x * -1, 0);
    }
}
