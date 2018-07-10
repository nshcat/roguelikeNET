using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;

namespace game.EntityComponent.Components
{
    public class TestComponent2 : IComponent
    {
        public static string Identifier => "test_component_2";


        string IComponent.Identifier()
        {
            return Identifier;
        }

        public Type ComponentType()
        {
            return typeof(TestComponent2);
        }

        public void Construct(JObject obj)
        {
            throw new NotImplementedException();
        }
    }
}