using System;
using game.AutoJson;
using Newtonsoft.Json.Linq;

namespace game.EntityComponent.Components
{
    [Deserializable]
    public class TestComponent3 : IComponent
    {
        public static string Identifier => "test_component_3";

        [Key("test_meow")]     
        public int TestMeow
        {
            get;
            set;
        }


        string IComponent.Identifier()
        {
            return Identifier;
        }

        public Type ComponentType()
        {
            return typeof(TestComponent1);
        }

        public void Construct(JObject obj)
        {
            JsonLoader.Populate(this, obj);
        }
    }
}