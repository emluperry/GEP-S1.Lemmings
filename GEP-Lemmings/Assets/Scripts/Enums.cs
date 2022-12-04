using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lemmings.Enums
{
    public enum LEMMING_STATE
    {
        WALKING,
        FALLING,
        TURNING,
        DEAD
    }

    public enum LEMMING_JOB
    {
        NONE,
        FLOATING,
        BUILDING,
        BLOCKING,
        EXPLODING
    }

    public enum UI_STATE
    {
        NONE,
        LEVEL_SELECT,
        HOWTOPLAY,
        SETTINGS,
        PAUSED,
        WIN,
        LOSE,
        BACK
    }

    public enum VOLUME_SLIDER
    {
        MASTERVOLUME,
        MUSICVOLUME,
        SFXVOLUME
    }

}

