using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using game.Ascii.Native;

namespace game.Ascii
{
    public static class Configuration
    {
        private static readonly Dictionary<Type, Native.EntryType> TypeMap
            = new Dictionary<Type, EntryType>()
            {
                { typeof(int), EntryType.Int },
                { typeof(uint), EntryType.UInt },
                { typeof(float), EntryType.Float },
                { typeof(double), EntryType.Double },
                { typeof(string), EntryType.String },
                { typeof(bool), EntryType.Bool }         
            };
        
        
        private static Native.EntryType getEntryType<T>()
        {
            if (TypeMap.ContainsKey(typeof(T)))
            {
                return TypeMap[typeof(T)];
            }
            else
            {
                Logger.postMessage(SeverityLevel.Fatal, "configuration", "Invalid Type T given for Configuration.getValue<T>");
                throw new Exception();
            }
        }

        private static Object unpackValue<T>(IntPtr p)
        {
            var type = getEntryType<T>();

            switch (type)
            {
                case EntryType.String:
                    return Marshal.PtrToStringAnsi(p);
                
                case EntryType.Bool:
                    return Marshal.ReadByte(p) != 0;

                case EntryType.Float:
                    Single[] fbuf = new Single[1];
                    Marshal.Copy(p, fbuf, 0, 1);
                    return fbuf[0];

                case EntryType.Double:
                    Double[] dbuf = new Double[1];
                    Marshal.Copy(p, dbuf, 0, 1);
                    return dbuf[0];
                
                case EntryType.Int: // TODO: Is the int really always 32 bits? Check that in libascii!
                    return Marshal.ReadInt32(p);
                
                case EntryType.UInt:
                    return (uint) Marshal.ReadInt32(p);
                
                default:
                    Logger.postMessage(SeverityLevel.Fatal, "configuration", "Invalid Type T given for Configuration.getValue<T>");
                    throw new Exception();
            }

        }
        
        public static T getValue<T>(string path)
        {
            var valPtr = Native.ConfigurationNative.configuration_get(getEntryType<T>(), path);

            return (T)unpackValue<T>(valPtr);
        }
    }
}