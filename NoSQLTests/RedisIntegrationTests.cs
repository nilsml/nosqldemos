using System;
using System.Threading.Tasks;
using BookSleeve;
using Xunit;
using Xunit.Should;

namespace NoSQLTests
{
    public class RedisIntegrationTests
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

        [Fact]
        public async Task should_be_able_to_store_sets_in_the_database()
        {
            await WithRedisAsync(async connection =>
                            {
                                await connection.Sets.Add(_database, "key:2", "Foo");
                                await connection.Sets.Add(_database, "key:2", "Bar");
                                var result = await connection.Sets.GetAllString(_database, "key:2");
                                result.ShouldContain("Foo");
                                result.ShouldContain("Bar");
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
        
        private async static Task WithRedisAsync(Func<RedisConnection, Task> func)
        {
            using (var conn = new RedisConnection("localhost"))
            {
                await conn.Open();
                await func(conn);
            }
        }
    }
}