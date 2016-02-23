using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Serilog;

namespace Rogero.Serilog
{
    public class ConfigurableSerilogFactory : IConfigurableSerilogFactory
    {
        public IReadOnlyList<ISerilogConfigurator> Configurators { get; set; }

        public ConfigurableSerilogFactory(IEnumerable<ISerilogConfigurator> configurators)
        {
            Configurators = new ReadOnlyCollection<ISerilogConfigurator>(configurators.ToList());
        }
        public ConfigurableSerilogFactory(params ISerilogConfigurator[] configurators) : this((IEnumerable<ISerilogConfigurator>)configurators) { }
        private ConfigurableSerilogFactory() { }

        public ILogger Create()
        {
            var finalConfiguration = Configurators
                .Aggregate(new LoggerConfiguration(), (config, configurator) => configurator.Apply(config));
            var logger = finalConfiguration.CreateLogger();
            return logger;
        }
    }
}
