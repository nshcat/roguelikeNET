using System;

namespace game.Ascii
{
    public enum SeverityLevel
    {
        Fatal = 0,
        Error, 
        Warning,
        Info,
        Debug
    }
    
    public static class Logger
    {
        public static void postMessage(SeverityLevel level, String tag, String message)
        {
            Native.LoggerNative.logger_post_message(level, tag, message);
        }
        
        public static void postMessage(SeverityLevel level, String message)
        {
            Native.LoggerNative.logger_post_message(level, String.Empty, message);
        }
    }
}