using System;
using System.Runtime.InteropServices;

namespace game.Ascii.Native
{   
    public static class LoggerNative
    {
        [DllImport("libascii.so")]      
        public static extern void logger_post_message(SeverityLevel lvl, string tag, string msg);
    }
}