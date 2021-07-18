using Catalog_Final.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog_Final.Repositories
{
    public interface IItemsRepository
    {
        //For the async method we have to have Async at the end - COnvention 
        //Return type has to be task to indicate that these methods are going to be async 
        //and not synchornous anymore

        //Synchronous Method Starts
        //Items GetItem(Guid id);
        //IEnumerable<Items> GetItems();
        //void CreateItem(Items item);
        //void UpdateItem(Items item);
        //void DeleteItem(Guid Id);
        //Synchronous Method Ends

        //Asynchronous Method STarts
        // So two changes 1) Change the return type to Task and 2) the add async as convention
        Task<Items> GetItemAsync(Guid id);
        Task<IEnumerable<Items>> GetItemsAsync();
        Task CreateItemAsync(Items item);
        Task UpdateItemAsync(Items item);
        Task DeleteItemAsync(Guid Id);
        //Asynchronous Method ends
    }
}