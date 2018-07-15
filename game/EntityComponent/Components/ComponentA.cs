using System;
using game.AutoJson;
using Newtonsoft.Json.Linq;

namespace game.EntityComponent.Components
{
    [Deserializable]
    public class ComponentA : ComponentBase
    {
        public static string Identifier => "component_A";
        
        [Key("nyan")]
        public int Nyan
        {
            get;
            set;
        }

        public override Type ComponentType()
        {
            return typeof(ComponentA);
        }
        
        public override void Construct(JObject obj)
        {
            base.Construct(obj);
            JsonLoader.Populate(this, obj);
        }  
    }
}