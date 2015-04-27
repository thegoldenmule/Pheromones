using UnityEngine;

public static class VectorExtensions
{
    public static Vector3 ToXZVector3(this Vector2 @this) {
        return new Vector3(@this.x, 0f, @this.y);
    }

    public static bool Approximately(this Vector3 @this, Vector3 vector)
    {
        var epsilon = Mathf.Epsilon;

        return Mathf.Abs(@this.x - vector.x) < epsilon
            && Mathf.Abs(@this.y - vector.y) < epsilon
            && Mathf.Abs(@this.z - vector.z) < epsilon;
    }

    public static Vector3 Floor(this Vector3 @this)
    {
        return new Vector3(
            Mathf.Floor(@this.x),
            Mathf.Floor(@this.y),
            Mathf.Floor(@this.z));
    }

    public static Vector3 Round(this Vector3 @this)
    {
        return new Vector3(
            Mathf.Round(@this.x),
            Mathf.Round(@this.y),
            Mathf.Round(@this.z));
    }

    public static Vector2 xz(this Vector3 @this)
    {
        return new Vector2(@this.x, @this.z);
    }

    public static Vector2 xy(this Vector3 @this)
    {
        return new Vector2(@this.x, @this.y);
    }

    public static bool IsNan(this Vector3 @this)
    {
        return float.IsNaN(@this.x) || float.IsNaN(@this.y) || float.IsNaN(@this.z);
    }

    public static Vector3 NaN
    {
        get
        {
            return new Vector3(float.NaN, float.NaN, float.NaN);
        }
    }

    public static float DistanceFromLine(Vector3 a, Vector3 b, Vector3 point)
    {
        var epsilon = Mathf.Epsilon;
        var lenSquared = (a - b).sqrMagnitude;

        if (lenSquared < epsilon)
        {
            return (a - point).magnitude;
        }

        float t = Vector3.Dot(point - a, point - b) / lenSquared;
        if (t < 0)
        {
            return (a - point).magnitude;
        }

        if (t > 1f)
        {
            return (b - point).magnitude;
        }

        Vector3 projection = a + t * (b - point);
        return (projection - point).magnitude;
    }
}