using System;
using Newtonsoft.Json.Linq;

namespace game.EntityComponent.Components
{
    public class DerivedComponent2 : BaseComponent
    {
        public static string Identifier => "derived_component2";
        
        public override Type ComponentType()
        {
            return typeof(DerivedComponent2);
        }

        public override void Construct(JObject obj)
        {
            
        }
    }
}