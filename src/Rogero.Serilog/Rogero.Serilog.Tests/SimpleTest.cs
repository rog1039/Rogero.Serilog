using System;
using Rogero.Serilog.Enrichments;
using Rogero.Serilog.Sinks;
using Serilog;
using Xunit;

namespace Rogero.Serilog.Tests
{
    public class SimpleTest
    {
        [Fact()]
        [Trait("Category", "Instant")]
        public void WithNoConfigurators()
        {
            var configurator = new ConfigurableSerilogFactory(new JsonConsoleSinkConfigurator(), new ConsoleSinkConfigurator());
            var logger = configurator.Create();
            TestLog(logger);
        }

        [Fact()]
        [Trait("Category", "Instant")]
        public void WithBasicApplicationDetailsConfigurator()
        {
            var configurator =
                new ConfigurableSerilogFactory(new JsonConsoleSinkConfigurator(), new BasicApplicationDetailsConfigurator("Some App", "Dev"));
            var logger = configurator.Create();
            TestLog(logger);
        }

        [Fact()]
        [Trait("Category", "Instant")]
        public void WithMachineAndUserDetailsConfigurator()
        {
            var configurator =
                new ConfigurableSerilogFactory(new JsonConsoleSinkConfigurator(), new MachineAndUserDetailsConfigurator());
            var logger = configurator.Create();
            TestLog(logger);
        }

        [Fact()]
        [Trait("Category", "Instant")]
        public void WithPropertyEnricherConfigurator()
        {
            var configurator =
                new ConfigurableSerilogFactory(new JsonConsoleSinkConfigurator(), new PropertyEnricherConfigurator("Prop", new TestObject(), true));
            var logger = configurator.Create();
            TestLog(logger);
        }

        private static void TestLog(ILogger logger)
        {
            logger.Information("Test log message");
        }
    }
}
