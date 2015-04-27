using UnityEngine;

public static class BoundsExtensions
{
    public static Bounds Unit()
    {
        return new Bounds(Vector3.one, Vector3.one);
    }
}