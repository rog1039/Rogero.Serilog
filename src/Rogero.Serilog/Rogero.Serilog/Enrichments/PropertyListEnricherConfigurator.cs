using System.Collections.Generic;
using Serilog;

namespace Rogero.Serilog.Enrichments
{
    public class PropertyListEnricherConfigurator : ISerilogConfigurator
    {
        public IDictionary<string, object> Properties { get; set; }
        public bool DestructureObjects { get; }

        public PropertyListEnricherConfigurator(IDictionary<string,object> properties, bool destructureObjects)
        {
            Properties = properties;
            DestructureObjects = destructureObjects;
        }

        public LoggerConfiguration Apply(LoggerConfiguration configuration)
        {
            foreach (var property in Properties)
            {
                configuration = configuration.Enrich.WithProperty(property.Key, property.Value, DestructureObjects);
            }
            return configuration;
        }
    }
}