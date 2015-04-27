using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

public static class GameObjectExtensions
{
    public static T GetComponentAs<T>(this GameObject @this) where T : class
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

    public static T[] GetComponentsAs<T>(this GameObject @this) where T : class
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

    public static T GetComponentInChildrenAs<T>(this GameObject @this) where T : class
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

    public static T[] GetComponentsInChildrenAs<T>(this GameObject @this) where T : class
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

    public static T MoveComponent<T>(this GameObject @this, T component) where T : Component
    {
        var newComponent = @this.AddComponent<T>();
        
        var type = component.GetType();

        var fields = type.GetFields(BindingFlags.Public
            | BindingFlags.Instance
            | BindingFlags.Default
            | BindingFlags.DeclaredOnly);
        for (int i = 0, len = fields.Length; i < len; i++)
        {
            var field = fields[i];
            field.SetValue(newComponent, field.GetValue(component));
        }

        UnityEngine.Object.Destroy(component);

        return newComponent;
    }
}