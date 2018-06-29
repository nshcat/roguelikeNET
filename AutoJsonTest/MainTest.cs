using System;
using System.Collections.Generic;
using Xunit;
using game.AutoJson;
using Newtonsoft.Json.Linq;

namespace AutoJsonTest
{
    [Deserializable]
    public class Test
    {     
        [Key("meow")]
        [DefaultValue(1337)]
        public int Meow
        {
            get;
            set;
        }

        [Key("nyan")]
        public string Nyan
        {
            get;
            protected set;
        }
        
        [Key("woof")]
        public int Woof
        {
            get;
            protected set;
        }
        
        [Key("bar")]
        [Required]
        public List<string> Bar
        {
            get;
            set;
        }
        
        [Key("foo")]
        [Required]
        public Foobar Foo
        {
            get;
            set;
        }
    }
    
    [Deserializable]
    public class Foobar
    {     
        [Key("chirp")]
        [DefaultValue(42)]
        public int Chirp
        {
            get;
            set;
        }
    }
    
    
    public class MainTest
    {
        // TODO: test lists, base types, sub objects..
        
        
        /// <summary>
        /// Test correct interpretation of required values
        /// </summary>
        [Fact]
        public void TestRequiredValue()
        {
            Assert.Throws<ArgumentException>(() => { Deserialize("{ }"); });
        }
        
        
        /// <summary>
        /// Test correct interpretation of optional values. This includes default construction and default values.
        /// </summary>
        [Fact]
        public void TestOptionalValue()
        {
            var src =
                "{ \"bar\" : [ \"first\", \"second\", \"third\" ], \"foo\" : { \"chirp\" : 42 } }";

            Assert.Equal("", Deserialize(src).Nyan);
            Assert.Equal(0, Deserialize(src).Woof);
            Assert.Equal(1337, Deserialize(src).Meow);
        }

        
        private Test Deserialize(string src)
        {
            return JsonLoader.Deserialize<Test>(JObject.Parse(src));
        }
    }
}