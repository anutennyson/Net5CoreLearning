using Catalog_Final.Dtos;
using Catalog_Final.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog_Final
{
    //If you are creating a extension class then it has to be static
    public static class Extensions
    {
        public static ItemDTO AsDto(this Items item)
        {
            return new ItemDTO
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                CreatedDate = item.CreatedDate
            };
            
        }
    }
}
