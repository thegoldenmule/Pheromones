using UnityEngine;

public static class AnimationCurveExtensions
{
    public static void Constant(this AnimationCurve @this, float constant)
    {
        while (@this.keys.Length > 0)
        {
            @this.RemoveKey(0);
        }

        @this.AddKey(0, constant);
    }

    public static float Duration(this AnimationCurve @this)
    {
        var len = @this.keys.Length;
        if (len > 0)
        {
            return @this.keys[len - 1].time;
        }

        return 0f;
    }
}