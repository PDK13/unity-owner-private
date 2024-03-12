using System;
using System.Collections.Generic;
using System.ComponentModel;

public enum FormatTimeType
{
    [Description("{0:D2}")]
    Sec = 1,
    [Description("{0:D2}:{1:D2}")]
    MinSec,
    [Description("{0:D2}:{1:D2}:{2:D2}")]
    HouMinSec,
}

public static class TimeExtension
{
    public static string FormatTime(FormatTimeType formatType, int seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        List<object> convertTimes = new List<object>() { time.Seconds, time.Minutes, time.Hours };
        List<object> doubleTimes = new List<object>();
        for (int i = (int)formatType - 1; i >= 0; i--)
        {
            doubleTimes.Add(convertTimes[i]);
        }
        return string.Format(ToDescriptionString(formatType), doubleTimes.ToArray());
    }

    public static string ToDescriptionString(FormatTimeType val)
    {
        DescriptionAttribute[] attributes = (DescriptionAttribute[])val
           .GetType()
           .GetField(val.ToString())
           .GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
    }
}
