using System;
using game.AutoJson;
using Newtonsoft.Json.Linq;

namespace game.EntityComponent.Components
{
    [Deserializable]
    public class ComponentB : ComponentBase
    {
        public static string Identifier => "component_B";
        
        [Key("nyuu")]
        public int Nyuu
        {
            get;
            set;
        }

        public override Type ComponentType()
        {
            return typeof(ComponentB);
        }
        
        public override void Construct(JObject obj)
        {
            base.Construct(obj);
            JsonLoader.Populate(this, obj);
        }  
    }
}