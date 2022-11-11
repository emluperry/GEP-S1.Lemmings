using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Level Properties")]
    [SerializeField] private GameObject m_LemmingObject;
    [SerializeField][Min(0f)] private int m_MaxLemmings = 10;
    private List<GameObject> m_ArrLemmings = new List<GameObject>();
    [SerializeField][Min(0.01f)] private float m_LemmingSpawnDelay = 0.5f;
    private float m_CurrentInterval = 0;
    [SerializeField] private GameObject m_LevelSpawnPoint;
    [SerializeField] private GameObject m_LevelEndPoint;

    private void Update()
    {
        m_CurrentInterval += Time.deltaTime;
        if(m_CurrentInterval >= m_LemmingSpawnDelay && m_ArrLemmings.Count < m_MaxLemmings)
        {
            GameObject Lemming = Instantiate(m_LemmingObject, m_LevelSpawnPoint.transform.position, Quaternion.identity, gameObject.transform);
            m_ArrLemmings.Add(Lemming);
            m_CurrentInterval = 0;
        }
    }
}
