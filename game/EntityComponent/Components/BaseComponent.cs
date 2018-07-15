using System;
using Newtonsoft.Json.Linq;

namespace game.EntityComponent.Components
{
    public abstract class BaseComponent : IComponent
    {
        public abstract Type ComponentType();
        public abstract void Construct(JObject obj);
    }
}