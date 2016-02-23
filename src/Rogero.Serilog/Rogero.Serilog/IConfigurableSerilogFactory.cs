using Serilog;

namespace Rogero.Serilog
{
    public interface IConfigurableSerilogFactory
    {
        ILogger Create();
    }
}