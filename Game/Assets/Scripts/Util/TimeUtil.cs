using UnityEngine;

public static class TimeUtil
{
    public static string SecondsToDigitalClock(int seconds)
    {
        var mm = Mathf.FloorToInt(seconds / 60f).ToString("D2");
        var ss = (seconds % 60).ToString("D2");

        return string.Format("{0}:{1}", mm, ss);
    }

    public static string SecondsToDigitalClock(float seconds)
    {
        return SecondsToDigitalClock(Mathf.RoundToInt(seconds));
    }
}
