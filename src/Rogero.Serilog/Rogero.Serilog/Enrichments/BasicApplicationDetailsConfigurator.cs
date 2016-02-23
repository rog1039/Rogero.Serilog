using System;
using System.Diagnostics;
using Serilog;

namespace Rogero.Serilog.Enrichments
{
    public class BasicApplicationDetailsConfigurator : ISerilogConfigurator
    {
        private readonly string _applicationName;
        private readonly Guid _appInstanceGuid;
        private readonly string _environmentName;

        public BasicApplicationDetailsConfigurator(string applicationName, Guid appInstanceGuid, string environmentName)
        {
            _applicationName = applicationName;
            _appInstanceGuid = appInstanceGuid;
            _environmentName = environmentName;
        }

        public LoggerConfiguration Apply(LoggerConfiguration configuration)
        {
            return configuration
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationName", _applicationName)
                .Enrich.WithProperty("AppInstanceId", _appInstanceGuid)
                .Enrich.WithProperty("Environment", _environmentName)
                .Enrich.WithProperty("Debugging status", Debugger.IsAttached);
        }
    }
}