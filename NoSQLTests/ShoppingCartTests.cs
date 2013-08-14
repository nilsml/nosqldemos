using System.Collections.Generic;
using BookSleeve;
using Moq;
using ShoppingCartEndpoint;
using Xunit;

namespace NoSQLTests
{
    public class ShoppingCartTests
    {
        private readonly Mock<RedisConnection> _redisDb = new Mock<RedisConnection>();

        public ShoppingCartTests()
        {
            _redisDb.Setup(mock => mock.Strings.Set(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), false));
        }

        [Fact]
        public void should_store_object_in_database()
        {
            var endpoint = new DataHandler(_redisDb.Object);
            endpoint.SaveShoppingCart(new List<ItemDto>
                                      {
                                          new ItemDto
                                          {
                                              Name = "Name",
                                              Price = 100
                                          }
                                      });
            _redisDb.VerifyAll();
        } 
    }
}