using API.Controllers;
using API.Mappers;
using Application.Commands;
using Application.Interfaces;
using Application.Queries;
using Domain.DomainModels;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tests.Builders;
using Xunit;

namespace Tests.APITests
{
    public class ShopperControllerTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<IShopperService> _shopperService;
        private readonly ShopperController _shopperController;

        public ShopperControllerTests()
        {
            _mediator = new Mock<IMediator>();
            _shopperService = new Mock<IShopperService>();
            _shopperController = new ShopperController(_shopperService.Object, _mediator.Object );
        }



        // Testing Get:
        [Fact]
        public async Task Given_Shoppers_When_GetIsCalled_Then_ReturnShoppers()
        {
            // Given
            var shoppers = new List<Shopper>
            {
                ShopperBuilder.WithDefaults().WithId(1).WithName("John Doe").Build(),
                ShopperBuilder.WithDefaults().WithId(2).WithName("Jane Smith").Build()
            };

            var shopperDTOs = shoppers.Select(s => ShopperMapperDomainToDTO.MapToDTO(s)).ToList();

            _mediator.Setup(m => m.Send(It.IsAny<GetShoppersQuery>(), default)).ReturnsAsync(shoppers);

            // When
            var result = await _shopperController.Get();

            // Then
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(shopperDTOs);
        }



        // Testing GetShopperById:
        [Fact]
        public async Task Given_Id_When_GetShopperByIdIsCalled_Then_ReturnShopper()
        {
            // Given
            var shopper = ShopperBuilder.WithDefaults().WithId(1).WithName("John Doe").Build();
            var shopperDTO = ShopperMapperDomainToDTO.MapToDTO(shopper);

            _mediator.Setup(m => m.Send(It.IsAny<GetShopperByIdQuery>(), default)).ReturnsAsync(shopper);

            // When
            var result = await _shopperController.GetShopperById(1);

            // Then
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(shopperDTO);
        }



        // Testing AddShopper:
        [Fact]
        public async Task Given_ShopperDTO_When_AddShopperIsCalled_Then_CreateShopper()
        {
            // Given
            var shopperDTO = ShopperDTOBuilder.WithDefaults().WithId(1).WithName("John Doe").Build();

            // When
            var result = await _shopperController.AddShopper(shopperDTO);

            // Then
            _mediator.Verify(m => m.Send(It.Is<CreateShopperCommand>(c => c.Id == shopperDTO.Id && c.Name == shopperDTO.Name), default));
            result.Should().NotBeNull();
            result.Should().BeOfType<OkResult>();
        }



        // Testing DeleteShopper:
        [Fact]
        public async Task Given_Id_When_DeleteShopperIsCalled_Then_DeleteShopper()
        {
            // Given
            var id = 1;

            // When
            var result = await _shopperController.DeleteShopper(id);

            // Then
            _mediator.Verify(m => m.Send(It.Is<DeleteShopperCommand>(c => c.Id == id), default));
            result.Should().NotBeNull();
            result.Should().BeOfType<OkResult>();
        }



        // Testing EditShopper:
        [Fact]
        public async Task Given_IdAndShopperDTO_When_EditShopperIsCalled_Then_UpdateShopper()
        {
            // Given
            var id = 1;
            var shopperDTO = ShopperDTOBuilder.WithDefaults().WithId(id).WithName("John Doe").Build();
            var updateShopperDTO = ShopperDTOBuilder.WithDefaults().WithId(id).WithName("John Update").Build();

            // When
            var result = await _shopperController.EditShopper(1, updateShopperDTO);

            // Then
            _mediator.Verify(m => m.Send(It.Is<UpdateShopperCommand>(c => c.Id == id && c.Name == updateShopperDTO.Name), default));
            result.Should().NotBeNull();
            result.Should().BeOfType<OkResult>();
        }
    }
}
