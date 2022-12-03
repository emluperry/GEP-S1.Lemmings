using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Lemmings.Enums;

public class Scene_Manager : MonoBehaviour
{
    [SerializeField] private GameObject m_HowToPlayPrefab;
    [SerializeField] private GameObject m_SettingsPrefab;
    [SerializeField] private GameObject m_PausePrefab;
    [SerializeField] private GameObject m_WinPrefab;
    [SerializeField] private GameObject m_LosePrefab;

    private UI_Abstract m_HowToPlay;
    private UI_Abstract m_Settings;
    private UI_Abstract m_Pause;
    private UI_Abstract m_Win;
    private UI_Abstract m_Lose;

    private Stack<UI_Abstract> m_UIStack;

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
            uiObject.CallLoadUI += LoadUI;
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
            uiObject.CallLoadUI -= LoadUI;
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

    private void LoadUI(UI_STATE UIScreen)
    {
        UI_Abstract LastUI;
        m_UIStack.TryPeek(out LastUI);
        if (LastUI)
            LastUI.gameObject.SetActive(false);

        switch(UIScreen)
        {
            case UI_STATE.NONE:
                m_UIStack.Clear();
                break;

            case UI_STATE.PAUSED:
                if (!m_Pause)
                    m_Pause = Instantiate(m_PausePrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<UI_Abstract>();
                else
                    m_Pause.gameObject.SetActive(true);
                m_UIStack.Push(m_Pause);
                break;

            case UI_STATE.SETTINGS:
                if (!m_Settings)
                    m_Settings = Instantiate(m_SettingsPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<UI_Abstract>();
                else
                    m_Settings.gameObject.SetActive(true);
                m_UIStack.Push(m_Settings);
                break;

            case UI_STATE.HOWTOPLAY:
                if (!m_HowToPlay)
                    m_HowToPlay = Instantiate(m_HowToPlayPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<UI_Abstract>();
                else
                    m_HowToPlay.gameObject.SetActive(true);
                m_UIStack.Push(m_HowToPlay);
                break;

            case UI_STATE.WIN:
                if (!m_Win)
                    m_Win = Instantiate(m_WinPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<UI_Abstract>();
                else
                    m_Win.gameObject.SetActive(true);
                m_UIStack.Push(m_Win);
                break;

            case UI_STATE.LOSE:
                if (!m_Lose)
                    m_Lose = Instantiate(m_LosePrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<UI_Abstract>();
                else
                    m_Lose.gameObject.SetActive(true);
                m_UIStack.Push(m_Lose);
                break;

            case UI_STATE.BACK:
                m_UIStack.Pop();
                m_UIStack.TryPeek(out LastUI);
                if (LastUI)
                    LastUI.gameObject.SetActive(true);
                break;
        }
    }
}
