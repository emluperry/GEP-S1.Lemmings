using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lemming_SFX : MonoBehaviour
{
    [SerializeField] private AudioClip m_FootstepClip;
    [SerializeField] private AudioClip m_ExplodeClip;
    [SerializeField] private AudioClip m_DeathClip;
    [SerializeField] private AudioClip m_BlockPlaceClip;

    private Lemming_Movement m_MovementComponent;
    private AudioSource m_AudioSourceComponent;

    private void Awake()
    {
        m_AudioSourceComponent = GetComponent<AudioSource>();
        m_MovementComponent = GetComponent<Lemming_Movement>();

        m_MovementComponent.onDead += PlayDeath;
        m_MovementComponent.onExplode += PlayExplosion;
        m_MovementComponent.onBlockPlaced += PlayBlockPlace;
        //block place event
    }

    private void OnDestroy()
    {
        m_MovementComponent.onDead -= PlayDeath;
        m_MovementComponent.onExplode -= PlayExplosion;
        m_MovementComponent.onBlockPlaced -= PlayBlockPlace;
    }

    private void PlayFootstep() //called by animator
    {
        m_AudioSourceComponent.clip = m_FootstepClip;
        m_AudioSourceComponent.Play();
    }

    private void PlayExplosion(Vector3 explosion)
    {
        m_AudioSourceComponent.Stop();
        m_AudioSourceComponent.clip = m_ExplodeClip;
        m_AudioSourceComponent.Play();
    }

    private void PlayDeath()
    {
        m_AudioSourceComponent.Stop();
        m_AudioSourceComponent.clip = m_DeathClip;
        m_AudioSourceComponent.Play();
    }

    private void PlayBlockPlace()
    {
        m_AudioSourceComponent.Stop();
        m_AudioSourceComponent.clip = m_BlockPlaceClip;
        m_AudioSourceComponent.Play();
    }
}
