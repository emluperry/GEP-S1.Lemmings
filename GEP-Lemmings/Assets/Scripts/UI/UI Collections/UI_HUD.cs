using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Lemmings.Enums;

public class UI_HUD : UI_Abstract
{
    [Header("Buttons")]
    [SerializeField] private Button_OnClick FloatJob;
    [SerializeField] private Button_OnClick BlockingJob;
    [SerializeField] private Button_OnClick BuildJob;
    [SerializeField] private Button_OnClick ExplodeJob;
    [SerializeField] private Button_OnClick NoneJob;

    [Header("Role Numbers")]
    [SerializeField] private UpdatableValue FloatVal;
    [SerializeField] private UpdatableValue BlockingVal;
    [SerializeField] private UpdatableValue BuildVal;
    [SerializeField] private UpdatableValue ExplodeVal;

    [Header("Stat Numbers")]
    [SerializeField] private UpdatableValue TimerMins;
    [SerializeField] private UpdatableValue TimerSecs;
    [SerializeField] private UpdatableValue CurrentLemmings;
    [SerializeField] private UpdatableValue NumWinningLemmings;
    [SerializeField] private UpdatableValue NumTotalWinLemmings;


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

    public void UpdateJob(LEMMING_JOB job, int newVal)
    {
        switch (job)
        {
            case LEMMING_JOB.FLOATING:
                FloatVal.UpdateValue(newVal.ToString());
                break;
            case LEMMING_JOB.BLOCKING:
                BlockingVal.UpdateValue(newVal.ToString());
                break;
            case LEMMING_JOB.BUILDING:
                BuildVal.UpdateValue(newVal.ToString());
                break;
            case LEMMING_JOB.EXPLODING:
                ExplodeVal.UpdateValue(newVal.ToString());
                break;
        }
    }

    public void SetTotalLemmingsNeeded(int num)
    {
        NumTotalWinLemmings.UpdateValue("/" + num.ToString());
    }

    public void UpdateWinningNumLemmings(int num)
    {
        NumWinningLemmings.UpdateValue(num.ToString());
    }

    public void UpdateActiveNumLemmings(int num)
    {
        CurrentLemmings.UpdateValue(num.ToString() + "-");
    }

    public void UpdateSeconds(int num)
    {
        TimerSecs.UpdateValue(num.ToString() + "s");
    }

    public void UpdateMinutes(int num)
    {
        TimerMins.UpdateValue(num.ToString() + "m");
    }
}
