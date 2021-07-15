using Catalog_Final.Entities;
using System;
using System.Collections.Generic;

namespace Catalog_Final.Repositories
{
    public interface IItemsRepository
    {
        Items GetItem(Guid id);
        IEnumerable<Items> GetItems();
        void CreateItem(Items item);
        void UpdateItem(Items item);
        void DeleteItem(Guid Id);
    }
}