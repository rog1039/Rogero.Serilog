using System.IO;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;

namespace Rogero.Serilog.Serialization
{
    public static class ConfigurableSerilogFactorySerializer
    {
        public static JsonSerializerSettings JsonSerializerSettings { get; } = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.All,
            TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
        };

        public static ConfigurableSerilogFactory LoadFromJsonFile(string fileName)
        {
            var text = File.ReadAllText(fileName);
            return LoadFromJsonText(text);
        }

        public static ConfigurableSerilogFactory LoadFromJsonFileInAppRoot(string fileName)
        {
            fileName = $"{ApplicationLocation.AppBase}\\{fileName}";
            var text = File.ReadAllText(fileName);
            return LoadFromJsonText(text);
        }

        public static ConfigurableSerilogFactory LoadFromJsonText(string json)
        {
            var configurableSerilogFactory = JsonConvert.DeserializeObject<ConfigurableSerilogFactory>(json,
                                                                                                       JsonSerializerSettings);
            return configurableSerilogFactory;
        }

        public static void SaveToJsonFile(ConfigurableSerilogFactory factory, string outputFileName)
        {
            var json = WriteToJsonString(factory);
            File.WriteAllText(outputFileName, json);
        }

        public static string WriteToJsonString(ConfigurableSerilogFactory factory)
        {
            var json = JsonConvert.SerializeObject(factory, JsonSerializerSettings);
            return json;
        }
    }
}