using System;
using Serilog;
using Serilog.Events;

namespace Rogero.Serilog.Sinks
{
    public class ConsoleSinkConfigurator : ISerilogConfigurator
    {
        public LogEventLevel RestrictedToMinimumLevel { get; }
        public string OutputTemplate { get; }
        public IFormatProvider FormatProvider { get; }
        public ConsoleSinkConfigurator(LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}", IFormatProvider formatProvider = null)
        {
            RestrictedToMinimumLevel = restrictedToMinimumLevel;
            OutputTemplate = outputTemplate;
            FormatProvider = formatProvider;
        }
        
        public LoggerConfiguration Apply(LoggerConfiguration configuration)
        {
            return configuration.WriteTo.Console(RestrictedToMinimumLevel, OutputTemplate, FormatProvider);
        }
    }
}