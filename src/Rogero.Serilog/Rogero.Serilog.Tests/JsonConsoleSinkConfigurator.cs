using Serilog;

namespace Rogero.Serilog.Tests
{
    public class JsonConsoleSinkConfigurator : ISerilogConfigurator
    {
        public LoggerConfiguration Apply(LoggerConfiguration configuration)
        {
            return configuration.WriteTo.Sink(new JsonConsoleSink());
        }
    }
}