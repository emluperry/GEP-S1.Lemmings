using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Exit_Object : MonoBehaviour
{
    public event Action<int> onLemmingExit;
    [SerializeField] private GameObject m_ParticleEffect;
    [SerializeField] private float m_EffectDuration = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        Lemming_Movement lemming = other.GetComponent<Lemming_Movement>();
        if(lemming)
        {
            onLemmingExit?.Invoke(lemming.m_LemmingID);
            StartCoroutine(StartConfetti());
        }
    }

    private IEnumerator StartConfetti()
    {
        m_ParticleEffect.SetActive(true);
        yield return new WaitForSeconds(m_EffectDuration);
        m_ParticleEffect.SetActive(false);
    }
}
