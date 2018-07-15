using System;
using Newtonsoft.Json.Linq;

namespace game.EntityComponent.Components
{
    public class DerivedComponent : BaseComponent
    {
        public static string Identifier => "derived_component";
        
        public override Type ComponentType()
        {
            return typeof(DerivedComponent);
        }

        public override void Construct(JObject obj)
        {
            
        }
    }
}