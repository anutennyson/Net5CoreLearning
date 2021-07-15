namespace Catalog.Repositories
{

    public class InMemItemsRepo : IItemsRepo
    {
        private readonly List<Item> items = new()
        {
            new Item{Id= Guid.NewGuid(), Name="1", Price=9, CreatedDate=DateTimeOffset.UtcNow},
             new Item{Id= Guid.NewGuid(), Name="2", Price=99, CreatedDate=DateTimeOffset.UtcNow},
        };

        public IEnumerable<Item> GetItems()
        {
            retunr Items;
        }

        public Item GetItem(Guid id)
        {
            return items.Where(item => item.Id == id).SingleOrDefault();
        }
    }
}