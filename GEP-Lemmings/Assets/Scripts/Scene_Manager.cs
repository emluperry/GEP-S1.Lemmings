using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Lemmings.Enums;
using TreeEditor;

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

    private GameManager m_CurrentGameManager;

    private Stack<UI_Abstract> m_UIStack;

    private List<UI_Abstract> m_ActiveUIObjects;

    private void Awake()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
        m_UIStack = new Stack<UI_Abstract>(); 
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        m_UIStack.Clear();

        SceneManager.SetActiveScene(scene);
        m_ActiveUIObjects = new List<UI_Abstract>();
        m_ActiveUIObjects.AddRange(FindObjectsOfType<UI_Abstract>());

        foreach (UI_Abstract uiObject in m_ActiveUIObjects)
        {
            ListenForEventsIn(uiObject);

            m_UIStack.Push(uiObject);
        }

        m_CurrentGameManager = FindObjectOfType<GameManager>();
        if (m_CurrentGameManager)
            m_CurrentGameManager.onPausePressed += LoadPauseMenu;
    }

    private void ListenForEventsIn(UI_Abstract uiObject)
    {
        uiObject.CallLoadScene += LoadScene;
        uiObject.CallQuitApp += QuitApplication;
        uiObject.CallLoadNextScene += LoadNextLevel;
        uiObject.CallReloadScene += RestartLevel;
        uiObject.CallLoadUI += LoadUI;
    }

    private void StopListeningForEvents()
    {
        if (m_ActiveUIObjects.Count <= 0)
            return;

        foreach (UI_Abstract uiObject in m_ActiveUIObjects)
        {
            uiObject.CallLoadScene -= LoadScene;
            uiObject.CallQuitApp -= QuitApplication;
            uiObject.CallLoadNextScene -= LoadNextLevel;
            uiObject.CallReloadScene -= RestartLevel;
            uiObject.CallLoadUI -= LoadUI;
        }

        m_ActiveUIObjects.Clear();
    }

    private void LoadScene(int BuildIndex)
    {
        StopListeningForEvents();

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadSceneAsync(BuildIndex, LoadSceneMode.Additive);
    }

    private void RestartLevel()
    {
        int BuildIndex = SceneManager.GetActiveScene().buildIndex;
        LoadScene(BuildIndex);
    }

    private void LoadNextLevel()
    {
        int BuildIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if(BuildIndex > SceneManager.sceneCountInBuildSettings)
        {
            BuildIndex = 1;
        }
        LoadScene(BuildIndex);
    }

    private void QuitApplication()
    {
        Application.Quit();
    }

    private void LoadPauseMenu(bool paused)
    {
        if(paused)
        {
            LoadUI(UI_STATE.PAUSED);
        }
        else
        {
            UI_Abstract uiObject;
            while(m_UIStack.TryPeek(out uiObject) && !uiObject.GetComponent<UI_Pause>())
            {
                LoadUI(UI_STATE.BACK);
            }
            if (uiObject && uiObject.GetComponent<UI_Pause>())
                LoadUI(UI_STATE.BACK);
        }
    }

    private void LoadUI(UI_STATE UIScreen)
    {
        UI_Abstract LastUI;
        if(m_UIStack.TryPeek(out LastUI))
            LastUI.gameObject.SetActive(false);

        switch(UIScreen)
        {
            case UI_STATE.NONE:
                m_UIStack.Clear();
                break;

            case UI_STATE.PAUSED:
                if (!m_Pause)
                {
                    m_Pause = Instantiate(m_PausePrefab, Vector3.zero, Quaternion.identity).GetComponent<UI_Abstract>();
                    m_ActiveUIObjects.Add(m_Pause);
                    ListenForEventsIn(m_Pause);
                }
                else
                    m_Pause.gameObject.SetActive(true);
                m_UIStack.Push(m_Pause);
                break;

            case UI_STATE.SETTINGS:
                if (!m_Settings)
                {
                    m_Settings = Instantiate(m_SettingsPrefab, Vector3.zero, Quaternion.identity).GetComponent<UI_Abstract>();
                    m_ActiveUIObjects.Add(m_Settings);
                    ListenForEventsIn(m_Settings);
                }
                else
                    m_Settings.gameObject.SetActive(true);
                m_UIStack.Push(m_Settings);
                break;

            case UI_STATE.HOWTOPLAY:
                if (!m_HowToPlay)
                {
                    m_HowToPlay = Instantiate(m_HowToPlayPrefab, Vector3.zero, Quaternion.identity).GetComponent<UI_Abstract>();
                    m_ActiveUIObjects.Add(m_HowToPlay);
                    ListenForEventsIn(m_HowToPlay);
                }
                else
                    m_HowToPlay.gameObject.SetActive(true);
                m_UIStack.Push(m_HowToPlay);
                break;

            case UI_STATE.WIN:
                if (!m_Win)
                {
                    m_Win = Instantiate(m_WinPrefab, Vector3.zero, Quaternion.identity).GetComponent<UI_Abstract>();
                    m_ActiveUIObjects.Add(m_Win);
                    ListenForEventsIn(m_Win);
                }
                else
                    m_Win.gameObject.SetActive(true);
                m_UIStack.Push(m_Win);
                break;

            case UI_STATE.LOSE:
                if (!m_Lose)
                {
                    m_Lose = Instantiate(m_LosePrefab, Vector3.zero, Quaternion.identity).GetComponent<UI_Abstract>();
                    m_ActiveUIObjects.Add(m_Lose);
                    ListenForEventsIn(m_Lose);
                }
                else
                    m_Lose.gameObject.SetActive(true);
                m_UIStack.Push(m_Lose);
                break;

            case UI_STATE.BACK:
                m_UIStack.Pop();
                if (m_UIStack.TryPeek(out LastUI))
                    LastUI.gameObject.SetActive(true);
                break;
        }
    }
}
