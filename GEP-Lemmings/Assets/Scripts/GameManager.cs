using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Lemmings.Enums;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject m_LemmingObject;

    [Header("Level Properties")]
    [SerializeField][Min(0f)] private int m_MaxLemmings = 10;
    private GameObject[] m_ArrLemmings;
    private int m_LastActiveLemming = 0;
    private int m_CurrentLivingLemmingNum = 0;

    [SerializeField][Min(0.01f)] private float m_LemmingSpawnDelay = 0.5f;
    private float m_CurrentInterval = 0;

    [SerializeField][Min(1)] private int m_WinNum = 5;
    private int m_CurrentNumIn = 0;

    [SerializeField][Min(0)] private float m_MaximumTimeLimitInSeconds = 300f;

    private bool m_IsPaused = false;
    public Action<bool> onPausePressed;

    [Header("Role counts")]
    [SerializeField][Min(-1)] private int m_MaxNumFloat = 0;
    [SerializeField][Min(-1)] private int m_MaxNumBlock = 0;
    [SerializeField][Min(-1)] private int m_MaxNumBuild = 0;
    [SerializeField][Min(-1)] private int m_MaxNumExplode = 0;

    [Header("VFX")]
    [SerializeField] private GameObject m_ExplosionPrefab;
    private GameObject m_ExplosionObject;
    [SerializeField] private float m_ExplosionLength = 1f;

    [Header("In-Scene References")]
    [SerializeField] private GameObject m_LevelSpawnPoint;
    [SerializeField] private Exit_Object m_LevelEndPoint;

    [Header("Mouse References")]
    private LEMMING_JOB m_CurrentJob = LEMMING_JOB.NONE;

    [Header("UI References")]
    [SerializeField] private UI_HUD m_HUDButtons;

    private void Awake()
    {
        //events
        m_LevelEndPoint.onLemmingExit += LemmingExitStage;
        m_HUDButtons.onRoleChosen += UpdateJobCast;

        //lemmings
        m_CurrentLivingLemmingNum = m_MaxLemmings;
        m_ArrLemmings = new GameObject[m_MaxLemmings];
        for (int index = 0; index < m_MaxLemmings; index++)
        {
            m_ArrLemmings[index] = Instantiate(m_LemmingObject, m_LevelSpawnPoint.transform.position, Quaternion.identity, gameObject.transform);
            m_ArrLemmings[index].SetActive(false);
            Lemming_Movement movComp = m_ArrLemmings[index].GetComponent<Lemming_Movement>();
            movComp.m_LemmingID = index;
            movComp.onLemmingClicked += SetLemmingJob;
            movComp.onExplode += ExplodeEffect;
            movComp.onDead += KillLemming;
            movComp.onDeactivate += DeactivateLemming;
        }
        m_CurrentInterval = m_LemmingSpawnDelay;

        m_ExplosionObject = Instantiate(m_ExplosionPrefab, transform);
        m_ExplosionObject.SetActive(false);
    }

    private void Start()
    {
        //roles
        m_HUDButtons.UpdateJob(LEMMING_JOB.FLOATING, m_MaxNumFloat);
        m_HUDButtons.UpdateJob(LEMMING_JOB.BUILDING, m_MaxNumBuild);
        m_HUDButtons.UpdateJob(LEMMING_JOB.BLOCKING, m_MaxNumBlock);
        m_HUDButtons.UpdateJob(LEMMING_JOB.EXPLODING, m_MaxNumExplode);

        //other hud stats
        m_HUDButtons.SetTotalLemmingsNeeded(m_WinNum);
        m_HUDButtons.UpdateActiveNumLemmings(m_CurrentLivingLemmingNum);

        StartCoroutine(Timer());
    }

    private void OnDestroy()
    {
        m_LevelEndPoint.onLemmingExit -= LemmingExitStage;
        m_HUDButtons.onRoleChosen -= UpdateJobCast;

        for (int index = 0; index < m_MaxLemmings; index++)
        {
            DeactivateLemming(index);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_IsPaused = !m_IsPaused;
            onPausePressed?.Invoke(m_IsPaused);
            foreach (GameObject Lemming in m_ArrLemmings)
            {
                Lemming.GetComponent<Lemming_Movement>().SetPaused(m_IsPaused);
            }
        }

        if (m_IsPaused)
            return;

        m_CurrentInterval += Time.deltaTime;
        if(m_CurrentInterval >= m_LemmingSpawnDelay && m_LastActiveLemming < m_MaxLemmings)
        {
            m_ArrLemmings[m_LastActiveLemming].SetActive(true);
            m_LastActiveLemming++;
            m_CurrentInterval = 0;
        }
    }

    private IEnumerator Timer()
    {
        int minutes = (int)(m_MaximumTimeLimitInSeconds / 60);
        int seconds = (int)(m_MaximumTimeLimitInSeconds % 60);
        m_HUDButtons.UpdateMinutes(minutes);
        m_HUDButtons.UpdateSeconds(seconds);

        float secs = 0f;
        while (!(minutes == 0 && seconds == 0))
        {
            do
            {
                yield return new WaitForFixedUpdate();
            } while (m_IsPaused);
            
            secs += Time.fixedDeltaTime;

            if(secs > 1)
            {
                seconds -= 1;
                secs -= 1;
                if(seconds < 0)
                {
                    seconds += 60;
                    minutes -= 1;
                    m_HUDButtons.UpdateMinutes(minutes);
                }
                m_HUDButtons.UpdateSeconds(seconds);
            }
        }
    }

    private void LemmingExitStage(int LemmingIndex)
    {
        m_CurrentNumIn++;
        m_HUDButtons.UpdateWinningNumLemmings(m_CurrentNumIn);
        m_HUDButtons.UpdateActiveNumLemmings(m_CurrentLivingLemmingNum - m_CurrentNumIn);

        if (m_CurrentNumIn >= m_WinNum)
        {
            Debug.Log("Win game!");
        }

        DeactivateLemming(LemmingIndex);
    }

    private void KillLemming(int LemmingIndex)
    {
        m_CurrentLivingLemmingNum--;
        m_HUDButtons.UpdateActiveNumLemmings(m_CurrentLivingLemmingNum);

        if (m_CurrentLivingLemmingNum < m_WinNum)
        {
            Debug.Log("Too many dead. Lose game.");
        }
    }

    private void DeactivateLemming(int LemmingIndex)
    {
        m_ArrLemmings[LemmingIndex].SetActive(false);

        Lemming_Movement movComp = m_ArrLemmings[LemmingIndex].GetComponent<Lemming_Movement>();
        movComp.onLemmingClicked -= SetLemmingJob;
        movComp.onExplode -= ExplodeEffect;
        movComp.onDead -= KillLemming;
        movComp.onDeactivate -= DeactivateLemming;
    }

    private void SetLemmingJob(int LemmingIndex)
    {
        switch (m_CurrentJob)
        {
            case LEMMING_JOB.FLOATING:
                if (m_MaxNumFloat <= 0)
                    return;
                break;
            case LEMMING_JOB.BLOCKING:
                if (m_MaxNumBlock <= 0)
                    return;
                break;
            case LEMMING_JOB.BUILDING:
                if (m_MaxNumBuild <= 0)
                    return;
                break;
            case LEMMING_JOB.EXPLODING:
                if (m_MaxNumExplode <= 0)
                    return;
                break;
        }

        bool wasSet = m_ArrLemmings[LemmingIndex].GetComponent<Lemming_Movement>().SetJobState(m_CurrentJob);

        if (!wasSet)
            return;

        switch (m_CurrentJob)
        {
            case LEMMING_JOB.FLOATING:
                m_MaxNumFloat--;
                m_HUDButtons.UpdateJob(m_CurrentJob, m_MaxNumFloat);
                break;
            case LEMMING_JOB.BLOCKING:
                m_MaxNumBlock--;
                m_HUDButtons.UpdateJob(m_CurrentJob, m_MaxNumBlock);
                break;
            case LEMMING_JOB.BUILDING:
                m_MaxNumBuild--;
                m_HUDButtons.UpdateJob(m_CurrentJob, m_MaxNumBuild);
                break;
            case LEMMING_JOB.EXPLODING:
                m_MaxNumExplode--;
                m_HUDButtons.UpdateJob(m_CurrentJob, m_MaxNumExplode);
                break;
        }
    }

    private void UpdateJobCast(LEMMING_JOB job)
    {
        m_CurrentJob = job;
    }

    private void ExplodeEffect(Vector3 position)
    {
        m_ExplosionObject.transform.position = position + new Vector3(0, 0, -3);
        m_ExplosionObject.SetActive(true);

        StartCoroutine(CountdownExplosion());
    }

    private IEnumerator CountdownExplosion()
    {
        yield return new WaitForSeconds(m_ExplosionLength);
        do
        {
            yield return new WaitForFixedUpdate();
        } while (m_IsPaused);
        m_ExplosionObject.SetActive(false);
    }
}
