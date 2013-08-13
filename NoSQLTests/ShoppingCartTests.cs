using BookSleeve;
using ShoppingCartEndpoint;
using Xunit;

namespace NoSQLTests
{
    public class ShoppingCartTests
    {
        private DataHandler _shoppingCartEndpoint;

        public ShoppingCartTests()
        {
            _shoppingCartEndpoint = new DataHandler();
        }

        [Fact]
        public void should_connect_to_database()
        {
            using (var conn = new RedisConnection("localhost"))
            {
                conn.Open();
            }
        }

        
    }
}