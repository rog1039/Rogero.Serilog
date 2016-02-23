using System;

namespace Rogero.Serilog.Tests
{
    public class TestObject
    {
        public int Number { get; set; } = 12;
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public string Text { get; set; } = "asdkljfaslkdjlk";
    }
}