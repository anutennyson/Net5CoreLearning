using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog_Final.Dtos
{
    public record CreateItemDTO
    {
        //here we only need the name andd the price and we do not need rest
        [Required]
        public string Name { get; init; }
        [Required]
        [Range(1,1000)]//making sure that we are not accepting negative values and also 0
        public decimal Price { get; init; }
    }
}
