using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject m_LemmingObject;

    [Header("Level Properties")]
    [SerializeField][Min(0f)] private int m_MaxLemmings = 10;
    private GameObject[] m_ArrLemmings;
    private int m_LastActiveLemming = 0;

    [SerializeField][Min(0.01f)] private float m_LemmingSpawnDelay = 0.5f;
    private float m_CurrentInterval = 0;

    [Header("In-Scene References")]
    [SerializeField] private GameObject m_LevelSpawnPoint;
    [SerializeField] private Exit_Object m_LevelEndPoint;

    [Header("UI References")]
    [SerializeField] private UIManager m_UIHandler;

    private void Awake()
    {
        m_LevelEndPoint.onLemmingExit += DeactivateLemming;

        m_ArrLemmings = new GameObject[m_MaxLemmings];
        for (int index = 0; index < m_MaxLemmings; index++)
        {
            m_ArrLemmings[index] = Instantiate(m_LemmingObject, m_LevelSpawnPoint.transform.position, Quaternion.identity, gameObject.transform);
            m_ArrLemmings[index].SetActive(false);
            m_ArrLemmings[index].GetComponent<Lemming_Movement>().LemmingID = index;
        }
        m_CurrentInterval = m_LemmingSpawnDelay;
    }

    private void OnDestroy()
    {
        m_LevelEndPoint.onLemmingExit -= DeactivateLemming;
    }

    private void Update()
    {
        m_CurrentInterval += Time.deltaTime;
        if(m_CurrentInterval >= m_LemmingSpawnDelay && m_LastActiveLemming < m_MaxLemmings)
        {
            m_ArrLemmings[m_LastActiveLemming].SetActive(true);
            m_LastActiveLemming++;
            m_CurrentInterval = 0;
        }
    }

    private void DeactivateLemming(int LemmingIndex)
    {
        m_ArrLemmings[LemmingIndex].SetActive(false);
        //increase number of successful lemmings
    }
}
