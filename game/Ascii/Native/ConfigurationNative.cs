﻿using System;
using System.Runtime.InteropServices;

namespace game.Ascii.Native
{
    public enum EntryType
    {
        Float = 0,
	    Double,
	    Int,
	    UInt,
	    String,
	    Bool
    }
    
    public static class ConfigurationNative
    {
	    [DllImport("libascii.so")]
	    public static extern IntPtr configuration_get(EntryType type, string path);
    }
}