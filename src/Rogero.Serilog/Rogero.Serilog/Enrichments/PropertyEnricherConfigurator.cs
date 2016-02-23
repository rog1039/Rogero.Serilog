using System.Collections;
using Serilog;

namespace Rogero.Serilog.Enrichments
{
    public class PropertyEnricherConfigurator : ISerilogConfigurator
    {
        public string PropertyName { get; }
        public object Value { get; }
        public bool DestructureObjects { get; }

        public PropertyEnricherConfigurator(string propertyName, object value, bool destructureObjects)
        {
            PropertyName = propertyName;
            Value = value;
            DestructureObjects = destructureObjects;
        }

        public LoggerConfiguration Apply(LoggerConfiguration configuration)
        {
            return configuration.Enrich.WithProperty(PropertyName, Value, DestructureObjects);
            
        }
    }
}
