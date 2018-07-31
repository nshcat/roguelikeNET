using System;
using Newtonsoft.Json.Linq;

namespace game.EntityComponent
{
    /// <summary>
    /// The base interface for all components.
    /// </summary>
    /// <remarks>
    /// In addition to implementing all methods this interface defines, components
    /// need to define a static property called "Identifier" which stores the unique
    /// string identifier of that particular component type. This is important, since
    /// they otherwise can't be referenced from JSON entity type definitions.
    /// </remarks>
    public interface IComponent
    {
        /// <summary>
        /// Returns the type information of this component.
        /// </summary>
        /// <returns>Type information of the implementing class</returns>
        Type ComponentType();
        
        /// <summary>
        /// Populate empty component instance with data from a JSON document node.
        /// Calling this method more than once on any given instance will result in
        /// undefined behaviour.
        /// </summary>
        /// <param name="obj">JSON document node to use as data source</param>
        void Construct(JObject obj);
    }
}