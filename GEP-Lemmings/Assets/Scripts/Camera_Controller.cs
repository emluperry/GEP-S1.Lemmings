using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    private Vector3 m_Direction = new Vector3(0,0,0);

    [SerializeField] private float m_CameraSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        m_Direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
    }

    private void LateUpdate()
    {
        transform.position += m_Direction * m_CameraSpeed * Time.deltaTime;
    }
}
