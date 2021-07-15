//This can be used for DTO to model 
namespace Catalog
{
    //For extentsion methods we have to have the static classes
    public static class Extensions{

        public static ItemDto AsDto(this Item item)
        {
            new ItemDto{
                Id = item.id;
                Name = item.Name;
                Price = item.Price;
                CreatedDate = item.CreatedDate
            };
        }
    }
}