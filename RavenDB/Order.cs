using System;
using System.Collections.Generic;

namespace RavenDB
{
    public class Order
    {
        public Guid Id { get; private set; }
        public IEnumerable<OrderItem> Items { get; set; }

        public Order()
        {
            Id = Guid.NewGuid();
        }
    }

    public class OrderItem
    {

    }
}