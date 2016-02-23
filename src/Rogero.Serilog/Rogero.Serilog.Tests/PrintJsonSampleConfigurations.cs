using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Rogero.Serilog.ElasticSearchSink.Sinks;
using Rogero.Serilog.Enrichments;
using Rogero.Serilog.Serialization;
using Rogero.Serilog.Sinks;
using Xunit;

namespace Rogero.Serilog.Tests
{
    public class PrintJsonSampleConfigurations
    {
        public IList<Type> IgnoredConfigurators { get; set; } = new List<Type>()
        {
            typeof(JsonConsoleSinkConfigurator)
        };

        [Fact()]
        [Trait("Category", "Instant")]
        public void PrintJson()
        {
            var factory = new ConfigurableSerilogFactory(
                new AppInstanceGuidConfigurator(),
                new BasicApplicationDetailsConfigurator("MyApp", "EnvironmentName"),
                new MachineAndUserDetailsConfigurator(),
                new PropertyEnricherConfigurator("SomeProp", "SomeValue", false),
                new PropertyListEnricherConfigurator(new Dictionary<string, object>{ {"prop1", "val1"}, {"prop2",10 }},true),
                new ConsoleSinkConfigurator(),
                new RollingFileSinkConfigurator("logPrefix"),                
                new ElasticSearchSinkConfigurator(new []{ "http://elastic1:5601", "http://elastic1:5601", }, ElasticSearchIndexName.FromAppName("myApp"), TimeSpan.FromSeconds(1))
                );

            MakeSureAllConfiguratorsArePresentInThisSample(factory);
            var json = factory.WriteToJsonString();
            Console.WriteLine(json);

            EnsureRoundTrip(json);
        }

        private void EnsureRoundTrip(string json)
        {
            var config = json.LoadFromJsonText();
            var json2 = config.WriteToJsonString();
            json.Trim().Should().BeEquivalentTo(json2.Trim());
        }

        private void MakeSureAllConfiguratorsArePresentInThisSample(ConfigurableSerilogFactory factory)
        {
            var configurators = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(z => z.GetTypes())
                .Where(z => z.GetInterfaces().Contains(typeof(ISerilogConfigurator)))
                .Where(z => !IgnoredConfigurators.Contains(z))
                .ToList();
            var expectedCount = configurators.Count;

            factory.Configurators.Should().HaveCount(expectedCount);
        }
    }
}