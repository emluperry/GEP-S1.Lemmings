using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Lemmings.Enums;

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
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        m_ActiveUIObjects = FindObjectsOfType<UI_Abstract>();

        foreach(UI_Abstract uiObject in m_ActiveUIObjects)
        {
            uiObject.CallLoadScene += LoadScene;
            uiObject.CallQuitApp += QuitApplication;
            uiObject.CallLoadNextScene += LoadNextLevel;
            uiObject.CallReloadScene += RestartLevel;
            //uiObject.CallLoadUI += LoadUI;
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
            uiObject.CallLoadNextScene -= LoadNextLevel;
            uiObject.CallReloadScene -= RestartLevel;
            //uiObject.CallLoadUI -= LoadUI;
        }

        Array.Clear(m_ActiveUIObjects, 0, m_ActiveUIObjects.Length);
    }

    private void LoadScene(int BuildIndex)
    {
        SceneManager.LoadScene(BuildIndex);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void QuitApplication()
    {
        Application.Quit();
    }
}
