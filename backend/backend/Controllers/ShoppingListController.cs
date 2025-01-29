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
    [Route("/api/[controller]")]  // route for this controller will be /api/shoppinglist
    public class ShoppingListController : ControllerBase
    {
        private readonly IShoppingListService _shoppingListService;
        private readonly IShopperService _shopperService;
        private readonly IItemService _itemService;
        private readonly IMediator _mediator;

        public ShoppingListController(IShoppingListService shoppingListService, IShopperService shopperService, IItemService itemService, IMediator mediator)  // Constructor for Dependency Injection 
        {
            _shoppingListService = shoppingListService;
            _shopperService = shopperService;
            _itemService = itemService;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("shoppingLists")]
        public async Task<IActionResult> GetShoppingLists()
        {
            var shoppingLists = await _mediator.Send(new GetShoppingListsQuery());

            // map each ShoppingList domain model to ShoppingListDTO model
            var shoppingListsDTOs = new List<ShoppingListDTO>();

            foreach (var shoppingList in shoppingLists)
            {
                var shoppingListDTO = await ShoppingListMapperDomainToDTO.MapToDTO(shoppingList, _shopperService, _itemService);
                shoppingListsDTOs.Add(shoppingListDTO);
            }

            return Ok(shoppingListsDTOs); // the list of ShoppingListsDTO objects is returned with a 200 OK response    
        }

        [HttpGet]
        [Route("shoppingLists/{shopperId}")]
        public async Task<IActionResult> GetShoppingListsByShopperId(int shopperId)
        {
            var shoppingLists = await _mediator.Send(new GetShoppingListsByShopperIdQuery { ShopperId = shopperId });  
            // using MediatR to send a query (GetShoppingListsByShopperIdQuery) with ShopperId = shopperId to a query handler and it waits asynchronously for the result

            // map each ShoppingList domain model to ShoppingListDTO model
            var shoppingListsDTOs = new List<ShoppingListDTO>();

            foreach (var shoppingList in shoppingLists)
            {
                var shoppingListDTO = await ShoppingListMapperDomainToDTO.MapToDTO(shoppingList, _shopperService, _itemService);
                shoppingListsDTOs.Add(shoppingListDTO);
            }

            return Ok(shoppingListsDTOs); // the list of ShoppingListsDTO objects is returned with a 200 OK response    
        }

        [HttpPost]
        [Route("addShoppingList")]
        public async Task<IActionResult> AddShoppingList([FromBody] ShoppingListDTO shoppingListDTO)   // adding shopping list and shopping list items to database
        {
            if (shoppingListDTO == null || shoppingListDTO.Items == null || !shoppingListDTO.Items.Any()) 
            { 
                return BadRequest("Shopping list and shopping list items can't be null or empty"); 
            }

            var shoppingListDomain = await ShoppingListMapperDTOToDomain.MapToDomain(shoppingListDTO, _shopperService, _itemService);  // map dto to domain

            var shoppingList = new CreateShoppingListCommand   // passing domain values to CreateShoppingListCommand
            {
                Id = shoppingListDomain.Id,
                ShopperId = shoppingListDomain.ShopperId,
                Items = shoppingListDomain.Items
            };

            await _mediator.Send(shoppingList);  

            return Ok();
        }

        [HttpDelete]
        [Route("deleteShoppingList/{id}")]
        public async Task<IActionResult> DeleteShopping(int id)
        {
            await _mediator.Send(new DeleteShoppingListCommand { Id = id });

            return Ok();
        }
    }
}
