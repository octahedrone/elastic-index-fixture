using System;
using System.Configuration;

namespace ElasticIndexFixture
{
    public static class ElasticServerAddressProvider
    {
        public static ElasticServerAddress GetFromEnvironment()
        {
            var serverHost = Environment.GetEnvironmentVariable("ELASTIC_TEST_SERVER_HOST");
            var serverPort = Environment.GetEnvironmentVariable("ELASTIC_TEST_SERVER_PORT");

            return string.IsNullOrWhiteSpace(serverHost)
                ? null
                : new ElasticServerAddress
                {
                    ServerPort = serverPort,
                    ServerHost = serverHost
                };
        }

        public static ElasticServerAddress GetFromConfig()
        {
            var serverHost = ConfigurationManager.AppSettings["ELASTIC_TEST_SERVER_HOST"];
            var serverPort = ConfigurationManager.AppSettings["ELASTIC_TEST_SERVER_PORT"];

            return string.IsNullOrWhiteSpace(serverHost)
                ? null
                : new ElasticServerAddress
                {
                    ServerPort = serverPort,
                    ServerHost = serverHost
                };
        }
    }
}