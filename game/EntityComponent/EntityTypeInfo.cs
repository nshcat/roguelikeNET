using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace game.EntityComponent
{
    /// <summary>
    /// A class storing information about a particular entity type. This kind of acts
    /// like a schema.
    /// </summary>
    public class EntityTypeInfo
    {
        /// <summary>
        /// The name of this type.
        /// </summary>
        public string Name
        {
            get;
        }
        
        /// <summary>
        /// The components associated with entities of this type.
        /// </summary>
        public List<string> Components
        {
            get;
        } = new List<string>();

        /// <summary>
        /// Construct new type information from JSON document describing an entity type
        /// </summary>
        /// <param name="obj">JSON document to use as data source</param>
        public EntityTypeInfo(JObject obj)
        {
            // Ensure that a name is always supplied
            if(!obj.ContainsKey("name") || obj["name"].Type != JTokenType.String)
                throw new ArgumentException("Given entity type definition does not contain a name entry");

            // Retrieve entity type name
            Name = obj["name"].Value<string>();

            // Parse component declarations
            foreach (var token in obj)
            {
                // Check if current token is a subobject. If so, try to parse it as a component
                if (token.Value.Type == JTokenType.Object)
                {
                    // Check that component name is actually valid and references a known
                    // component
                    if(ComponentManager.IsComponentKnown(token.Key))
                        Components.Add(token.Key);
                    else throw new UnknownComponentException("No component known with id \"" + token.Key + "\"");
                }
            }
        }
        
        // TODO: filter method to get entity types that have certain components
    }
}