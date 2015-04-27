using System;
using System.Collections.Generic;
using System.Reflection;

public static class TypeExtensions
{
    public static Type[] Implementors(this Type baseType)
    {
        List<Type> implementors = new List<Type>();
            
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        for (int i = 0, len = assemblies.Length; i < len; i++)
        {
            Assembly assembly = assemblies[i];
            Type[] types = assembly.GetTypes();
            for (int j = 0, jlen = types.Length; j < jlen; j++)
            {
                Type type = types[j];
                if (baseType != type)
                {
                    if (baseType.IsAssignableFrom(type))
                    {
                        implementors.Add(type);
                    }
                }
            }
        }

        return implementors.ToArray();
    }
}