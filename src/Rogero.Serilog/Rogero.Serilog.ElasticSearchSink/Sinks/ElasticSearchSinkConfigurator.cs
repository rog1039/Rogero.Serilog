using System;
using System.Linq;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Rogero.Serilog.ElasticSearchSink.Sinks
{
    public class ElasticSearchSinkConfigurator : ISerilogConfigurator
    {
        public ElasticsearchSinkOptions SinkOptions { get; }
        
        public ElasticSearchSinkConfigurator(ElasticsearchSinkOptions sinkOptions)
        {
            SinkOptions = sinkOptions;
        }

        public ElasticSearchSinkConfigurator(string[] elasticSearchUrls, string indexNameFormat, TimeSpan shippingPeriod = default(TimeSpan),
                                             string elasticSearchTemplateName = "serilog-events-template", bool autoRegisterTemplate = true, bool bufferInLogsSubdirectory = true)
        {
            ValidateIndexName(indexNameFormat);
            SinkOptions = new ElasticsearchSinkOptions(elasticSearchUrls.Select(z => new Uri(z)));
            SinkOptions.IndexFormat = indexNameFormat;
            SinkOptions.Period = shippingPeriod == default(TimeSpan) ? TimeSpan.FromSeconds(1) : shippingPeriod;
            SinkOptions.TemplateName = elasticSearchTemplateName;
            SinkOptions.AutoRegisterTemplate = autoRegisterTemplate;
            SinkOptions.BufferBaseFilename = GetBufferBaseFileName(bufferInLogsSubdirectory);
        }

        private static string GetBufferBaseFileName(bool bufferInLogsSubdirectory)
        {
            return bufferInLogsSubdirectory
                ? ApplicationLocation.AppBase + @"logs\esl-buffer"
                : ApplicationLocation.AppBase + @"esl-buffer";
        }

        public ElasticSearchSinkConfigurator(string[] elasticSearchUrls, string applicationName, TimeSpan shippingPeriod = default(TimeSpan), bool autoRegisterTemplate = true, bool bufferInLogsSubdirectory = true, string elasticSearchTemplateName = "serilog-events-template")
        {
            ValidateIndexName(applicationName);
            SinkOptions = new ElasticsearchSinkOptions(elasticSearchUrls.Select(z => new Uri(z)));
            SinkOptions.IndexFormat = applicationName+"-{0:yyyy.MM.dd}";
            SinkOptions.Period = shippingPeriod == default(TimeSpan) ? TimeSpan.FromSeconds(1) : shippingPeriod;
            SinkOptions.TemplateName = elasticSearchTemplateName;
            SinkOptions.AutoRegisterTemplate = autoRegisterTemplate;
            SinkOptions.BufferBaseFilename = GetBufferBaseFileName(bufferInLogsSubdirectory);
        }

        private  void ValidateIndexName(string indexNameFormat)
        {
            if(char.IsLower(indexNameFormat[0]))
                return;

            throw new ArgumentException("The first letter of the index name must be lowercase. If it is not, problems usually arise with ElasticSearch not creating the index.", nameof(indexNameFormat));
        }

        public LoggerConfiguration Apply(LoggerConfiguration configuration)
        {
            return configuration.WriteTo.Elasticsearch(SinkOptions);
        }
    }
}
