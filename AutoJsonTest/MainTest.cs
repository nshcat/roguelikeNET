using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using game.AutoJson;
using Newtonsoft.Json.Linq;

namespace AutoJsonTest
{
    [Deserializable]
    public class MethodTest
    {
        [Key("mrew")]
        public int Mrew
        {
            get;
            set;
        }

        public bool AfterTriggered = false;
        public bool BeforeTriggered = false;

        [AfterDeserialization]
        private void AfterDeserialization(JObject o)
        {
            AfterTriggered = true;
        }
        
        [BeforeDeserialization]
        private void BeforeDeserialization(JObject o)
        {
            BeforeTriggered = true;
        }
    }
    
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

    public enum TestEnum
    {
        Miau,
        Nyan
    }

    [game.AutoJson.Serializable]
    [Deserializable]
    public class Test3
    {
        public Test3(int nyan)
        {
            Nyan = nyan;
        }

        public Test3()
        {
            
        }

        [Key("nyan")]
        public int Nyan { get; set; }

        protected bool Equals(Test3 other)
        {
            return Nyan == other.Nyan;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Test3) obj);
        }

        public override int GetHashCode()
        {
            return Nyan;
        }

        public static bool operator ==(Test3 left, Test3 right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Test3 left, Test3 right)
        {
            return !Equals(left, right);
        }
    }
    
    [game.AutoJson.Serializable]
    [Deserializable]
    public class Test2
    {
        [Key("foo")]
        public TestEnum Foo { get; set; }
        
        [Key("bar")]
        public int Bar { get; set; }
        
        [Key("kitten")]
        public Test3 Kitten { get; set; }

        public Test2()
        {
            
        }

        public Test2(TestEnum foo, int bar, Test3 kitten)
        {
            Foo = foo;
            Bar = bar;
            Kitten = kitten;
        }

        protected bool Equals(Test2 other)
        {
            return Foo == other.Foo && Bar == other.Bar && Equals(Kitten, other.Kitten);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Test2) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Foo;
                hashCode = (hashCode * 397) ^ Bar;
                hashCode = (hashCode * 397) ^ (Kitten != null ? Kitten.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Test2 left, Test2 right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Test2 left, Test2 right)
        {
            return !Equals(left, right);
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

    [Deserializable]
    public class Bar
    {
        [Key("chirp")]
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

        [Fact]
        public void TestPreAndPost()
        {
            var src = "{ \"mrew\" : 42 }";

            var instance = JsonLoader.Deserialize<MethodTest>(JObject.Parse(src));
            
            Assert.True(instance.BeforeTriggered);
            Assert.True(instance.AfterTriggered);    
        }
        
        [Fact]
        public void TestPopulate()
        {
            var src = "{ \"chirp\" : 1337 }";
            
            var instance = new Bar();

            Populate(instance, src);
            
            Assert.Equal(1337, instance.Chirp);
        }

        [Fact]
        public void TestSerialize()
        {
            var element = new Test2(TestEnum.Nyan, 1337, new Test3(42));

            var json = JsonWriter.Serialize(element);

            var result = JsonLoader.Deserialize<Test2>(json);
            
            Console.WriteLine(json.ToString());
            
            Assert.Equal(element, result);
            
            File.WriteAllText("test_out", json.ToString());
        }

        private void Populate<T>(T instance, string src)
        {
            JsonLoader.Populate<T>(instance, JObject.Parse(src));
        }
        
        private Test Deserialize(string src)
        {
            return JsonLoader.Deserialize<Test>(JObject.Parse(src));
        }
    }
}