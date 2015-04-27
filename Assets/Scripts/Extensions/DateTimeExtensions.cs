using System;

public static class DateTimeExtensions
{
    public static float SecondsFromEpoch(this DateTime @this)
    {
        var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        var diff = @this.ToUniversalTime() - origin;
        return (float) diff.TotalSeconds;
    }
}