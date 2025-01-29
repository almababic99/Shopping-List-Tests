using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using API.Mappers;
using API.DTOModels;
using MediatR;
using Application.Queries;
using Application.Commands;

namespace API.Controllers
{

    [ApiController]
    [Route("/api/[controller]")]  // route for this controller will be /api/shopper
    public class ShopperController : ControllerBase
    {
        private readonly IShopperService _shopperService;
        private readonly IMediator _mediator;

        public ShopperController(IShopperService shopperService, IMediator mediator)  // Constructor for Dependency Injection of IShopperService
        {
            _shopperService = shopperService;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("shoppers")]
        public async Task<IActionResult> Get()   // The Get() method uses the _shopperService.GetShoppers() method to fetch the list of shoppers. This is an asynchronous operation, so it uses await to wait for the result.
        { 
            var shoppers = await _mediator.Send(new GetShoppersQuery());  // using MediatR to send a query (GetShoppersQuery) to a query handler and it asynchronously awaits the result

            // map each Shopper domain model to ShopperDTO model
            var shopperDTOs = shoppers.Select(shopper => ShopperMapperDomainToDTO.MapToDTO(shopper)).ToList();

            return Ok(shopperDTOs); // the list of ShopperDTO objects is returned with a 200 OK response            
        }

        [HttpGet]
        [Route("shopper/{id}")]
        public async Task<IActionResult> GetShopperById(int id)
        {
            var shopper = await _mediator.Send(new GetShopperByIdQuery { Id = id });  // using MediatR to send a query (GetShopperByIdQuery) with Id = id to a query handler and it waits asynchronously for the result

            return Ok(shopper);
        }

        [HttpPost]
        [Route("addShopper")]
        public async Task<IActionResult> AddShopper([FromBody] ShopperDTO shopperDTO)
        {
            if (shopperDTO == null)
                return BadRequest("Shopper data is null");

            await _mediator.Send(new CreateShopperCommand { Id = shopperDTO.Id, Name = shopperDTO.Name });

            return Ok();
        }

        [HttpDelete]
        [Route("deleteShopper/{id}")]
        public async Task<IActionResult> DeleteShopper(int id)
        {
            await _mediator.Send(new DeleteShopperCommand { Id = id });

            return Ok();
        }

        [HttpPut]
        [Route("editShopper/{id}")]

        public async Task<IActionResult> EditShopper(int id, [FromBody] ShopperDTO shopperDTO)
        {
            if (id != shopperDTO.Id)
            {
                return BadRequest();
            }

            await _mediator.Send(new UpdateShopperCommand { Id = id, Name = shopperDTO.Name });

            return Ok();
        }
    }
}
