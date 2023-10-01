using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClockWidget : MonoBehaviour
{
    public GameObject clockWidget;
    public TimeOfDay timeOfDay;
    public TMP_Text timeOfDayText;

    [Header("Times of Day")]
    public bool isSunrise;
    public bool isDayTime;
    public bool isSunset;
    public bool isNightTime;
    public bool isDreamWorld;


    [Header("Icons")]
    public GameObject dayEmoji;
    public GameObject sunsetSunriseEmoji;
    public GameObject nightEmoji;
    public GameObject dreamWorldEmoji;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        ChangeTheTimeOfDay();

        if(isSunrise)
        {
            Sunrise();
        }
        
        if(isDayTime)
        {
            DayTime();
        }

        if(isSunset)
        {
            Sunset();
        }

        if(isNightTime)
        {
            Night();
        }

        if(isDreamWorld)
        {
            DreamWorld();
        }
    }

    public void Sunrise()
    {
        sunsetSunriseEmoji.SetActive(true);
        dayEmoji.SetActive(false);
        nightEmoji.SetActive(false);
        dreamWorldEmoji.SetActive(false);

        timeOfDayText.text = "SUNRISE";
    }

    public void DayTime()
    {
        sunsetSunriseEmoji.SetActive(false);
        dayEmoji.SetActive(true);
        nightEmoji.SetActive(false);
        dreamWorldEmoji.SetActive(false);

        timeOfDayText.text = "DAYTIME";
    }

    public void Sunset()
    {
        sunsetSunriseEmoji.SetActive(true);
        dayEmoji.SetActive(false);
        nightEmoji.SetActive(false);
        dreamWorldEmoji.SetActive(false);

        timeOfDayText.text = "SUNSET";
    }

    public void Night()
    {
        sunsetSunriseEmoji.SetActive(false);
        dayEmoji.SetActive(false);
        nightEmoji.SetActive(true);
        dreamWorldEmoji.SetActive(false);

        timeOfDayText.text = "NIGHT";
    }

    public void DreamWorld()
    {
        sunsetSunriseEmoji.SetActive(false);
        dayEmoji.SetActive(false);
        nightEmoji.SetActive(false);
        dreamWorldEmoji.SetActive(true);

        timeOfDayText.text = "DREAM WORLD";
    }

    //This is the Enum that sets the Time of day. This will be changed whenever you progress through the day.
    public void ChangeTheTimeOfDay()
    {
        switch (timeOfDay)
        {
            case TimeOfDay.Sunrise :
                isSunrise = true;
                break;
            case TimeOfDay.Day :
                isDayTime = true;
                break;
            case TimeOfDay.Sunset :
                isSunset = true;
                break;
            case TimeOfDay.Night :
                isNightTime = true;
                break;
            case TimeOfDay.DreamWorld :
                isDreamWorld = true;
                break;
            default :
                Debug.LogError("Time of Day does not exist!");
                break;
        }
    }

    public enum TimeOfDay
    {
        Sunrise,
        Day,
        Sunset,
        Night,
        DreamWorld
    }
}
