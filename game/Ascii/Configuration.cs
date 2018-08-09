using System;

namespace game.Ascii
{
    public static class Configuration
    {
        private static object GetValueImpl<T>(string path)
        {
            var type = typeof(T);

            if (type == typeof(float))
                return Native.ConfigurationNative.configuration_get_float(path);
            else if(type == typeof(double))
                return Native.ConfigurationNative.configuration_get_double(path);
            else if(type == typeof(int))
                return Native.ConfigurationNative.configuration_get_int(path);
            else if(type == typeof(uint))
                return Native.ConfigurationNative.configuration_get_uint(path);
            else if(type == typeof(string))
                return Memory.PtrToString(Native.ConfigurationNative.configuration_get_string(path));
            else if(type == typeof(bool))
                return Native.ConfigurationNative.configuration_get_boolean(path);
            else
            {
                Logger.PostMessageTagged(SeverityLevel.Fatal, "Configuration", $"Unsupported type in GetValue: {type}");
                throw new ArgumentException($"Unsupported type in GetValue: {type}");
            }
        }

        public static T GetValue<T>(string path)
        {
            return (T)GetValueImpl<T>(path);
        }
    }
}