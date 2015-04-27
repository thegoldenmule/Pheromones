using System;
using UnityEngine;

public static class FloatExtensions
{
    public static bool Approximately(this float @this, float value)
    {
        var epsilon = Mathf.Epsilon;

        return Math.Abs(@this - value) < epsilon;
    }
}