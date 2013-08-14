using System;
using System.Linq;
using OrderEndpoint;
using Raven.Client.Document;
using Raven.Client.Embedded;
using RavenDB;
using Xunit;
using Xunit.Should;

namespace NoSQLTests
{
    public class OrderTests : IDisposable
    {
        private readonly EmbeddableDocumentStore _documentStore;

        public OrderTests()
        {
            _documentStore = new EmbeddableDocumentStore
                             {
                                 RunInMemory = true,
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
        public void should_save_order_to_database()
        {
            Guid id;

            using (var session = _documentStore.OpenSession())
            {
                var endpoint = new DataHandler(session);
                id = endpoint.SaveOrder(new [] {new ItemDto(), new ItemDto()});
                session.SaveChanges();
            }

            using (var session = _documentStore.OpenSession())
            {
                var order = session.Load<Order>(id);
                order.Items.Count().ShouldBe(2);
            }
        }

    }
}