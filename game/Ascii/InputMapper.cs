using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using game.AutoJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonWriter = game.AutoJson.JsonWriter;

namespace game.Ascii
{
    /// <summary>
    /// A particular key binding, containing a main key and a collection of
    /// additional modifier keys.
    /// </summary>
    [Deserializable]
    [AutoJson.Serializable]
    public class KeyBinding
    {
        /// <summary>
        /// The main key that this binding is set to
        /// </summary>
        [AutoJson.Key("main_key")]
        [AutoJson.Required]
        public Key MainKey
        {
            get;
            set;
        }

        /// <summary>
        /// Modifier keys that need to be pressed in addition to the main key
        /// </summary>
        [AutoJson.Key("modifiers")]
        public List<Key> Modifiers
        {
            get;
            set;
        } = new List<Key>();

        /// <summary>
        /// Construct new key binding instance from given keys.
        /// </summary>
        /// <param name="key">Main key</param>
        /// <param name="modifiers">Modifier keys</param>
        public KeyBinding(Key key, List<Key> modifiers)
        {
            MainKey = key;
            Modifiers = modifiers;
        }
        
        /// <summary>
        /// Construct new key binding instance from given key with no modifiers.
        /// </summary>
        /// <param name="key">Main key</param>
        public KeyBinding(Key key)
        {
            MainKey = key;
        }

        /// <summary>
        /// Parameterless default constructor
        /// </summary>
        public KeyBinding()
        {
            
        }

