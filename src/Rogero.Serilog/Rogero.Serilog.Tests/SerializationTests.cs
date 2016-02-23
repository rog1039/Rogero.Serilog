using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Rogero.Serilog.Tests
{
    public class SerializationTests
    {
        [Fact()]
        [Trait("Category", "Instant")]
        public void TestJsonSerialization()
        {
            var head = new HeadObject() {Tomorrow = DateTime.UtcNow};
            head.SubObjects.Add(new SubObjectA() {Name = "Hi"});
            head.SubObjects.Add(new SubObjectB() {Color = "Blue"});

            var json = JsonConvert.SerializeObject(head, Formatting.Indented);
            Console.WriteLine(json);

            var headDeserialized = JsonConvert.DeserializeObject<HeadObject>(json);

            var jsonDeserialized = JsonConvert.SerializeObject(headDeserialized, Formatting.Indented);
            Console.WriteLine(jsonDeserialized);
        }

        [Fact()]
        [Trait("Category", "Instant")]
        public void TestJsonSerializationWithTyping()
        {
            var head = new HeadObject() { Tomorrow = DateTime.UtcNow };
            head.SubObjects.Add(new SubObjectA() { Name = "Hi" });
            head.SubObjects.Add(new SubObjectB() { Color = "Blue" });

            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All
            };
            var json = JsonConvert.SerializeObject(head, settings);
            Console.WriteLine(json);

            var headDeserialized = JsonConvert.DeserializeObject<HeadObject>(json, settings);

            var jsonDeserialized = JsonConvert.SerializeObject(headDeserialized, settings);
            Console.WriteLine(jsonDeserialized);

            json.Should().BeEquivalentTo(jsonDeserialized);
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

    public class SerilogJsonConverter : Newtonsoft.Json.JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(HeadObject);
        }

        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var allSubclassesWithProperties = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(z => z.ExportedTypes)
                .Where(z => z.IsClass)
                .Where(z => z.GetInterfaces().Contains(typeof(ISubObject)))
                .Select(z => Tuple.Create(z, z.GetProperties()))
                .ToList();
            
            var obj = new HeadObject();
            var objProperties = typeof(HeadObject).GetProperties();
            var jo = JObject.Load(reader);
            foreach (var child in jo.Children())
            {
                Console.WriteLine(child.Path);
                Console.WriteLine(child.Path.GetMostRightAfter(".", true));
                var property = objProperties.Single(z => z.Name == child.Path.GetMostRightAfter(".",true));

                
            }
            foreach (var childObj in jo["SubObjects"])
            {
                var childProps = childObj.Children();
                Console.WriteLine(string.Join(",", childProps.Select(z => z.Path.GetMostRightAfter(".",false))));
                var childPropNames = childProps.Select(z => z.Path.GetMostRightAfter(".",false)).ToList();
                var likelyType = MatchTypeOnProperties(allSubclassesWithProperties, childPropNames);
                if (likelyType != null)
                {
                    var child = (ISubObject)childObj.ToObject(likelyType);
                    obj.SubObjects.Add(child);
                }
            }

            return obj;
        }

        private Type MatchTypeOnProperties(List<Tuple<Type, PropertyInfo[]>> allClasses, List<string> childPropNames)
        {
            foreach (var possibleClass in allClasses)
            {
                var propNames = possibleClass.Item2.Select(z => z.Name);
                if (childPropNames.All(z => propNames.Contains(z)))
                    return possibleClass.Item1;
            }
            return null;
        }

        public override bool CanWrite { get; } = false;

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }

    public static class StringExtensions
    {
        public static string GetMostRightAfter(this string s, string sep, bool returnWholeStringOnNoMatch)
        {
            var indexOf = s.LastIndexOf(sep);
            if (indexOf < 0)
            {
                return returnWholeStringOnNoMatch
                    ? s
                    : string.Empty;
            }

            var result = s.Substring(indexOf + sep.Length, s.Length - (indexOf + sep.Length));
            return result;
        }
    }
}
