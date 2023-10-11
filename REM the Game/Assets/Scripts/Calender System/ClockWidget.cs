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
    }

    public void TimeChange()
    {
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
        GameManager.instance.sunsetLights.gameObject.SetActive(true);
        GameManager.instance.sunsetLights.color = new Vector4(0.9292453f, 1f, 0.9951574f, 0.09411765f);
        
    }

    public void DayTime()
    {
        sunsetSunriseEmoji.SetActive(false);
        dayEmoji.SetActive(true);
        nightEmoji.SetActive(false);
        dreamWorldEmoji.SetActive(false);

        timeOfDayText.text = "DAYTIME";
        GameManager.instance.sunsetLights.gameObject.SetActive(false);
    }

    public void Sunset()
    {
        sunsetSunriseEmoji.SetActive(true);
        dayEmoji.SetActive(false);
        nightEmoji.SetActive(false);
        dreamWorldEmoji.SetActive(false);

        timeOfDayText.text = "SUNSET";
        GameManager.instance.sunsetLights.gameObject.SetActive(true);
        GameManager.instance.sunsetLights.color = new Vector4(1, 0.8611606f, 0.6933962f, 0.09411765f);
    }

    public void Night()
    {
        sunsetSunriseEmoji.SetActive(false);
        dayEmoji.SetActive(false);
        nightEmoji.SetActive(true);
        dreamWorldEmoji.SetActive(false);

        timeOfDayText.text = "NIGHT";
        GameManager.instance.sunsetLights.gameObject.SetActive(true);
        GameManager.instance.sunsetLights.color = new Vector4(0.5730687f, 0.6930597f, 0.8867924f, 1f);
        
    }

    public void DreamWorld()
    {
        sunsetSunriseEmoji.SetActive(false);
        dayEmoji.SetActive(false);
        nightEmoji.SetActive(false);
        dreamWorldEmoji.SetActive(true);

        timeOfDayText.text = "DREAM WORLD";
        GameManager.instance.sunsetLights.gameObject.SetActive(false);
    }

    //This is the Enum that sets the Time of day. This will be changed whenever you progress through the day.
    public void ChangeTheTimeOfDay()
    {
        switch (timeOfDay)
        {
            case TimeOfDay.Sunrise :
                isSunrise = true;
                //Disable all other bools
                isDayTime = false;
                isSunset = false;
                isNightTime = false;
                isDreamWorld = false;
                TimeChange();
                break;
            case TimeOfDay.Day :
                isDayTime = true;
                //Disable all other bools
                isSunrise = false;
                isSunset = false;
                isNightTime = false;
                isDreamWorld = false;
                TimeChange();
                break;
            case TimeOfDay.Sunset :
                isSunset = true;
                //Disable all other bools
                isDayTime = false;
                isSunrise = false;
                isNightTime = false;
                isDreamWorld = false;
                TimeChange();
                break;
            case TimeOfDay.Night :
                isNightTime = true;
                //Disable all other bools
                isDayTime = false;
                isSunset = false;
                isSunrise = false;
                isDreamWorld = false;
                TimeChange();
                break;
            case TimeOfDay.DreamWorld :
                isDreamWorld = true;
                //Disable all other bools
                isDayTime = false;
                isSunset = false;
                isNightTime = false;
                isSunrise = false;
                TimeChange();
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
