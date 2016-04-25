using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rogero.Serilog.Serialization;
using Serilog;
using Xunit;

namespace Rogero.Serilog.Tests
{
    public class LoadJsonSampleConfiguration
    {
        [Fact()]
        [Trait("Category", "Instant")]
        public void LoadJson()
        {
            var logger = CreateLogger();
        }

        private static ILogger CreateLogger()
        {
            var loggerFactory = ConfigurableSerilogFactorySerializer.LoadFromJsonFile("sampleConfigurableSerilogFactory.json");
            return loggerFactory.Create();
        }
    }
}
