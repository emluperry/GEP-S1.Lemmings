using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Lemmings.Enums;
using TreeEditor;

public class Scene_Manager : MonoBehaviour
{
    [Header("Prefab Objects")]
    [SerializeField] private GameObject m_LevelSelectPrefab;
    [SerializeField] private GameObject m_HowToPlayPrefab;
    [SerializeField] private GameObject m_SettingsPrefab;
    [SerializeField] private GameObject m_PausePrefab;
    [SerializeField] private GameObject m_WinPrefab;
    [SerializeField] private GameObject m_LosePrefab;

    [SerializeField] private CanvasGroup m_LoadScreenObject;
    private UI_Abstract m_LevelSelect;
    private UI_Abstract m_HowToPlay;
    private UI_Abstract m_Settings;
    private UI_Abstract m_Pause;
    private UI_Abstract m_Win;
    private UI_Abstract m_Lose;

    [Header("Other Managers")]
    [SerializeField] private SettingsManager m_SettingsManager;
    private GameManager m_CurrentGameManager;
    [SerializeField] private float m_FadeInOutTime = 1f;

    [SerializeField] private Camera_Controller m_Camera;

    private Stack<UI_Abstract> m_UIStack;

    private List<UI_Abstract> m_ActiveUIObjects;

    private void Awake()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            m_Camera.SetPaused(true);
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        }

        if (m_SettingsManager)
            m_SettingsManager.RestoreSavedSettings();
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
        {
            m_CurrentGameManager.onPausePressed += LoadPauseMenu;
            m_CurrentGameManager.onLevelEnd += LoadWinLoseScreen;

            m_Camera.SetPaused(false);
        }

        StartCoroutine(FadeOut());
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
        if (m_CurrentGameManager)
        {
            m_CurrentGameManager.onPausePressed -= LoadPauseMenu;
            m_CurrentGameManager.onLevelEnd -= LoadWinLoseScreen;

            m_Camera.SetPaused(true);
        }

        m_Camera.ResetPosition();

        if(m_SettingsManager && m_Settings)
        {
            m_SettingsManager.StopListeningForEvents();
        }

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

        if(m_Pause)
            m_Pause.GetComponent<UI_Pause>().onUnpause -= Unpause;

        m_ActiveUIObjects.Clear();
    }

    private void LoadScene(int BuildIndex)
    {
        StartCoroutine(FadeIn());

        StopListeningForEvents();

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadSceneAsync(BuildIndex, LoadSceneMode.Additive);
    }

    private IEnumerator FadeIn()
    {
        float Increment = 1 / m_FadeInOutTime;
        m_LoadScreenObject.gameObject.SetActive(true);
        m_LoadScreenObject.blocksRaycasts = true;
        m_LoadScreenObject.alpha = 0;
        while (m_LoadScreenObject.alpha < 1)
        {
            m_LoadScreenObject.alpha += Increment;
            yield return new WaitForFixedUpdate();
        }
        m_LoadScreenObject.alpha = 1;
    }

    private IEnumerator FadeOut()
    {
        float Increment = 1 / m_FadeInOutTime;
        m_LoadScreenObject.alpha = 1;
        while (m_LoadScreenObject.alpha < 1)
        {
            m_LoadScreenObject.alpha -= Increment;
            yield return new WaitForFixedUpdate();
        }
        m_LoadScreenObject.alpha = 0;
        m_LoadScreenObject.blocksRaycasts = false;
        m_LoadScreenObject.gameObject.SetActive(false);
    }

    private void RestartLevel()
    {
        int BuildIndex = SceneManager.GetActiveScene().buildIndex;
        LoadScene(BuildIndex);
    }

    private void LoadNextLevel()
    {
        int BuildIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if(BuildIndex >= SceneManager.sceneCountInBuildSettings)
        {
            BuildIndex = 1;
        }
        LoadScene(BuildIndex);
    }

    private void QuitApplication()
    {
        Application.Quit();
    }

    private void Unpause()
    {
        m_CurrentGameManager.PauseScene();
        LoadPauseMenu(false);
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
                if(uiObject.GetComponent<UI_Settings>())
                    m_SettingsManager.RestoreSavedSettings();

                LoadUI(UI_STATE.BACK);
            }
            if (uiObject && uiObject.GetComponent<UI_Pause>())
                LoadUI(UI_STATE.BACK);
        }
        m_Camera.SetPaused(paused);
    }

    private void LoadWinLoseScreen(bool playerWon)
    {
        if (playerWon)
            LoadUI(UI_STATE.WIN);
        else
            LoadUI(UI_STATE.LOSE);
    }

    private void LoadUI(UI_STATE UIScreen)
    {
        if(m_UIStack.TryPeek(out UI_Abstract LastUI))
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
                    m_Pause.GetComponent<UI_Pause>().onUnpause += Unpause;
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
                    m_SettingsManager.ListenForEvents(m_Settings.GetComponent<UI_Settings>());
                }
                else
                    m_Settings.gameObject.SetActive(true);
                m_SettingsManager.RestoreSettingMenuValues();
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

            case UI_STATE.LEVEL_SELECT:
                if (!m_LevelSelect)
                {
                    m_LevelSelect = Instantiate(m_LevelSelectPrefab, Vector3.zero, Quaternion.identity).GetComponent<UI_Abstract>();
                    m_ActiveUIObjects.Add(m_LevelSelect);
                    ListenForEventsIn(m_LevelSelect);
                }
                else
                    m_LevelSelect.gameObject.SetActive(true);
                m_UIStack.Push(m_LevelSelect);
                break;

            case UI_STATE.BACK:
                m_UIStack.Pop();
                if (m_UIStack.TryPeek(out LastUI))
                    LastUI.gameObject.SetActive(true);
                break;
        }
    }
}
