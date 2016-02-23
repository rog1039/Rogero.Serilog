using System;
using System.Linq;
using Newtonsoft.Json;
using Serilog.Core;
using Serilog.Events;

namespace Rogero.Serilog.Tests
{
    internal class JsonConsoleSink : ILogEventSink
    {
        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null)
                throw new ArgumentNullException("logEvent");
            var json = JsonConvert.SerializeObject(logEvent, Formatting.Indented);
            Console.Out.WriteLine(json);
        }
    }
}