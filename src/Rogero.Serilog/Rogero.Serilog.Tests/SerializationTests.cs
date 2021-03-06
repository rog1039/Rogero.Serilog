﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Rogero.Serilog.Enrichments;
using Rogero.Serilog.Sinks;
using Xunit;

namespace Rogero.Serilog.Tests
{
    public class SerializationTests
    {
        private JsonSerializerSettings _jsonSettings;
        private ConfigurableSerilogFactory _factory;
        private HeadObject _headObject;

        public SerializationTests()
        {
            _headObject = new HeadObject() { Tomorrow = DateTime.UtcNow };
            _headObject.SubObjects.Add(new SubObjectA() { Name = "Hi" });
            _headObject.SubObjects.Add(new SubObjectB() { Color = "Blue" });

            _jsonSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
            };

            _factory = new ConfigurableSerilogFactory(
                new ConsoleSinkConfigurator(),
                new RollingFileSinkConfigurator("rootPath"),
                new MachineAndUserDetailsConfigurator(),
                new BasicApplicationDetailsConfigurator("SomeApp", "UnitTestingEnvironment"),
                new PropertyEnricherConfigurator("SomeProperty", "SomeValue", true));
        }

        [Fact()]
        [Trait("Category", "Instant")]
        public void TestJsonSerializationWithTyping()
        {
            var json1 = JsonConvert.SerializeObject(_headObject, _jsonSettings);
            Console.WriteLine(json1);

            var headReserialized = JsonConvert.DeserializeObject<HeadObject>(json1, _jsonSettings);
            var json2 = JsonConvert.SerializeObject(headReserialized, _jsonSettings);
            Console.WriteLine(json2);

            json1.Should().BeEquivalentTo(json2);
        }

        [Fact()]
        public void TestSerializationOfConfigurableSerilogFactory()
        {
            var json = JsonConvert.SerializeObject(_factory, _jsonSettings);
            Console.WriteLine(json);
        }

        [Fact()]
        [Trait("Category", "Instant")]
        public void TestDeserializationOfPropertiesToDefaults()
        {
            var incomingJson = File.ReadAllText("sampleConfigurableSerilogFactory.json");
            Console.WriteLine(incomingJson);
            var factory = JsonConvert.DeserializeObject<ConfigurableSerilogFactory>(incomingJson, _jsonSettings);

            var json = JsonConvert.SerializeObject(factory, _jsonSettings);
            Console.WriteLine(json);
        }

        [Fact()]
        [Trait("Category", "Instant")]
        public void DefaultConstructorParameters()
        {
            var json = "{ \"X\": 5 }";
            var obj = JsonConvert.DeserializeObject<TestObject2>(json);

            var serializedJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
            Console.WriteLine(serializedJson);
        }

        [Fact()]
        [Trait("Category", "Instant")]
        public void Test_CustomCreationConverters()
        {
            var json = "{ \"X\": 5 }";
            var obj = JsonConvert.DeserializeObject<TestObject2>(json, new TestObject2CustomCreationConverter());

            var serializedJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
            Console.WriteLine(serializedJson);
        }
    }

    public class TestObject2CustomCreationConverter : CustomCreationConverter<TestObject2>
    {
        public override bool CanWrite { get; } = false;

        public override TestObject2 Create(Type objectType)
        {
            return new TestObject2();
        }
    }

    public class TestObject2
    {
        public int X { get; }
        public int Y { get; set; }

        public TestObject2(int x = 10, int y = 20)
        {
            X = x;
            Y = y;
        }
    }

    public class HeadObject
    {
        public DateTime Tomorrow { get; set; }
        public IList<ISubObject> SubObjects { get; set; } = new List<ISubObject>();
    }

    public interface ISubObject
    {
        
    }

    public class SubObjectA : ISubObject
    {
        public string Name { get; set; }
    }

    public class SubObjectB : ISubObject
    {
        public string Color { get; set; }
    }
}
