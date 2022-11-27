using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Lemmings.Enums;

public class HUD_ButtonManager : MonoBehaviour
{
    [SerializeField] private Button_OnClick FloatJob;
    [SerializeField] private Button_OnClick BlockingJob;
    [SerializeField] private Button_OnClick BuildJob;
    [SerializeField] private Button_OnClick ExplodeJob;
    [SerializeField] private Button_OnClick NoneJob;

    [SerializeField] private UpdatableValue FloatVal;
    [SerializeField] private UpdatableValue BlockingVal;
    [SerializeField] private UpdatableValue BuildVal;
    [SerializeField] private UpdatableValue ExplodeVal;

    public event Action<LEMMING_JOB> onRoleChosen;

    private void Start()
    {
        FloatJob.OnClicked += SetFloatingJob;
        BlockingJob.OnClicked += SetBlockingJob;
        BuildJob.OnClicked += SetBuildJob;
        ExplodeJob.OnClicked += SetExplodeJob;
        NoneJob.OnClicked += SetNone;
    }

    private void OnDestroy()
    {
        FloatJob.OnClicked -= SetFloatingJob;
        BlockingJob.OnClicked -= SetFloatingJob;
        BuildJob.OnClicked -= SetBuildJob;
        ExplodeJob.OnClicked -= SetExplodeJob;
        NoneJob.OnClicked -= SetNone;
    }

    private void SetFloatingJob()
    {
        onRoleChosen?.Invoke(LEMMING_JOB.FLOATING);
    }
    private void SetBlockingJob()
    {
        onRoleChosen?.Invoke(LEMMING_JOB.BLOCKING);
    }
    private void SetBuildJob()
    {
        onRoleChosen?.Invoke(LEMMING_JOB.BUILDING);
    }
    private void SetExplodeJob()
    {
        onRoleChosen?.Invoke(LEMMING_JOB.EXPLODING);
    }

    private void SetNone()
    {
        onRoleChosen?.Invoke(LEMMING_JOB.NONE);
    }

    public void UpdateValue(LEMMING_JOB job, int newVal)
    {
        switch (job)
        {
            case LEMMING_JOB.FLOATING:
                FloatVal.UpdateValue(newVal);
                break;
            case LEMMING_JOB.BLOCKING:
                BlockingVal.UpdateValue(newVal);
                break;
            case LEMMING_JOB.BUILDING:
                BuildVal.UpdateValue(newVal);
                break;
            case LEMMING_JOB.EXPLODING:
                ExplodeVal.UpdateValue(newVal);
                break;
        }
    }
}
