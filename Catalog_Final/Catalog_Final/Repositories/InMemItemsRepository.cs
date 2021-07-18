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

        public async Task<IEnumerable<Items>> GetItemsAsync()
        {
            return await Task.FromResult(items);//We are creating a task and sending the result
        }

        public async Task<Items> GetItemAsync(Guid id)
        {
            return await Task.FromResult(items.Where(items => items.Id == id).SingleOrDefault());
        }

        public async Task CreateItemAsync(Items item)
        {
            items.Add(item);
            //Here we might need a different item dto because for creation we do not need
            //all the contracts
            await Task.CompletedTask; //Here there is nothing to return
        }

        public async Task UpdateItemAsync(Items item)
        {
            //doing by this we are actually maintaining the index
            var index = items.FindIndex(exisitngItem => exisitngItem.Id == item.Id);
            items[index] = item;
            await Task.CompletedTask; //Here there is nothing to return
        }

        public async Task DeleteItemAsync(Guid Id)
        {
            var index = items.FindIndex(exisitngItem => exisitngItem.Id == Id);
            items.RemoveAt(index);
            await Task.CompletedTask; //Here there is nothing to return
        }
    }
}