        protected bool Equals(KeyBinding other)
        {
            return MainKey == other.MainKey && Equals(Modifiers, other.Modifiers);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((KeyBinding) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) MainKey * 397) ^ (Modifiers != null ? Modifiers.GetHashCode() : 0);
            }
        }

        public static bool operator ==(KeyBinding left, KeyBinding right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(KeyBinding left, KeyBinding right)
        {
            return !Equals(left, right);
        }
    }

    /// <summary>
    /// A class representing an entry in a key map schema. It contains name, description and default
    /// key binding information.
    /// </summary>
    [Deserializable]
    public class KeySchemaEntry
    {
        /// <summary>
        /// The unique ID of this schema entry, for example "MOVE_UP". This is also used
        /// when using automatic enumeration conversion.
        /// </summary>
        [AutoJson.Key("id")]
        [AutoJson.Required]
        public string Identifier
        {
            get;
            protected set;
        }
        
        /// <summary>
        /// The name of this schema entry, for example "Move Up"
        /// </summary>
        [AutoJson.Key("name")]
        [AutoJson.Required]
        public string Name
        {
            get;
            protected set;
        }

        /// <summary>
        /// The description of this schema entry, for example "Move character up"
        /// </summary>
        [AutoJson.Key("description")]
        [AutoJson.Required]
        public string Description
        {
            get;
            protected set;
        }

        /// <summary>
        /// The default key binding. This is used if no user override was supplied.
        /// </summary>
        [AutoJson.Key("default_binding")]
        [AutoJson.Required]
        public KeyBinding DefaultBinding
        {
            get;
            protected set;
        }

        /// <summary>
        /// Parameterless default constructor
        /// </summary>
        public KeySchemaEntry()
        {
            
        }
    }
    
    /// <summary>
    /// A class that allows the user to decouple input actions from the actual key input, thus
    /// allowing key rebindings to be used.
    /// </summary>
    public class InputMapper
    {
        /// <summary>
        /// The immutable schema of this keymap. The default bindings can be overridden by the user.
        /// </summary>
        public ReadOnlyDictionary<string, KeySchemaEntry> Schema
        {
            get;
            protected set;
        }

        /// <summary>
        /// A collection of key bindings that override the default bindings found in the key map schema.
        /// The entries are read from a user-supplied file, if it exists.
        /// </summary>
        public Dictionary<string, KeyBinding> OverrideBindings
        {
            get;
        } = new Dictionary<string, KeyBinding>();
        
        /// <summary>
        /// Unique identifier of this key map. This is used to find the schema file in the asset file system.
        /// </summary>
        public string Identifier
        {
            get;
        }

        /// <summary>
        /// Compute the path to the potentially existing override file for this particular key map.
        /// </summary>
        protected string OverrideFilePath =>
            Path.Combine(Paths.UserDirectory, "keybindings", Path.ChangeExtension(Identifier, "json"));
        
        /// <summary>
        /// Compute the path to the schema file.
        /// </summary>
        protected string SchemaFilePath =>
            Path.Combine(Paths.DataDirectory, "keybindings", Path.ChangeExtension(Identifier, "json"));

        /// <summary>
        /// Construct input mapping with given unique identifier.
        /// This causes the schema to be loaded from the asset file tree. If a matching override file exists,
        /// it is also loaded and applied.
        /// </summary>
        /// <param name="identifier">Unique identifier associated with the key maps</param>
        public InputMapper(string identifier)
        {
            Identifier = identifier;
            LoadSchema();
            LoadOverrides();        
        }

        /// <summary>
        /// Check if the keys set by given key binding are currently pressed
        /// </summary>
        /// <param name="kb">Key binding to use as a source of keys</param>
        /// <returns>Flag indicating if the keys are pressed</returns>
        protected bool HasInput(KeyBinding kb)
        {
            if (!Input.hasKey(kb.MainKey))
                return false;

            return kb.Modifiers.All(Input.hasKey);
        }

        /// <summary>
        /// Check if the keys associated with given input action are currently pressed.
        /// </summary>
        /// <param name="input">Input action to use</param>
        /// <returns>Flag indicating if the keys associated with given input action are pressed</returns>
        /// <exception cref="ArgumentException">If the given input action is not known to this mapping</exception>
        public bool HasInput(string input)
        {
            if (OverrideBindings.ContainsKey(input))
                return HasInput(OverrideBindings[input]);
            else if (Schema.ContainsKey(input))
                return HasInput(Schema[input].DefaultBinding);
            else throw new ArgumentException("Unknown input action \"{0}\"", input);
        }

        /// <summary>
        /// Check if the keys associated with the input action associated with given enum value are currently pressed.
        /// This converts the name of the enumeration to string, and uses that as input action identifier.
        /// </summary>
        /// <param name="value">Enumeration value to use</param>
        /// <returns>Flag indicating if the keys associated with the input action corresponding to given enumeration value are currently pressed</returns>
        public bool HasInput(Enum value)
        {
            return HasInput(value.ToString());
        }

        /// <summary>
        /// Update a key binding
        /// </summary>
        /// <param name="input">Input action identifier to set key binding for</param>
        /// <param name="kb">Key binding information</param>
        /// <exception cref="ArgumentException">If the given input action is not known to this mapping</exception>
        public void SetBinding(string input, KeyBinding kb)
        {
            if(!Schema.ContainsKey(input))
                throw new ArgumentException("Unknown input action \"{0}\"", input);
            
            if (OverrideBindings.ContainsKey(input))
                OverrideBindings[input] = kb;
            else
                OverrideBindings.Add(input, kb);       
        }

        /// <summary>
        /// Update a key binding
        /// </summary>
        /// <param name="value">Input action identifier to set key binding for</param>
        /// <param name="kb">Key binding information</param>
        /// <exception cref="ArgumentException">If the given input action is not known to this mapping</exception>
        public void SetBinding(Enum value, KeyBinding kb)
        {
            SetBinding(value.ToString(), kb);
        }
        
        /// <summary>
        /// Save all changes made to the bindings to the override key map file. This method
        /// automatically detects which bindings are redundant and ignores them.
        /// </summary>
        public void SaveChanges()
        {
            // Extract parent directory path from override file path
            var parentDirectory = Path.GetDirectoryName(OverrideFilePath);
            
            // Create directory if not already exists
            Directory.CreateDirectory(parentDirectory);
            
            // Determine all binding entries that are actually different from the
            // defaults stored in the schema
            var bindings = OverrideBindings.Where(kvp => kvp.Value != Schema[kvp.Key].DefaultBinding);
            
            // Build override JSON document in memory
            var parent = new JObject();

            // Serialize them all
            foreach (var kvp in bindings)
            {
                parent.Add(kvp.Key, JsonWriter.Serialize(kvp.Value));
            }
            
            // Create (or override) file and save JSON document
            File.WriteAllText(OverrideFilePath, parent.ToString());
        }

        /// <summary>
        /// Load the schema from JSON document
        /// </summary>
        /// <exception cref="ArgumentException">If the schema file does not exist or is otherwise incorrect</exception>
        protected void LoadSchema()
        {
            // Check if the schema file actually exists
            if (!File.Exists(SchemaFilePath))
            {
                Logger.PostMessageTagged(SeverityLevel.Fatal, "InputMapper", String.Format("Schema file for input mapping \"{0}\" does not exist!", Identifier));
                throw new ArgumentException("Input mapping schema file not found");
            }
            
            try
            {
                // Parse schema file as JSON object
                using(var fileReader = File.OpenText(SchemaFilePath))
                using (var jsonReader = new JsonTextReader(fileReader))
                {
                    // Since the readonly dictionary does not support insertion, we need to collect our schema data in a temporary
                    // mutable dictionary and later assign it to the property
                    var temporarySchema = new Dictionary<string, KeySchemaEntry>();
                    
                    var array = JToken.ReadFrom(jsonReader) as JArray;
                    
                    if(array == null)
                        throw new ArgumentException("Failed to load top level array from schema file");
                    
                    // Parse each schema entry
                    foreach (var entry in array)
                    {
                        // All entries need to be objects
                        if (entry.Type != JTokenType.Object)
                        {
                            Logger.PostMessageTagged(SeverityLevel.Fatal, "InputMapper", String.Format("Expected JSON object in schema definition, got \"{0}\" instead", entry.Type.ToString()));
                            throw new ArgumentException("Expected JSON object in schema definition");                               
                        }

                        var schemaEntry = JsonLoader.Deserialize<KeySchemaEntry>(entry as JObject);
                        
                        temporarySchema.Add(schemaEntry.Identifier, schemaEntry);
                    }
                    
                    Schema = new ReadOnlyDictionary<string, KeySchemaEntry>(temporarySchema);
                }
            }
            catch (Exception e)
            {
                Logger.PostMessageTagged(SeverityLevel.Fatal, "InputMapper", String.Format("Failed to load input mapping schema file: {0}", e.Message));
                throw;
            }  
        }

        /// <summary>
        /// Load the key map override file if it exists.
        /// </summary>
        /// <exception cref="ArgumentException">If the key map override file is in invalid format</exception>
        protected void LoadOverrides()
        {
            // Only load overrides if the file actually exists
            if (!File.Exists(OverrideFilePath))
                return;
            
            try
            {
                // Parse schema file as JSON object
                using(var fileReader = File.OpenText(OverrideFilePath))
                using (var jsonReader = new JsonTextReader(fileReader))
                {       
                    var array = JToken.ReadFrom(jsonReader) as JObject;
                    
                    if(array == null)
                        throw new ArgumentException("Failed to load top level object from key binding override file");
                    
                    // Parse each key binding
                    foreach (var entry in array)
                    {
                        // All entries need to be objects
                        if (entry.Value.Type != JTokenType.Object)
                        {
                            Logger.PostMessageTagged(SeverityLevel.Fatal, "InputMapper", String.Format("Expected JSON object in key binding override file, got \"{0}\" instead", entry.Value.Type.ToString()));
                            throw new ArgumentException("Expected JSON object in schema definition");                               
                        }

                        // Check that a schema entry actually exists for given identifier
                        if (!Schema.ContainsKey(entry.Key))
                        {
                            Logger.PostMessageTagged(SeverityLevel.Fatal, "InputMapper", String.Format("Unknown action identifier \"{0}\" in override file for key map \"{1}\"", entry.Key, Identifier));
                            throw new ArgumentException("Unknown action identifier in override file");
                        }
                        
                        var binding = JsonLoader.Deserialize<KeyBinding>(entry.Value as JObject);
                        
                        OverrideBindings.Add(entry.Key, binding);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.PostMessageTagged(SeverityLevel.Fatal, "InputMapper", String.Format("Failed to load input mapping schema file: {0}", e.Message));
                throw;
            }  
        }
    }
}