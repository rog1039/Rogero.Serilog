using System;

namespace Rogero.Serilog.ElasticSearchSink.Sinks
{
    public class ElasticSearchIndexName
    {
        public string IndexName { get; }

        public ElasticSearchIndexName(string indexName)
        {
            ValidateIndexName(indexName);
            IndexName = indexName;
        }

        private void ValidateIndexName(string indexNameFormat)
        {
            if (!char.IsLower(indexNameFormat[0]))
                throw new ArgumentException("The first letter of the index name must be lowercase. If it is not, problems usually arise with ElasticSearch not creating the index.", nameof(indexNameFormat));
            if (!(indexNameFormat.Contains("{") && indexNameFormat.Contains("}")))
                throw new ArgumentException("The index name is parameterized on the date and so must contain a name that contains something like the following: {0:yyyy.MM.dd}", nameof(indexNameFormat));
        }

        public static ElasticSearchIndexName FromIndexName(string indexName)
        {
            return new ElasticSearchIndexName(indexName);
        }

        public static ElasticSearchIndexName FromAppName(string appName)
        {
            var indexName = appName + "-{0:yyyy.MM.dd}";
            return new ElasticSearchIndexName(indexName);
        }
    }
}