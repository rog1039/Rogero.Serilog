using System;
using System.Diagnostics;
using Serilog;

namespace Rogero.Serilog.Enrichments
{
    public class BasicApplicationDetailsConfigurator : ISerilogConfigurator
    {
        private readonly string _applicationName;
        private readonly string _environmentName;

        public BasicApplicationDetailsConfigurator(string applicationName, string environmentName)
        {
            _applicationName = applicationName;
            _environmentName = environmentName;
        }

        public LoggerConfiguration Apply(LoggerConfiguration configuration)
        {
            return configuration
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationName", _applicationName)
                .Enrich.WithProperty("Environment", _environmentName)
                .Enrich.WithProperty("Debugging status", Debugger.IsAttached);
        }
    }
}