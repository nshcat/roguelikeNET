using System;
using System.Runtime.Serialization;

namespace game.EntityComponent
{
    /// <summary>
    /// An exception type describing a case where an entity has been given a dependency
    /// on an unknown entity type
    /// requested
    /// </summary>
    public class EntityDependencyException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public EntityDependencyException()
            : base()
        {
            
        }

        /// <summary>
        /// Construct with description message
        /// </summary>
        /// <param name="message">Description</param>
        public EntityDependencyException(string message)
            : base(message)
        {
            
        }

        /// <summary>
        /// Construct with description message and inner exception
        /// </summary>
        /// <param name="message">Description</param>
        /// <param name="innerException">Inner exception</param>
        public EntityDependencyException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }

        /// <summary>
        /// Construct with serialization information
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="ctx">Streaming context</param>
        public EntityDependencyException(SerializationInfo info, StreamingContext ctx)
            : base(info, ctx)
        {
            
        }
    }
}