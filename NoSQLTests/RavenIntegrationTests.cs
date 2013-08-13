using System;
using Raven.Client;
using Raven.Client.Document;
using Xunit;

namespace NoSQLTests
{
    public class RavenIntegrationTests : IDisposable
    {
        private readonly DocumentStore _documentStore;

        public RavenIntegrationTests()
        {
            _documentStore = new DocumentStore
                             {
                                 Url = "http://localhost:8081",
                                 Conventions =
                                 {
                                     DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites
                                 }
                             };
            _documentStore.Initialize();
        }

        public void Dispose()
        {
            _documentStore.Dispose();
        }

        [Fact]
        public void should_be_able_to_connect_to_database()
        {
            
        }

        private void WithRaven(Action<IDocumentSession> action)
        {
            using (var session = _documentStore.OpenSession())
            {
                action(session);
            }
        }
    }
}