using Catalog_Final.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog_Final.Repositories
{
    public class InMemItemsRepository : IItemsRepository
    {
        private readonly List<Items> items = new()
        {
            new Items { Id = Guid.NewGuid(), Name = "One", Price = 9, CreatedDate = DateTimeOffset.UtcNow },
            new Items { Id = Guid.NewGuid(), Name = "Two", Price = 99, CreatedDate = DateTimeOffset.UtcNow },
            new Items { Id = Guid.NewGuid(), Name = "Three", Price = 999, CreatedDate = DateTimeOffset.UtcNow }
        };

        public IEnumerable<Items> GetItems()
        {
            return items;
        }

        public Items GetItem(Guid id)
        {
            return items.Where(items => items.Id == id).SingleOrDefault();
        }

        public void CreateItem(Items item)
        {
            items.Add(item);
            //Here we might need a different item dto because for creation we do not need
            //all the contracts
        }

        public void UpdateItem(Items item)
        {
            //doing by this we are actually maintaining the index
            var index = items.FindIndex(exisitngItem => exisitngItem.Id == item.Id);
            items[index] = item;
        }

        public void DeleteItem(Guid Id)
        {
            var index = items.FindIndex(exisitngItem => exisitngItem.Id == Id);
            items.RemoveAt(index);
        }
    }
}
