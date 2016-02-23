using System.Collections.Generic;
using System.Linq;
using Serilog;

namespace Rogero.Serilog
{
    public class ConfigurableSerilogFactory : IConfigurableSerilogFactory
    {
        private readonly IList<ISerilogConfigurator> _configurators = new List<ISerilogConfigurator>();

        public ConfigurableSerilogFactory(IEnumerable<ISerilogConfigurator> configurators)
        {
            foreach (var serilogConfigurator in configurators)
            {
                _configurators.Add(serilogConfigurator);
            }
        }
        public ConfigurableSerilogFactory(params ISerilogConfigurator[] configurators) : this((IEnumerable<ISerilogConfigurator>)configurators) { }

        public ILogger Create()
        {
            var finalConfiguration = _configurators
                .Aggregate(new LoggerConfiguration(), (config, configurator) => configurator.Apply(config));
            var logger = finalConfiguration.CreateLogger();
            return logger;
        }
    }
}
