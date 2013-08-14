using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using RavenDB;

namespace OrderEndpoint
{
    public class DataHandler
    {
        public IDocumentSession Session { get; set; }

        public DataHandler(IDocumentSession session)
        {
            Session = session;
        }

        public Guid SaveOrder(IEnumerable<ItemDto> orderItems)
        {
            var myOrder = new Order {Items = orderItems.Select(item => new OrderItem())};
            Session.Store(myOrder);

            return myOrder.Id;
        }

        public IEnumerable<ItemDto> LoadOrder(Guid id)
        {
            return Session.Load<Order>(id).Items.Select(item => new ItemDto());
        }
    }

    public class ItemDto
    {
    }
}