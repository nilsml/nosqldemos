using System;
using System.Runtime.Remoting.Messaging;
using BookSleeve;
using ShoppingCartEndpoint;
using Xunit;
using Xunit.Should;

namespace NoSQLTests
{
    public class ShoppingCartTests
    {
        private const int _database = 1;

        [Fact]
        public void should_connect_to_database()
        {
            WithRedis(connection => {});
        }

        [Fact]
        public void should_be_able_to_write_to_the_database()
        {
            WithRedis(connection => connection.Strings.Set(_database, "key:1", "Some value"));
        }

        [Fact]
        public void should_be_able_to_read_back_value_from_database()
        {
            WithRedis(connection =>
                      {
                          connection.Strings.Set(_database, "key:1", "Some other value");
                          var result = connection.Strings.GetString(_database, "key:1");
                          result.Result.ShouldBe("Some other value");
                      });
        }

        private static void WithRedis(Action<RedisConnection> action)
        {
            using (var conn = new RedisConnection("localhost"))
            {
                conn.Open();
                action(conn);
            }
        }
    }
}