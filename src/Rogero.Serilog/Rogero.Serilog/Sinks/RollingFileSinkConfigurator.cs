using System;
using Serilog;
using Serilog.Events;

namespace Rogero.Serilog.Sinks
{
    public class RollingFileSinkConfigurator : ISerilogConfigurator
    {
        public string PathFormat { get; }
        public LogEventLevel RestrictedToMinimumLevel { get; }
        public string OutputTemplate { get; }
        public IFormatProvider FormatProvider { get; }
        public long? FileSizeLimitBytes { get; }
        public int? RetainedFileCountLimit { get; }

        public RollingFileSinkConfigurator(string pathFormat, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}", IFormatProvider formatProvider = null, long? fileSizeLimitBytes = 1073741824, int? retainedFileCountLimit = 31)
        {
            PathFormat = pathFormat;
            RestrictedToMinimumLevel = restrictedToMinimumLevel;
            OutputTemplate = outputTemplate;
            FormatProvider = formatProvider;
            FileSizeLimitBytes = fileSizeLimitBytes;
            RetainedFileCountLimit = retainedFileCountLimit;
        }

        public LoggerConfiguration Apply(LoggerConfiguration configuration)
        {
            return configuration.WriteTo.RollingFile(PathFormat,
                                                     RestrictedToMinimumLevel,
                                                     OutputTemplate,
                                                     FormatProvider,
                                                     FileSizeLimitBytes,
                                                     RetainedFileCountLimit);
        }
    }
}