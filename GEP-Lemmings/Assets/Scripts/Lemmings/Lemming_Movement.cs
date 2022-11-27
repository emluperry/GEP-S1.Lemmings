using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lemmings.Enums;
using UnityEditor.Build.Reporting;
using Unity.VisualScripting;

public class Lemming_Movement : MonoBehaviour
{
    private Rigidbody m_RB;

    [Header("Lemming Properties")]
    [SerializeField][Min(0f)] private float m_Speed = 5;
    [SerializeField][Min(0f)] private float m_MinimumFallSpeed = 0.0000000001f;
    [SerializeField][Min(0f)] private float m_CoyoteTime = 0.5f;
    private float m_CurrentCoyoteTime = 0f;
    [SerializeField][Min(0f)] private float m_DeadlyFallTime = 2f;
    private float m_CurrentFallTime = 0f;

    [Header("Alternate Materials")]
    [SerializeField] private MeshRenderer m_MeshRenderer;
    [SerializeField] private Material m_StandardMat;
    [SerializeField] private Material m_BlockingMat;

    [Header("Floating")]
    [SerializeField][Min(0f)] private float m_FloatSpeed = 0.5f;

    [Header("Building")]
    [SerializeField] private GameObject m_BrickObject;
    [SerializeField] private int m_maxSteps = 5;
    [SerializeField] private float m_BuildDelay = 1f;
    [SerializeField] private float m_ClimbSpeed = 2f;
    [SerializeField] private float m_StepJumpPushback = 0.25f;
    private int m_numStepsPlaced = 0;
    private bool m_NeedsStepBoost = false;

    [Header("Exploding")]
    [SerializeField] private float m_ExplosionCountdown = 5f;
    private float m_CurrentCountdown = 5f;
    [SerializeField] private float m_ExplosionRadius = 2f;
    [SerializeField] private LayerMask m_ExplosionLayerMask;

    [Header("Other")]
    public int m_LemmingID = -1;

    private Vector3 m_direction = new Vector3(1, 0, 0);

    private LEMMING_STATE m_state;
    private LEMMING_JOB m_job = LEMMING_JOB.NONE;

    private Coroutine m_JobCoroutine;

    private bool m_hasFallReducedVelocity = false;

    private Button_OnClick m_LemmingButton;
    public Action<int> onLemmingClicked;

    public Action<int> onDeactivate;

    //actions
    public Action onWalking;
    public Action onFalling;
    public Action onFloating;
    public Action<int> onDead;
    public Action<Vector3> onExplode;

    private void Awake()
    {
        m_LemmingButton = GetComponentInChildren<Button_OnClick>();
        m_JobCoroutine = null;
    }

    private void OnEnable()
    {
        m_LemmingButton.OnClicked += LemmingClicked;
    }

    private void OnDisable()
    {
        DeactivateLemming();
    }

    private void DeactivateLemming()
    {
        m_LemmingButton.OnClicked -= LemmingClicked;
        onDeactivate?.Invoke(m_LemmingID);
    }

    private void Start()
    {
        m_RB = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 8 && m_direction == collision.gameObject.GetComponent<StairStep>().GetDirection()) //step layer
        {
            m_NeedsStepBoost = true;
        }
        else if (collision.gameObject.layer == 10) //death collision layer
        {
            KillLemming();
        }
        else
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
        if (job == m_job || m_state == LEMMING_STATE.DEAD)
            return;

        m_job = job;

        if (m_job == LEMMING_JOB.FLOATING)
            onFloating?.Invoke();

        if(m_job == LEMMING_JOB.BLOCKING)
        {
            gameObject.layer = 7; //Wall-Lemming layer
            m_RB.isKinematic = true;
            m_MeshRenderer.material = m_BlockingMat;
        }
        else
        {
            gameObject.layer = 6; //Lemming layer
            m_RB.isKinematic = false;
            m_MeshRenderer.material = m_StandardMat;
        }

        if(m_job == LEMMING_JOB.BUILDING)
        {
            if (m_JobCoroutine != null)
                StopCoroutine(m_JobCoroutine);
            m_JobCoroutine = StartCoroutine(BuildStairs());
        }
        else if (m_job == LEMMING_JOB.EXPLODING)
        {
            if (m_JobCoroutine != null)
                StopCoroutine(m_JobCoroutine);
            m_JobCoroutine = StartCoroutine(Explode());
        }
        else
        {
            if (m_JobCoroutine != null)
            {
                StopCoroutine(m_JobCoroutine);
                m_CurrentCountdown = m_ExplosionCountdown;
            }
        }
    }

    void FixedUpdate()
    {
        if (!(m_job == LEMMING_JOB.FLOATING || m_job == LEMMING_JOB.NONE))
            return;

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

        if (m_NeedsStepBoost)
        {
            m_RB.MovePosition(new Vector3(transform.position.x - (m_StepJumpPushback * m_direction.x), transform.position.y, 0));
            ClimbStep();
            m_NeedsStepBoost = false;
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
                KillLemming();
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

    private IEnumerator BuildStairs()
    {
        GameObject step1;
        GameObject step2;

        step1 = Instantiate(m_BrickObject, GetNewBrickPosition(), Quaternion.identity);
        step1.GetComponent<StairStep>().SetDirection(m_direction);
        m_numStepsPlaced++;
        ClimbStep();
        step2 = step1;
        
        do
        {
            yield return new WaitForSeconds(m_BuildDelay);
            step1 = Instantiate(m_BrickObject, GetNewBrickPosition(), Quaternion.identity, step2.transform);
            step1.GetComponent<StairStep>().SetDirection(m_direction);
            step2.GetComponent<StairStep>().SetNextStep(step1);
            m_numStepsPlaced++;
            ClimbStep();
            step2 = step1;
        } while (m_numStepsPlaced <= m_maxSteps);

        m_job = LEMMING_JOB.NONE;
    }

    private Vector3 GetNewBrickPosition()
    {
        return transform.position + m_direction - new Vector3(0, 0.75f, 0);
    }

    private void ClimbStep()
    {
        m_RB.velocity = new Vector3(0, m_RB.velocity.y, 0);
        Vector2 NeededAcceleration = (m_ClimbSpeed * (1.5f * Vector3.up + m_direction) - new Vector3(0, m_RB.velocity.y, 0)) / Time.fixedDeltaTime;

        m_RB.AddForce(NeededAcceleration, ForceMode.Force);
    }

    private IEnumerator Explode()
    {
        do
        {
            yield return new WaitForFixedUpdate();
            m_CurrentCountdown -= Time.fixedDeltaTime;
        } while (m_CurrentCountdown > 0);

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, m_ExplosionRadius, Vector3.forward, 1, m_ExplosionLayerMask);
        foreach(RaycastHit hit in hits)
        {
            hit.collider.gameObject.SetActive(false);
        }
        onExplode?.Invoke(transform.position);
        KillLemming();
    }

    private void KillLemming()
    {
        onDead?.Invoke(m_LemmingID);
        m_state = LEMMING_STATE.DEAD;
    }
}
