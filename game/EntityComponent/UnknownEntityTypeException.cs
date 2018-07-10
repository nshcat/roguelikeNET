using System;
using System.Runtime.Serialization;

namespace game.EntityComponent
{
    /// <summary>
    /// An exception type describing a case where an unknown entity type was
    /// requested
    /// </summary>
    public class UnknownEntityTypeException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public UnknownEntityTypeException()
            : base()
        {
            
        }

        /// <summary>
        /// Construct with description message
        /// </summary>
        /// <param name="message">Description</param>
        public UnknownEntityTypeException(string message)
            : base(message)
        {
            
        }

        /// <summary>
        /// Construct with description message and inner exception
        /// </summary>
        /// <param name="message">Description</param>
        /// <param name="innerException">Inner exception</param>
        public UnknownEntityTypeException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }

        /// <summary>
        /// Construct with serialization information
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="ctx">Streaming context</param>
        public UnknownEntityTypeException(SerializationInfo info, StreamingContext ctx)
            : base(info, ctx)
        {
            
        }
    }
}