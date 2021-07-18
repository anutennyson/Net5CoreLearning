using Catalog_Final.Dtos;
using Catalog_Final.Entities;
using Catalog_Final.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog_Final.Controllers
{
    [ApiController]// This marks this class as a Controller class or APi Controller
    [Route("[controller]")]// This means whatever is the name of the controller is the name of the route
    //You can also mention explicitly
    //[Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository repo;

        //private readonly InMemItemsRepository repo;

        public ItemsController(IItemsRepository repository)
        {
            //If you want to call directly then run below code otherwise inject the dependency
            //repo = new InMemItemsRepository();
            //Below code injects depedency
            this.repo = repository;
        }

        //Asynchrnous Code Starts
        //Get /items

        [HttpGet]
        public async Task<IEnumerable<ItemDTO>> GetItemsAsync()
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

            var items = (await repo.GetItemsAsync()).Select(item => item.AsDto());
            //Above line means first (await repo.GetItemsAsync()) complete these
            //and then do the select from that
            return items;
        }

        //Get /items/{id}
        //ActionResult will let me return more than one type from this method

        //Below code is also fine but returning DTO is the better way to handle this
        //[HttpGet("{id}")]
        //public ActionResult<Items> GetItem(Guid id)
        //{
        //    var items = repo.GetItem(id);

        //    if (items is null)
        //    {
        //        return NotFound();
        //    }
        //    //you can also say OK(items)
        //    return items;
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDTO>> GetItemAsync(Guid id)
        {
            var items = await repo.GetItemAsync(id);

            if (items is null)
            {
                return NotFound();
            }
            //you can also say OK(items)
            return items.AsDto();
        }

        //Coversion with POST is to create the object and return.
        //Some people send the response but in out case we can send ItemDTO
        //We are using a different item contract here


        [HttpPost]
        //POST /items
        public async Task<ActionResult<ItemDTO>> CreateItemAsync(CreateItemDTO itemDTO)
        {
            Items item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDTO.Name,
                Price = itemDTO.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await repo.CreateItemAsync(item);

            //Return the value and then specifiy the header where the can get that information.
            //We can use CreatedAtAction or CreatedAtRoute

            return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());
            //At runtime the framework will remove Async. SO at run time GetItemAsync will
            //be GetItem ... you hvae to make sure that it is skipped by adding code in
            //Startup addcontrollers
        }

        //For PUT convention is not to return anything
        //PUT /items/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDTO itemDTO)
        {
            var exisitngItem = await repo.GetItemAsync(id);

            if (exisitngItem is null)
            {
                return NotFound();
            }

            //we are creating copy of the exisitng with modification of two properties
            Items updatedItem = exisitngItem with
            {
                Name = itemDTO.Name,
                Price = itemDTO.Price
            };

            await repo.UpdateItemAsync(updatedItem);

            //Convention is not to return anything
            return NoContent();
        }

        //Delee /item/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(Guid id)
        {
            var exisitngItem = await repo.GetItemAsync(id);

            if (exisitngItem is null)
            {
                return NotFound();
            }

            await repo.DeleteItemAsync(id);

            return NoContent(); //Http 204 is the code

            //Asynchronous Code ends


            //********************************************************************************
            //Synchronous Code Starts
            ////Get /items

            //[HttpGet]
            //public IEnumerable<ItemDTO> GetItems()
            //{
            //    //Below is the normal way but we can incorporate the DTO as well as shown below
            //    //var items = repo.GetItems();

            //    //Whole below commented can be removed using an extension method
            //    // var items = repo.GetItems().Select(item => new ItemDto{
            //    //     Id = item.id;
            //    //     Name = item.Name;
            //    //     Price = item.Price;
            //    //     CreatedDate = item.CreatedDate
            //    // })

            //    var items = repo.GetItemsAsync().Select(item => item.AsDto());
            //    return items;
            //}

            ////Get /items/{id}
            ////ActionResult will let me return more than one type from this method

            ////Below code is also fine but returning DTO is the better way to handle this
            ////[HttpGet("{id}")]
            ////public ActionResult<Items> GetItem(Guid id)
            ////{
            ////    var items = repo.GetItem(id);

            ////    if (items is null)
            ////    {
            ////        return NotFound();
            ////    }
            ////    //you can also say OK(items)
            ////    return items;
            ////}

            //[HttpGet("{id}")]
            //public ActionResult<ItemDTO> GetItem(Guid id)
            //{
            //    var items = repo.GetItemAsync(id);

            //    if (items is null)
            //    {
            //        return NotFound();
            //    }
            //    //you can also say OK(items)
            //    return items.AsDto();
            //}

            ////Coversion with POST is to create the object and return.
            ////Some people send the response but in out case we can send ItemDTO
            ////We are using a different item contract here


            //[HttpPost]
            ////POST /items
            //public ActionResult<ItemDTO> CreateItem(CreateItemDTO itemDTO)
            //{
            //    Items item = new()
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = itemDTO.Name,
            //        Price = itemDTO.Price,
            //        CreatedDate = DateTimeOffset.UtcNow
            //    };

            //    repo.CreateItemAsync(item);

            //    //Return the value and then specifiy the header where the can get that information.
            //    //We can use CreatedAtAction or CreatedAtRoute

            //    return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item.AsDto());

            //}

            ////For PUT convention is not to return anything
            ////PUT /items/{id}
            //[HttpPut("{id}")]
            //public ActionResult UpdateItem(Guid id, UpdateItemDTO itemDTO)
            //{
            //    var exisitngItem = repo.GetItemAsync(id);

            //    if (exisitngItem is null)
            //    {
            //        return NotFound();
            //    }

            //    //we are creating copy of the exisitng with modification of two properties
            //    Items updatedItem = exisitngItem with
            //    {
            //        Name = itemDTO.Name,
            //        Price = itemDTO.Price
            //    };

            //    repo.UpdateItemAsync(updatedItem);

            //    //Convention is not to return anything
            //    return NoContent();
            //}

            ////Delee /item/{id}
            //[HttpDelete("{id}")]
            //public ActionResult DeleteItem(Guid id)
            //{
            //    var exisitngItem = repo.GetItemAsync(id);

            //    if (exisitngItem is null)
            //    {
            //        return NotFound();
            //    }

            //    repo.DeleteItemAsync(id);

            //    return NoContent(); //Http 204 is the code
            //}
            //Synchronous Code ends
        }
    }
    }
