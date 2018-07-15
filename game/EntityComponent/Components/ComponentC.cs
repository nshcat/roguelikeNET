using System;
using game.AutoJson;
using Newtonsoft.Json.Linq;

namespace game.EntityComponent.Components
{
    [Deserializable]
    public class ComponentC : ComponentBase
    {
        public new static string Identifier => "component_C";
        
        [Key("woof")]
        public int Woof
        {
            get;
            set;
        }

        public override Type ComponentType()
        {
            return typeof(ComponentC);
        }
        
        public override void Construct(JObject obj)
        {
            base.Construct(obj);
            JsonLoader.Populate(this, obj);
        }  
    }
}