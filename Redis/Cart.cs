using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Redis
{
    public class Cart
    {
        public Guid Id { get; private set; }
        public IEnumerable<Item> Items { get; set; }

        public Cart()
        {
            Id = Guid.NewGuid();
        }
    }

    public class Item
    {
        public string Name { get; private set; }
        public double Price { get; private set; }

        public Item(string name, double price)
        {
            Name = name;
            Price = price;
        }
    }
}