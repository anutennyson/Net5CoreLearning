namespace Catalog.Controllers
{
    //Mark this class as API COntroller
    [ApiController]
    [Route("[controller]")] // This means whatever is the name of the controller is name 
    //of the route
// Or you cna explicity mention the route as bellow
//[Route("items")]

//Get /items
    public class ItemsController: ControllerBase
    {
        //Here we need repository -- repository is actually incharge of doing everything
        private readonly IInMemItemsRepo repo;

        public ItemsController(IInMemItemsRepo repository)
        {
            //repo = new InMemItemsRepo();
            //Instead of directly calling the class we are injecting dependency
            this.repo = repository;
        }

        [HttpGet]
        //public IEnumerable<item> GetItems()
        public IEnumerable<ItemDto> GetItems()
        {
            //Below is the normal way but we can incorporate the DTO as well as shown below
            //var items = repo.GetItems();

            //Whole below commented can be removed using an extension method
            // var items = repo.GetItems().Select(item => new ItemDto{
            //     Id = item.id;
            //     Name = item.Name;
            //     Price = item.Price;
            //     CreatedDate = item.CreatedDate
            // })
            var items = repo.GetItems().Select(item => item.AsDto());
            return items;
        }

//Get items/{id}
        [HttpGet("{id}")]
        //ActionResult will let me return more than one type from this method
        public ActionResult<item> GetItem(Guid id)
        {
            var items = repo.GetItm(id);
            if(items is null)
            {
                return NotFound();
            }
            //you can also say OK(items)
            return items;
        }
        public ActionResult<ItemDto> GetItem(Guid id)
        {
            var items = repo.GetItm(id);
            if(items is null)
            {
                return NotFound();
            }
            //you can also say OK(items)
            return items.AsDto();
        }
    }
}