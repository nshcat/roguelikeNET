using System.Collections.Generic;

namespace game
{
    [AutoJson.Deserializable]
    public class Test
    {     
        [AutoJson.Key("meow")]
        [AutoJson.DefaultValue(1337)]
        public int Meow
        {
            get;
            set;
        }

        [AutoJson.Key("nyan")]
        [AutoJson.Required]
        public string Nyan
        {
            get;
            protected set;
        }
        
        [AutoJson.Key("bar")]
        [AutoJson.Required]
        public List<string> Bar
        {
            get;
            set;
        }
        
        [AutoJson.Key("foo")]
        [AutoJson.Required]
        public Foobar Foo
        {
            get;
            set;
        }
    }
    
    [AutoJson.Deserializable]
    public class Foobar
    {     
        [AutoJson.Key("chirp")]
        [AutoJson.DefaultValue(42)]
        public int Chirp
        {
            get;
            set;
        }
    }
}