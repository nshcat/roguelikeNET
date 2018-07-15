using System;
using game.AutoJson;
using Newtonsoft.Json.Linq;

namespace game.EntityComponent.Components
{
    [Deserializable]
    public abstract class ComponentBase : IComponent
    {
        [Key("meow")]
        public int Meow
        {
            get;
            set;
        }   
        
        public abstract Type ComponentType();

        public virtual void Construct(JObject obj)
        {
            JsonLoader.Populate(this, obj);
        }   
    }
}