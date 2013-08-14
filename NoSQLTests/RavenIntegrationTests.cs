using System;
using System.Linq;
using System.Threading;
using Raven.Client;
using Raven.Client.Document;
using Raven.Json.Linq;
using Xunit;
using Xunit.Should;

namespace NoSQLTests
{
    public class RavenIntegrationTests : IDisposable
    {
        private readonly DocumentStore _documentStore;
        private readonly Guid FooId = new Guid("FCDF5305-F48D-471F-842A-E659695A1E58");

        public RavenIntegrationTests()
        {
            _documentStore = new DocumentStore
                             {
                                 Url = "http://localhost:8081", 
                                 DefaultDatabase = "IntegrationTests",
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

        [Fact]
        public void should_store_one_element_to_database()
        {
            var foo = new {Id = FooId, Value = "Some string"};

            WithRaven(session =>
                      {
                          session.Store(foo);
                          session.SaveChanges();
                      });
        }

        [Fact]
        public void should_be_able_to_load_element_from_database()
        {
            WithRaven(session =>
                      {
                          var foo = session.Advanced.LuceneQuery<dynamic>().FirstOrDefault(document => document.Value == "Some string");
                          ((object)foo).ShouldNotBeNull();
                      });
        }

        [Fact]
        public void should_be_able_to_save_and_load_known_object()
        {
            var id = Guid.NewGuid();
            WithRaven(session =>
                      {
                          var bar = new Bar {Id = id, Value = "Bar string"};
                          session.Store(bar);
                          session.SaveChanges();
                      });

            WithRaven(session =>
                      {
                          var bar = session.Load<Bar>(id);
                          bar.ShouldNotBeNull();
                      });
        }

        private void WithRaven(Action<IDocumentSession> action)
        {
            using (var session = _documentStore.OpenSession())
            {
                action(session);
            }
        }
    }

    public class Bar
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
    }
}