using UnityEngine;

public static class ColorExtensions
{
    public static bool Approximately(this Color @this, Color color)
    {
        return Mathf.Approximately(@this.r, color.r)
            && Mathf.Approximately(@this.g, color.g)
            && Mathf.Approximately(@this.b, color.b)
            && Mathf.Approximately(@this.a, color.a);
    }
}