using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace game.AutoJson
{
    public static class JsonWriter
    {
        public static JObject Serialize<T>(T element)
        {
            return SerializeImpl(typeof(T), element);
        }

        private static JObject SerializeImpl(Type elementType, object source)
        {
            // Check if T is actually a serializable type
            if(!Attribute.IsDefined(elementType, typeof(Serializable)))
                throw new InvalidOperationException("Given type is not marked as serializable!");         
            
            // Create empty JSON object to store child elements
            var dest = new JObject();
            
            // Find all properties that have a Key attribute
            foreach (var prop in elementType.GetProperties())
            {
                if(!Attribute.IsDefined(prop, typeof(Key)))
                    continue;
                       
                // Create empty JToken reference for later use
                JToken element;
                
                // Retrieve key attribute identifier 
                var keyAttr = (Key) prop.GetCustomAttributes(typeof(Key), false)[0];
                var key = keyAttr.Value;
                
                // Retrieve property value type
                var propType = prop.PropertyType;

                element = WriteValue(propType, prop.GetValue(source));
       
                // Store JToken in object using the key
                dest.Add(key, element);
            }

            return dest;
        }

        private static JToken WriteValue(Type type, object source)
        {
            // Check if the type is a collection. If so, a JArray needs
            // to be used.
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                // Retrieve type of stored element
                var elementType = type.GetGenericArguments()[0];
                
                // Create new JSON array
                var array = new JArray();
                
                // Interpret source object instance as IList in order to access the elements
                var list = source as IList;

                // Try to parse each element in the list
                foreach (var entry in list)
                {
                    array.Add(WriteValue(elementType, entry));
                }

                return array;
            }
            else if(Attribute.IsDefined(type, typeof(Serializable))) // Subobject
            {
                return SerializeImpl(type, source);
            }
            else if (type.IsEnum) // Enumeration
            {
                // Convert to string and save
                var str = Enum.GetName(type, source);
                return new JValue(str);
            }
            else
            {
                // TODO solve this better: dictionary of converter instances  
                // TODO can one just set JValue.Value to a object?
                if (type == typeof(string))
                {
                    return new JValue(source as string);
                }
                else if (type == typeof(int))
                {
                    return new JValue((int)source);
                }
                
                throw new NotImplementedException();
            }     
        }
    }
}