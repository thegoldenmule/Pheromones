using System.Collections.Generic;
using UnityEngine;

public static class ComponentExtensions
{
    public static T GetComponentAs<T>(this Component @this) where T : class
    {
        var components = @this.GetComponents<Component>();
        for (int i = 0, len = components.Length; i < len; i++)
        {
            var instance = components[i] as T;
            if (null != instance)
            {
                return instance;
            }
        }

        return null;
    }

    public static T[] GetComponentsAs<T>(this Component @this) where T : class
    {
        var ts = new List<T>();

        var components = @this.GetComponents<Component>();
        for (int i = 0, len = components.Length; i < len; i++)
        {
            var instance = components[i] as T;
            if (null != instance)
            {
                ts.Add(instance);
            }
        }

        return ts.ToArray();
    }

    public static T GetComponentInChildrenAs<T>(this Component @this) where T : class
    {
        var components = @this.GetComponentsInChildren<Component>();
        for (int i = 0, len = components.Length; i < len; i++)
        {
            var instance = components[i] as T;
            if (null != instance)
            {
                return instance;
            }
        }

        return null;
    }

    public static T[] GetComponentsInChildrenAs<T>(this Component @this) where T : class
    {
        var ts = new List<T>();

        var components = @this.GetComponentsInChildren<Component>();
        for (int i = 0, len = components.Length; i < len; i++)
        {
            var instance = components[i] as T;
            if (null != instance)
            {
                ts.Add(instance);
            }
        }

        return ts.ToArray();
    }
}