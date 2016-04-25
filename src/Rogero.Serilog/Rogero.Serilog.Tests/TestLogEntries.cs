using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rogero.Serilog.Logentries.Sinks;
using Rogero.Serilog.Seq.Sinks;
using Serilog.Events;
using Xunit;

namespace Rogero.Serilog.Tests
{
    public class TestLogEntries
    {
        [Fact()]
        [Trait("Category", "Integration")]
        public void SendLogsToLogentries()
        {
            var factory = new ConfigurableSerilogFactory(
                new LogEntriesSinkConfigurator("76e38bd0-a878-4a1f-951d-03c704a5cf3b"));
            var logger = factory.Create().ForContext("SoemProperty", "SomeValue");
            logger.Information("Here is a test from a unit test.");
            Thread.Sleep(2000);
        }
    }

    public class TestSeq
    {
        [Fact()]
        [Trait("Category", "Integration")]
        public void SendLogsToSeqAndUseBufferBaseFileName()
        {
            var factory = new ConfigurableSerilogFactory(new SeqSinkConfigurator("http://ws2012r2seq:5341", LogEventLevel.Verbose, "", useLogFileBuffers:true));
            var logger = factory.Create()
                .ForContext("UniTest", "Rogero.Serilog.Tests.TestSeq.SendLogsToSeqAndUseBufferBaseFileName");
            logger.Information("A test for the seq sink");
        }
    }
}
