using System;

namespace game.AutoJson
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class Deserializable : Attribute
    {
        
    }
    
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class Key : Attribute
    {
        public string Value
        {
            get;
        }

        public Key(string val)
        {
            Value = val;
        }
    }
    
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class Required : Attribute
    {
    }
    
    
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DefaultValue : Attribute
    {
        public object Value
        {
            get;
        }

        public DefaultValue(object val)
        {
            Value = val;
        }
    }
}