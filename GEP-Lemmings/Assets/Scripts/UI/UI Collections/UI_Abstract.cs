using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lemmings.Enums;

public class UI_Abstract : MonoBehaviour
{
    public Action<int> CallLoadScene;
    public Action CallLoadNextScene;
    public Action CallReloadScene;
    public Action<UI_STATE> CallLoadUI;
    public Action CallQuitApp;

    protected void LoadUI(UI_STATE state)
    {
        CallLoadUI?.Invoke(state);
    }

    protected void BackButton()
    {
        CallLoadUI?.Invoke(UI_STATE.BACK);
    }

    protected void LoadLevel(int index)
    {
        CallLoadScene?.Invoke(index);
    }

    protected void ReloadScene()
    {
        CallReloadScene?.Invoke();
    }

    protected void LoadNextLevel()
    {
        CallLoadNextScene?.Invoke();
    }

    protected void QuitToTitle()
    {
        CallLoadScene?.Invoke(0);
    }

    protected void Quit()
    {
        CallQuitApp?.Invoke();
    }
}
