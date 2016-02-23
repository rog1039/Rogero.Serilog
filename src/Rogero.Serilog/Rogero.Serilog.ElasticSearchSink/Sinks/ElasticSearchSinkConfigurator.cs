using System;
using System.Linq;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Rogero.Serilog.ElasticSearchSink.Sinks
{
    public class ElasticSearchSinkConfigurator : ISerilogConfigurator
    {
        public string[] ElasticSearchUrls { get; set; }
        public ElasticSearchIndexName IndexName { get; set; }
        public TimeSpan ShippingPeriod { get; set; }
        public string ElasticSearchTemplateName { get; set; }
        public bool AutoRegisterTemplate { get; set; }
        public bool BufferInLogsSubdirectory { get; set; }

        private ElasticsearchSinkOptions SinkOptions { get; set; }
        
        public ElasticSearchSinkConfigurator(string[] elasticSearchUrls, ElasticSearchIndexName indexName, TimeSpan shippingPeriod = default(TimeSpan),
                                             string elasticSearchTemplateName = "serilog-events-template", bool autoRegisterTemplate = true, bool bufferInLogsSubdirectory = true)
        {
            ElasticSearchUrls = elasticSearchUrls;
            IndexName = indexName;
            ShippingPeriod = shippingPeriod;
            ElasticSearchTemplateName = elasticSearchTemplateName;
            AutoRegisterTemplate = autoRegisterTemplate;
            BufferInLogsSubdirectory = bufferInLogsSubdirectory;
        }

        private static string GetBufferBaseFileName(bool bufferInLogsSubdirectory)
        {
            return bufferInLogsSubdirectory
                ? ApplicationLocation.AppBase + @"logs\esl-buffer"
                : ApplicationLocation.AppBase + @"esl-buffer";
        }
        
        public LoggerConfiguration Apply(LoggerConfiguration configuration)
        {
            SinkOptions = new ElasticsearchSinkOptions(ElasticSearchUrls.Select(z => new Uri(z)));
            SinkOptions.IndexFormat = IndexName.IndexName;
            SinkOptions.Period = ShippingPeriod == default(TimeSpan) ? TimeSpan.FromSeconds(1) : ShippingPeriod;
            SinkOptions.TemplateName = ElasticSearchTemplateName;
            SinkOptions.AutoRegisterTemplate = AutoRegisterTemplate;
            SinkOptions.BufferBaseFilename = GetBufferBaseFileName(BufferInLogsSubdirectory);

            return configuration.WriteTo.Elasticsearch(SinkOptions);
        }
    }
}
