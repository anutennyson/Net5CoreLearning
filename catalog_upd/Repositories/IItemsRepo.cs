namespace catalog.Repositories
{
    public interface IInMemItemsRepo
    {
        Item GetItem(Guid id);
        IEnumerable<item> GetItems();
    }
}