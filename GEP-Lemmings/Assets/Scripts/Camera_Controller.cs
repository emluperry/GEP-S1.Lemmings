using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    private Vector3 m_Direction = new Vector3(0,0,0);
    private bool m_IsPaused = false;

    [SerializeField] private float m_CameraSpeed = 5f;
    [SerializeField] private float m_MaxDistFromCentre = 60f;

    void Update()
    {
        if(m_IsPaused)
        {
            m_Direction = Vector3.zero;
            return;
        }

        m_Direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
        if(Input.GetKeyDown(KeyCode.F))
        {
            ResetPosition();
        }
    }

    private void LateUpdate()
    {
        transform.position += m_Direction * m_CameraSpeed * Time.deltaTime;
        Vector2 ClampedPosition = Vector2.ClampMagnitude(transform.position, m_MaxDistFromCentre);
        transform.position = (Vector3)ClampedPosition + new Vector3(0, 0, -18);
    }

    public void SetPaused(bool paused)
    {
        m_IsPaused = paused;
    }

    public void ResetPosition()
    {
        transform.position = Vector3.zero;
    }
}
