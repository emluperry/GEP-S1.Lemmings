using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HUD_ButtonManager : MonoBehaviour
{
    [SerializeField] private Button_OnClick FloatJob;
    [SerializeField] private Button_OnClick BlockingJob;
    [SerializeField] private Button_OnClick BuildJob;
    [SerializeField] private Button_OnClick ExplodeJob;

    public event Action<int> onRoleChosen;

    private void Start()
    {
        FloatJob.OnClicked += SetFloatingJob;
        BlockingJob.OnClicked += SetBlockingJob;
        BuildJob.OnClicked += SetBuildJob;
        ExplodeJob.OnClicked += SetExplodeJob;
    }

    private void OnDestroy()
    {
        FloatJob.OnClicked -= SetFloatingJob;
        BlockingJob.OnClicked -= SetFloatingJob;
        BuildJob.OnClicked -= SetBuildJob;
        ExplodeJob.OnClicked -= SetExplodeJob;
    }

    private void SetFloatingJob()
    {
        onRoleChosen?.Invoke(0);
    }
    private void SetBlockingJob()
    {
        onRoleChosen?.Invoke(1);
    }
    private void SetBuildJob()
    {
        onRoleChosen?.Invoke(2);
    }
    private void SetExplodeJob()
    {
        onRoleChosen?.Invoke(3);
    }
}
