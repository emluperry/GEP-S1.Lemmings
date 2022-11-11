using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Level Properties")]
    [SerializeField] private GameObject m_LemmingObject;
    [SerializeField][Min(0f)] private int m_MaxLemmings = 10;
    private GameObject[] m_ArrLemmings;
    private int m_LastActiveLemming = 0;
    [SerializeField][Min(0.01f)] private float m_LemmingSpawnDelay = 0.5f;
    private float m_CurrentInterval = 0;
    [SerializeField] private GameObject m_LevelSpawnPoint;
    [SerializeField] private GameObject m_LevelEndPoint;

    private void Start()
    {
        m_ArrLemmings = new GameObject[m_MaxLemmings];
        for(int index = 0; index < m_MaxLemmings; index++)
        {
            m_ArrLemmings[index] = Instantiate(m_LemmingObject, m_LevelSpawnPoint.transform.position, Quaternion.identity, gameObject.transform);
            m_ArrLemmings[index].SetActive(false);
        }
        m_CurrentInterval = m_LemmingSpawnDelay;
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
}
