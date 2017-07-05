using Nest;

namespace ElasticIndexFixture
{
    public interface IIndexManager
    {
        void CreateIndex(IElasticClient client);
    }
}