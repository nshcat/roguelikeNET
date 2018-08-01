using System;

namespace game.Ascii
{
    /// <summary>
    /// An enumeration that describes the level of importance that is
    /// associated with a message.
    /// </summary>
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
        /// <summary>
        /// Post a tagged message to the logging system
        /// </summary>
        /// <param name="level">Severity level of the message</param>
        /// <param name="tag">Tag used to differentiate between different message sources</param>
        /// <param name="message">Message contents</param>
        /// <param name="lines">Additional message lines. They will be send as bare logger messages</param>
        public static void PostMessageTagged(SeverityLevel level, string tag, string message, params string[] lines)
        {
            // Duplication of code is worth it here, since locking is very expensive in this
            // context.
            if (lines.Length > 0)
            {
                Native.LoggerNative.logger_lock();
                Native.LoggerNative.logger_post_message(level, tag, message, false);

                foreach (var line in lines)
                {
                    Native.LoggerNative.logger_post_message(level, tag, line, true);
                }
                
                Native.LoggerNative.logger_unlock();
            }
            else
            {
                Native.LoggerNative.logger_post_message(level, tag, message, false);
            }
        }
        
        /// <summary>
        /// Post a message to the logging system
        /// </summary>
        /// <param name="level">Severity level of the message</param>
        /// <param name="message">Message contents</param>
        /// <param name="lines">Additional message lines. They will be send as bare logger messages</param>
        public static void PostMessage(SeverityLevel level, string message, params string[] lines)
        {
            PostMessageTagged(level, String.Empty, message, lines);
        }
    }
}