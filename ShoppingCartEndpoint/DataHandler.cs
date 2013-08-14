using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BookSleeve;
using Redis;

namespace ShoppingCartEndpoint
{
    public class DataHandler
    {
        private readonly RedisConnection _connection;

        public DataHandler(RedisConnection connection)
        {
            _connection = connection;
        }

        public Guid SaveShoppingCart(IEnumerable<ItemDto> shoppingCart)
        {
            var cart = new Cart { Items = shoppingCart.Select(item => new Item(item.Name, item.Price)) };
            var serializedCart = Newtonsoft.Json.JsonConvert.SerializeObject(cart);
            _connection.Strings.Set(1, cart.Id.ToString(), serializedCart);

            return cart.Id;
        }

        public IEnumerable<ItemDto> LoadShoppingCart(Guid id)
        {
            var serializedCart = _connection.Strings.GetString(1, id.ToString());
            var cart = Newtonsoft.Json.JsonConvert.DeserializeObject<Cart>(serializedCart.Result);

            return cart.Items.Select(item => new ItemDto
                                             {
                                                 Name = item.Name,
                                                 Price = item.Price
                                             });
        }
     }

    public class ItemDto
    {
        public string Name { get; set; }
        public double Price { get; set; }
    }
}