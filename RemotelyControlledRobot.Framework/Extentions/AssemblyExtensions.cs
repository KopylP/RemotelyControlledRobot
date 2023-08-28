using System;
using System.Reflection;

namespace RemotelyControlledRobot.Framework.Extentions
{
    public static class AssemblyExtensions
    {
        public static Type[] GetAllImplementingInterface(this Assembly assembly, Type interfaceType)
        {
            return assembly.GetTypes()
                .Where(type => interfaceType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .ToArray();
        }
    }
}

