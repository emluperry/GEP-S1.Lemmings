using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Abstract : MonoBehaviour
{
    public Action<int> CallLoadScene;

    public Action CallQuitApp;

    protected void LoadLevel(int index)
    {
        CallLoadScene?.Invoke(index);
    }

    protected void Quit()
    {
        CallQuitApp?.Invoke();
    }
}
