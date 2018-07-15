using System;
using game.AutoJson;
using Newtonsoft.Json.Linq;

namespace game.EntityComponent.Components
{
    [Deserializable]
    public class TestComponent1 : IComponent
    {
        public static string Identifier => "test_component_1";

        [Key("test_data")]     
        public int TestData
        {
            get;
            set;
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