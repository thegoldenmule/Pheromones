using System;
using UnityEngine;

public static class RectExtensions
{
    public static Vector2 Min(this Rect @this)
    {
        return new Vector2(@this.x, @this.y);
    }

    public static Vector2 Max(this Rect @this)
    {
        return new Vector2(@this.x + @this.width, @this.y + @this.height);
    }

    public static bool Approximately(this Rect @this, Rect other)
    {
        var epsilon = Mathf.Epsilon;

        return Math.Abs(@this.x - other.x) < epsilon
            && Math.Abs(@this.y - other.y) < epsilon
            && Math.Abs(@this.width - other.width) < epsilon
            && Math.Abs(@this.height - other.height) < epsilon;
    }
}