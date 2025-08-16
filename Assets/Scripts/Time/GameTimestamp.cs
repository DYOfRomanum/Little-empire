using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameTimestamp
{
    public int year;
    public int week;
    public int day;
    public int hour;
    public int minute;
    public int second;

    //constructor to set up the class
    public GameTimestamp(int year, int week, int day, int hour, int minute, int second)
    {
        this.year = year;
        this.week = week;
        this.day = day;
        this.hour = hour;
        this.minute = minute;
        this.second = second;
    }

    public void UpdateClock()
    {
        second++;
        //60sec in 1min
        if (second>=60)
        {
            //reset second
            second = 0;
            minute++;
        }
        // 60min in 1h
        if (minute>=60)
        {
            minute = 0;
            hour++;
        }
        // 24h in 1day
        if (hour>=24)
        {
            hour=0;
            day++;
        }
        // 7day in 1week
        if (day-(week+1)*7>=7)
        {
            week++;
        }
        // 365day in 1 year
        if (day>=365)
        {
            day = 0;
            week = 0;
            year++;
        }
    }
    public static int MinuteToSeconds(int minute)
    {
        return minute*60;
    }
    public static int HoursToMinutes(int hour)
    {
        return hour*60;
    }
}
