using System;
using System.Collections.Generic;

using UnityEngine;

public static class IEnumerableExtensions
{
    public static void ForAll<T>(this IEnumerable<T> @this, Action<T> action)
    {
        foreach (var element in @this)
        {
            action(element);
        }
    }

    public static T Last<T>(this IList<T> @this)
    {
        return @this[Mathf.Max(0, @this.Count - 1)];
    }

    public static T RemoveLast<T>(this IList<T> @this)
    {
        T value = default(T);
        if (@this.Count > 0)
        {
            value = @this[@this.Count - 1];

            @this.RemoveAt(@this.Count - 1);
        }

        return value;
    }

    public static void Randomize<T>(this IList<T> @this)
    {
        for (int i = 0, len = @this.Count; i < len - 1; i++)
        {
            var a = UnityEngine.Random.Range(0, len - 1);
            var b = UnityEngine.Random.Range(0, len - 1);

            var temp = @this[a];
            @this[a] = @this[b];
            @this[b] = temp;
        }
    }

    public static void Randomize<T>(this IList<T> @this, int count)
    {
        count = Mathf.Min(count, @this.Count);

        for (int i = 0; i < count - 1; i++)
        {
            var a = UnityEngine.Random.Range(0, count - 1);
            var b = UnityEngine.Random.Range(0, count - 1);

            var temp = @this[a];
            @this[a] = @this[b];
            @this[b] = temp;
        }
    }

    public static void Distinct<T>(this List<T> @this)
    {
        for (var i = @this.Count - 1; i >= 1; i--)
        {
            for (var j = i - 1; j >= 0; j--)
            {
                if (@this[i].Equals(@this[j]))
                {
                    @this.RemoveAt(i);

                    break;
                }
            }
        }
    }

    public static bool Contains<T>(this T[] @this, T element)
    {
        for (int i = 0, len = @this.Length; i < len; i++)
        {
            if (element.Equals(@this[i]))
            {
                return true;
            }
        }

        return false;
    }
}