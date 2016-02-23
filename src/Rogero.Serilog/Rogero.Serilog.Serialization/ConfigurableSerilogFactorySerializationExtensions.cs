using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogero.Serilog.Serialization
{
    public static class ConfigurableSerilogFactorySerializationExtensions
    {
        public static ConfigurableSerilogFactory LoadFromJsonFile(this string fileName)
        {
            return ConfigurableSerilogFactorySerializer.LoadFromJsonFile(fileName);
        }

        public static ConfigurableSerilogFactory LoadFromJsonText(this string json)
        {
            return ConfigurableSerilogFactorySerializer.LoadFromJsonText(json);
        }

        public static void SaveToJsonFile(this ConfigurableSerilogFactory factory, string outputFileName)
        {
            ConfigurableSerilogFactorySerializer.SaveToJsonFile(factory, outputFileName);
        }

        public static string WriteToJsonString(this ConfigurableSerilogFactory factory)
        {
            return ConfigurableSerilogFactorySerializer.WriteToJsonString(factory);
        }
    }
}
