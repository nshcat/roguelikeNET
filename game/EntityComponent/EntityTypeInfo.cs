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
        /// Whether this entity type represents a template or not. Template entity types
        /// cannot be constructed.
        /// </summary>
        public bool IsTemplate
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
        /// List of identifiers of other entities/templates that this entity type
        /// inherits components from
        /// </summary>
        public List<string> Bases
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
            
            // Retrieve template indicator, if any
            if (obj.ContainsKey("is_template"))
                IsTemplate = obj["is_template"].Value<bool>();
            
            // Parse inheritance list
            if (obj.ContainsKey("inherits_from") && obj["inherits_from"].Type == JTokenType.Array)
            {
                // Retrieve array
                var baseArray = obj["inherits_from"] as JArray;
                
                // Store all entries in the base type list. We cannot check if the supplies type identifiers
                // are actually valid at this time, since the referenced entity types might not have been loaded
                // yet. Therefore, any checks will be done after entity loading is completed.
                foreach (var entry in baseArray)
                {
                    if(entry.Type != JTokenType.String)
                        throw new ArgumentException(String.Format("Expected string identifier in \"inherits_from\" list of entity type \"{0}\""), Name);
                    
                    Bases.Add(entry.Value<string>());
                }
            }

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