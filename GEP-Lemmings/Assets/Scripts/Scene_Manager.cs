using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    private UI_Abstract[] m_ActiveUIObjects;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
        StopListeningForEvents();
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        m_ActiveUIObjects = FindObjectsOfType<UI_Abstract>();

        foreach(UI_Abstract uiObject in m_ActiveUIObjects)
        {
            uiObject.CallLoadScene += LoadScene;
            uiObject.CallQuitApp += QuitApplication;
        }
    }

    private void StopListeningForEvents()
    {
        if (m_ActiveUIObjects.Length <= 0)
            return;

        foreach (UI_Abstract uiObject in m_ActiveUIObjects)
        {
            uiObject.CallLoadScene -= LoadScene;
            uiObject.CallQuitApp -= QuitApplication;
        }
    }

    private void LoadScene(int BuildIndex)
    {
        foreach (UI_Abstract uiObject in m_ActiveUIObjects)
        {
            uiObject.CallLoadScene -= LoadScene;
            uiObject.CallQuitApp -= QuitApplication;
        }

        Array.Clear(m_ActiveUIObjects, 0, m_ActiveUIObjects.Length);
        SceneManager.LoadScene(BuildIndex);
    }

    private void QuitApplication()
    {
        Application.Quit();
    }
}
