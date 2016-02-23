using Serilog;

namespace Rogero.Serilog
{
    public interface ISerilogConfigurator
    {
        LoggerConfiguration Apply(LoggerConfiguration configuration);
    }
}