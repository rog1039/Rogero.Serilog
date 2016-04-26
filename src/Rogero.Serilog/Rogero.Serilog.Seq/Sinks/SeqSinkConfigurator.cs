using System.IO;
using Serilog;
using Serilog.Events;

namespace Rogero.Serilog.Seq.Sinks
{
    public class SeqSinkConfigurator : ISerilogConfigurator
    {
        public string SeqUrl { get; set; }
        public LogEventLevel LogEventLevel { get; set; } = LogEventLevel.Verbose;
        public string ApiKey { get; set; } = null;
        public bool UseLogFileBuffers { get; set; } = true;
        public string BufferBaseFileName { get; set; }

        public SeqSinkConfigurator(string seqUrl, LogEventLevel logEventLevel = LogEventLevel.Verbose, string apiKey = null, bool useLogFileBuffers = true, string bufferBaseFileName = "logsBufferBase")
        {
            SeqUrl = seqUrl;
            LogEventLevel = logEventLevel;
            ApiKey = apiKey;
            UseLogFileBuffers = useLogFileBuffers;
            BufferBaseFileName = CreateRootedBufferBaseFileName(bufferBaseFileName);
        }

        private string CreateRootedBufferBaseFileName(string bufferBaseFileName)
        {
            bufferBaseFileName = bufferBaseFileName ?? "logsBufferBase";
            if(!Path.IsPathRooted(bufferBaseFileName))
            {
                var rootedPath = ApplicationLocation.AppBase + bufferBaseFileName.TrimStart('/', '\\');
                return rootedPath;
            }
            else
            {
                return bufferBaseFileName;
            }
        }

        public LoggerConfiguration Apply(LoggerConfiguration configuration)
        {
            if (UseLogFileBuffers)
            {
                return configuration.WriteTo.Seq(SeqUrl, LogEventLevel, apiKey: ApiKey, bufferBaseFilename:BufferBaseFileName);
            }
            return configuration.WriteTo.Seq(SeqUrl, LogEventLevel, apiKey:ApiKey);
        }
    }
}