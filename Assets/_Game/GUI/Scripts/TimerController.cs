using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimerController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerTMP;
    [SerializeField] DateTime nextDate;

    bool isShowingTime;

    public void SetNextDate(DateTime _day)
    {
        nextDate = _day;
    }

    public void EnableTimer()
    {
        isShowingTime = true;
    }

    void Update()
    {
        if (!isShowingTime) return;

        DateTime now = DateTime.Now;

        //DateTime tomorrow = now.AddDays(1).Date;
        TimeSpan timeRemaining = nextDate - now;

        string timeFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                             timeRemaining.Hours,
                                             timeRemaining.Minutes,
                                             timeRemaining.Seconds);
        timerTMP.text = timeFormatted;
    }
}
