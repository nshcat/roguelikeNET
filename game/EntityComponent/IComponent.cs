using System;
using Newtonsoft.Json.Linq;

namespace game.EntityComponent
{
    /// <summary>
    /// The base interface for all components.
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// Retrieve textual identifier uniquely identifying this component type
        /// </summary>
        /// <returns>A string uniquely identifying this type of component</returns>
        string Identifier();

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