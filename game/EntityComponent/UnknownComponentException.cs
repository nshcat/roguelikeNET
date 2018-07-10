using System;
using System.Runtime.Serialization;

namespace game.EntityComponent
{
    /// <summary>
    /// An exception type describing a case where an unknown component type was
    /// referenced by a entity description document.
    /// </summary>
    public class UnknownComponentException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public UnknownComponentException()
            : base()
        {
            
        }

        /// <summary>
        /// Construct with description message
        /// </summary>
        /// <param name="message">Description</param>
        public UnknownComponentException(string message)
            : base(message)
        {
            
        }

        /// <summary>
        /// Construct with description message and inner exception
        /// </summary>
        /// <param name="message">Description</param>
        /// <param name="innerException">Inner exception</param>
        public UnknownComponentException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }

        /// <summary>
        /// Construct with serialization information
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="ctx">Streaming context</param>
        public UnknownComponentException(SerializationInfo info, StreamingContext ctx)
            : base(info, ctx)
        {
            
        }
    }
}