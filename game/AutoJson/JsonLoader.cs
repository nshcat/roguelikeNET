using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using game.Ascii;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace game.AutoJson
{
    public static class JsonLoader
    {
        /// <summary>
        /// A mapping between type objects and callables that are able to extract a value of just that type
        /// from a JSON document token.
        /// </summary>
        private static Dictionary<Type, Func<JToken, object>> PrimitiveReaders { get; set; } =
            new Dictionary<Type, Func<JToken, object>>
            {
                [typeof(int)] = token => token.Value<int>(),
                [typeof(uint)] = token => token.Value<uint>(),
                [typeof(byte)] = token => token.Value<byte>(),
                [typeof(float)] = token => token.Value<float>(),
                [typeof(double)] = token => token.Value<double>(),             
                [typeof(string)] = token => token.Value<string>(),
                [typeof(long)] = token => token.Value<long>(),
                [typeof(ulong)] = token => token.Value<ulong>(),
                [typeof(short)] = token => token.Value<short>(),
                [typeof(ushort)] = token => token.Value<ushort>()
            };

        /// <summary>
        /// A mapping between type objects and the JSON token type that is required top extract a value
        /// of just that type.
        /// </summary>
        private static Dictionary<Type, JTokenType> PrimitiveTypeMap { get; set; } =
            new Dictionary<Type, JTokenType>
            {
                [typeof(int)] = JTokenType.Integer,
                [typeof(uint)] = JTokenType.Integer,
                [typeof(byte)] = JTokenType.Integer,
                [typeof(float)] = JTokenType.Float,
                [typeof(double)] = JTokenType.Float,
                [typeof(string)] = JTokenType.String,
                [typeof(long)] = JTokenType.Integer,
                [typeof(ulong)] = JTokenType.Integer,
                [typeof(short)] = JTokenType.Integer,
                [typeof(ushort)] = JTokenType.Integer
            };

        public static void Populate<T>(T instance, JObject o)
        {
            Deserialize(instance, typeof(T), o);
        }     
        
        /*public static void Populate<T>(T instance, JObject o, string key)
        {
            Deserialize<T>(instance, (JObject)o[key]);
        } */

        public static T Deserialize<T>(JObject o, string key)
        {
            return Deserialize<T>((JObject)o[key]);
        }
        
        public static T Deserialize<T>(JObject t)
        {
            if(typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null) == null && !typeof(T).IsValueType)
                throw new ArgumentException($"Type \"{typeof(T)}\"needs to provide a parameterless constructor in order to be deserialized");    
            
            return (T) Deserialize(ConstructEmpty(typeof(T)), typeof(T), t);
        }

        private static void CallPredeserializationMethods(object instance, Type type, JObject jobj)
        {
            foreach (var method in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if (Attribute.IsDefined(method, typeof(BeforeDeserialization)))
                {
                    // Call method with current JSON object, but only if signature is matching
                    if (method.GetParameters().Length != 1 || method.GetParameters()[0].ParameterType != typeof(JObject))
                    {
                        Logger.PostMessageTagged(SeverityLevel.Warning, "JsonLoader",
                            $"Method marked with \"BeforeDeserialization\" in \"{type}\" has invalid signature. Ignoring");
                    }
                    else
                    {
                        // Call it
                        method.Invoke(instance, new[]{jobj});
                    }                 
                }
            }
        }
        
        private static void CallPostdeserializationMethods(object instance, Type type, JObject jobj)
        {
            foreach (var method in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if (Attribute.IsDefined(method, typeof(AfterDeserialization)))
                {
                    // Call method with current JSON object, but only if signature is matching
                    if (method.GetParameters().Length != 1 || method.GetParameters()[0].ParameterType != typeof(JObject))
                    {
                        Logger.PostMessageTagged(SeverityLevel.Warning, "JsonLoader",
                            $"Method marked with \"AfterDeserialization\" in \"{type}\" has invalid signature. Ignoring");
                    }
                    else
                    {
                        // Call it
                        method.Invoke(instance, new[]{jobj});
                    }                 
                }
            }
        }

        private static object Deserialize(object instance, Type type, JObject t)
        { 
            // Create empty object TODO check for constructor without arguments first
            //var result = Activator.CreateInstance(type);
            
            // Check if the given type contains any methods marked with the "BeforeDeserialization" attribute,
            // and call them
            CallPredeserializationMethods(instance, type, t);
                 
            // Inspect all properties and fields of type T          
            foreach (var prop in type.GetProperties())
            {
                // Check if it has the "Key" property
                if(Attribute.IsDefined(prop, typeof(Key)))
                {
                    // Retrieve it
                    var keyAttr = (Key)prop.GetCustomAttributes(typeof(Key), false)[0];
                                            
                    // Check if it is there
                    if (!t.ContainsKey(keyAttr.Value))
                    {
                        // Is it required?
                        if(Attribute.IsDefined(prop, typeof(Required)))
                            throw new ArgumentException(String.Format("Expected required entry with key \"{0}\"", keyAttr.Value));
                        
                        // Is there a default value?
                        if (Attribute.IsDefined(prop, typeof(DefaultValue)))
                        {
                            prop.SetValue(instance, (prop.GetCustomAttributes(typeof(DefaultValue), false)[0] as DefaultValue).Value);
                        }
                        else
                        {
                            // Default initialize
                            prop.SetValue(instance, DefaultConstruct(prop.PropertyType));                            
                        }
                    }
                    else
                    {
                        // Read value from json object
                        var value = ReadValue(prop, t[keyAttr.Value]);

                        // Store it
                        prop.SetValue(instance, value);
                    }                   
                }
            }

            // Check if the given type contains any methods marked with the "AfterDeserialization" attribute,
            // and call them
            CallPostdeserializationMethods(instance, type, t);
            
            return instance;
        }

        public static List<T> DeserializeMany<T>(JObject obj)
        {
            throw new NotImplementedException();
        }


        private static object DefaultConstruct(Type t)
        {
            // Strings do not have an empty constructor
            if (t == typeof(string))
                return string.Empty;
            else return ConstructEmpty(t);
        }
        
        private static object ReadValue(PropertyInfo p, JToken t)
        {
            // Retrieve underlying type
            var type = p.PropertyType;

            // Multiple values
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                // Retrieve element type
                var elemType = type.GetGenericArguments()[0];
                
                var list = (IList) Activator.CreateInstance(type);
                
                if(t.Type != JTokenType.Array)
                    throw new ArgumentException("Given JSON object does not contain array");

                var obj = (JArray) t;

                foreach (var element in obj)
                {
                    list.Add(ReadSingleValue(elemType, element));
                }

                return list;
            }
            else
            {
                return ReadSingleValue(type, t);
            }
        }

        private static object ReadSingleValue(Type t, JToken elem)
        {
            // Subobject
            if (Attribute.IsDefined(t, typeof(Deserializable)))
            {
                // Token needs to be object
                if(elem.Type != JTokenType.Object)
                    throw new ArgumentException(string.Format("JToken needs to be object to allow deserialization of \"{0}\"", t));
                
                return Deserialize(ConstructEmpty(t), t, (JObject)elem);
            }
            else if(t.IsEnum) // Enumeration
            {
                // Retrieve as string value
                if(elem.Type != JTokenType.String)
                    throw new ArgumentException("Expected a string value in order to deserialize enumeration");

                return Enum.Parse(t, (elem as JValue).Value<string>());
            }
            else
            {
                return ReadPrimitiveType(t, elem);
            }
        }

        /// <summary>
        /// Read primitive value (like string, int, float..) from given JSON document token.
        /// </summary>
        /// <param name="t">Type of value to read</param>
        /// <param name="elem">Source JSON document token</param>
        /// <returns>Deserialized value</returns>
        private static object ReadPrimitiveType(Type t, JToken elem)
        {
            // Check if requested type is supported
            if (!PrimitiveReaders.ContainsKey(t))
            {
                Logger.PostMessageTagged(SeverityLevel.Fatal, "JsonLoader", $"Encountered unsupported type {t}");
                throw new ArgumentException($"Encountered unsupported type {t}");
            }

            // Check if the token type matches with the expected type
            var expectedType = PrimitiveTypeMap[t];
            if (elem.Type != expectedType)
            {
                Logger.PostMessageTagged(SeverityLevel.Fatal, "JsonLoader",
                    $"Encountered JSON token type mismatch: Expected {elem.Type}, but got {expectedType}");
                throw new ArgumentException(
                    $"Encountered JSON token type mismatch: Expected {elem.Type}, but got {expectedType}");
            }

            // Retrieve registered reader callable and process input
            return PrimitiveReaders[t].Invoke(elem);
        }

        /// <summary>
        /// Construct new instance of given type, as if the parameterless constructor was called.
        /// </summary>
        /// <param name="t">Type of the object to construct</param>
        /// <returns>Constructed object</returns>
        private static object ConstructEmpty(Type t)
        {
            // Value types do not offer parameterless constructors
            if (t.IsValueType && !t.IsEnum)
            {
                return FormatterServices.GetUninitializedObject(t);
            }
            else
            {
                return Activator.CreateInstance(t, true);
            }
        }
    }
}