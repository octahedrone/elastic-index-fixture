using System;
using System.Threading;
using Nest;

namespace ElasticIndexFixture
{
    public abstract class ElasticIndexFixtureBase
    {
        private ElasticServerAddress _serverAddress;

        public string IndexName { get; }

        public int IndexRefreshDelayMs { get; }

        public ConnectionSettings ConnectionSettings { get; }

        public ElasticIndexFixtureBase(IIndexManager indexManager,
                                       ElasticServerAddress serverAddress,
                                       string defaultIndexName,
                                       int indexRefreshDelayMs = 2000)
        {
            if (indexManager == null) throw new ArgumentNullException(nameof(indexManager));
            if (serverAddress == null) throw new ArgumentNullException(nameof(serverAddress));
            if (defaultIndexName == null) throw new ArgumentNullException(nameof(defaultIndexName));

            IndexName = defaultIndexName;
            IndexRefreshDelayMs = indexRefreshDelayMs;

            _serverAddress = serverAddress;

            var uri = new Uri($"{serverAddress.ServerHost}:{serverAddress.ServerPort}");
            ConnectionSettings = new ConnectionSettings(uri, IndexName);

            SetUpIndex(indexManager);
        }

        private void SetUpIndex(IIndexManager indexManager)
        {
            var client = new ElasticClient(ConnectionSettings);

            var existsResponse = client.IndexExists(i => i.Index(IndexName));

            if (existsResponse.Exists)
            {
                var deleteResponse = client.DeleteIndex(i => i.Index(IndexName));
                if (!deleteResponse.Acknowledged)
                    throw new Exception("Unable to delete index");

                Thread.Sleep(5000);
            }

            indexManager.CreateIndex(client);
            Thread.Sleep(2000);

            existsResponse = client.IndexExists(i => i.Index(IndexName));
            if (!existsResponse.Exists)
                throw new Exception("Unable to create index");
        }

        public IElasticClient Create()
        {
            return new ElasticClient(ConnectionSettings);
        }

        public void DeleteIndex()
        {
            var client = new ElasticClient(ConnectionSettings);
            client.DeleteIndex(i => i.Index(IndexName));
        }
    }
}