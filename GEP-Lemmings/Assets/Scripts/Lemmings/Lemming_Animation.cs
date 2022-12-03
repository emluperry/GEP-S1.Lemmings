using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lemming_Animation : MonoBehaviour
{
    private Animator m_Animator;
    private Lemming_Movement m_Movement;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Movement = GetComponent<Lemming_Movement>();

        m_Movement.onWalking += UpdateWalking;
        m_Movement.onFalling += UpdateFalling;
        m_Movement.onFloating += UpdateFloating;
        m_Movement.onDead += TriggerDie;
    }

    private void OnDestroy()
    {
        m_Movement.onWalking -= UpdateWalking;
        m_Movement.onFalling -= UpdateFalling;
        m_Movement.onFloating -= UpdateFloating;
        m_Movement.onDead -= TriggerDie;
    }

    private void UpdateWalking()
    {
        m_Animator.SetBool("isGrounded", true);
    }

    private void UpdateFalling()
    {
        m_Animator.SetBool("isGrounded", false);
    }

    private void UpdateFloating()
    {
        m_Animator.SetBool("isFloating", true);
    }

    private void TriggerDie(int var)
    {
        m_Animator.SetTrigger("die");
    }
}
