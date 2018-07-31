using System;
using game.AutoJson;
using Newtonsoft.Json.Linq;

namespace game.EntityComponent.Components
{
    [Deserializable]
    public class LifetimeComponent : IComponent
    {
        public static string Identifier => "lifetime";

        public int CurrentLifetime
        {
            get;
            set;
        }

        [Key("maximum")]
        [Required]
        public int MaximumLifetime
        {
            get;
            protected set;
        }
        
        public Type ComponentType()
        {
            return typeof(LifetimeComponent);
        }

        public void Construct(JObject obj)
        {
            JsonLoader.Populate(this, obj);
        }
    }
}