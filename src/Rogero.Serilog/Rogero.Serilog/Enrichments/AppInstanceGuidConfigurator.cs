using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace Rogero.Serilog.Enrichments
{
    public class AppInstanceGuidConfigurator : ISerilogConfigurator
    {
        public Guid AppInstanceGuid { get; set; } = Guid.NewGuid();

        public AppInstanceGuidConfigurator()
        {
        }

        public AppInstanceGuidConfigurator(Guid appInstanceGuid)
        {
            AppInstanceGuid = appInstanceGuid;
        }

        public LoggerConfiguration Apply(LoggerConfiguration configuration)
        {
            return configuration.Enrich.WithProperty("AppInstanceGuid", AppInstanceGuid);
        }
    }
}
