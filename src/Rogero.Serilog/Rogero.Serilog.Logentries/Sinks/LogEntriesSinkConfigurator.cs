using Serilog;

namespace Rogero.Serilog.Logentries.Sinks
{
    public class LogEntriesSinkConfigurator : ISerilogConfigurator
    {
        public string LogentriesToken { get; set; }

        public LogEntriesSinkConfigurator(string logentriesToken)
        {
            LogentriesToken = logentriesToken;
        }

        public LoggerConfiguration Apply(LoggerConfiguration configuration)
        {
            configuration.WriteTo.Logentries(LogentriesToken);
            return configuration;
        }
    }
}