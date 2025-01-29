using API.DTOModels;
using API.Mappers;
using Application.Commands;
using Application.Interfaces;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]  // route for this controller will be /api/item
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IMediator _mediator;

        public ItemController(IItemService itemService, IMediator mediator)  // Constructor for Dependency Injection of IItemService
        {
            _itemService = itemService;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("items")]
        public async Task<IActionResult> GetItems()   // The Get() method uses the _itemService.GetItems() method to fetch the list of items. This is an asynchronous operation, so it uses await to wait for the result.
        {
            var items = await _mediator.Send(new GetItemsQuery());    // using MediatR to send a query (GetItemsQuery) to a query handler and it asynchronously awaits the result

            // map each Item domain model to ItemDTO model
            var itemDTOs = items.Select(item => ItemMapperDomainToDTO.MapToDTO(item)).ToList();

            return Ok(itemDTOs); // the list of ItemDTO objects is returned with a 200 OK response            
        }

        [HttpGet]
        [Route("item/{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var item = await _mediator.Send(new GetItemByIdQuery { Id = id });   // using MediatR to send a query (GetItemByIdQuery) with Id = id to a query handler and it waits asynchronously for the result

            return Ok(item);
        }

        [HttpPost]
        [Route("addItem")]
        public async Task<IActionResult> AddItem([FromBody] ItemDTO itemDTO)
        {
            if (itemDTO == null)
                return BadRequest("Item data is null");

            await _mediator.Send(new CreateItemCommand { Id = itemDTO.Id, Name = itemDTO.Name, Quantity = itemDTO.Quantity });

            return Ok();  
        }

        [HttpDelete]
        [Route("deleteItem/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            await _mediator.Send(new DeleteItemCommand { Id = id });

            return Ok();
        }

        [HttpPut]
        [Route("editItem/{id}")]
        public async Task<IActionResult> EditItem(int id, [FromBody] ItemDTO itemDTO)
        {
            if (id != itemDTO.Id)
            {
                return BadRequest();
            }

            await _mediator.Send(new UpdateItemCommand { Id = id, Name = itemDTO.Name, Quantity = itemDTO.Quantity });

            return Ok();
        }
    }
}
