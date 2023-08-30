using System;
namespace RemotelyControlledRobot.Framework.Extentions
{
    public static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)
        {
            if (dictionary.TryGetValue(key, out var value))
            {
                return value;
            }
            return defaultValue;
        }
    }
}

