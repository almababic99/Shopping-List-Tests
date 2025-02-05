using API.Controllers;
using Application.Interfaces;
using MediatR;
using Moq;
using Xunit;
using FluentAssertions;
using Domain.DomainModels;
using Tests.Builders;
using API.Mappers;
using Application.Queries;
using API.DTOModels;
using Microsoft.AspNetCore.Mvc;
using Application.Commands;

namespace Tests.APITests.ControllerTests
{
    public class ShoppingListControllerTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<IShoppingListService> _shoppingListService;
        private readonly Mock<IShopperService> _shopperService;
        private readonly Mock<IItemService> _itemService;
        private readonly ShoppingListController _shoppingListController;

        public ShoppingListControllerTests()
        {
            _mediator = new Mock<IMediator>();
            _shoppingListService = new Mock<IShoppingListService>();
            _shopperService = new Mock<IShopperService>();
            _itemService = new Mock<IItemService>();
            _shoppingListController = new ShoppingListController(_shoppingListService.Object, _shopperService.Object, _itemService.Object, _mediator.Object);
        }

        // Testing GetShoppingLists:
        [Fact]
        public async Task Given_ShoppingLists_When_GetShoppingListsIsCalled_Then_ReturnShoppingLists()
        {
            // Given
            var shoppingLists = new List<ShoppingList>
            {
                ShoppingListBuilder.WithDefaults()
                    .WithId(1)
                    .WithShopperId(1)
                    .WithItems(new List<ShoppingListItem> {
                        new ShoppingListItem { Id = 1, ShoppingListId = 1, ItemId = 1 }, 
                        new ShoppingListItem { Id = 2, ShoppingListId = 1, ItemId = 2 }
                    })
                    .Build()
            };

            var shoppingListsDTOs = new List<ShoppingListDTO>();

            foreach (var shoppingList in shoppingLists)
            {
                var shoppingListDTO = await ShoppingListMapperDomainToDTO.MapToDTO(shoppingList, _shopperService.Object, _itemService.Object);
                shoppingListsDTOs.Add(shoppingListDTO);
            }

            _mediator.Setup(m => m.Send(It.IsAny<GetShoppingListsQuery>(), default)).ReturnsAsync(shoppingLists);

            // When
            var result = await _shoppingListController.GetShoppingLists();

            // Then
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(shoppingListsDTOs);
        }



        // Testing GetShoppingListsByShopperId:
        [Fact]
        public async Task Given_ShopperId_When_GetShoppingListsByShopperIdIsCalled_Then_ReturnShoppingListsByShopperId()
        {
            // Given
            var shopperId = 1;
            var shoppingLists = new List<ShoppingList>
            {
                ShoppingListBuilder.WithDefaults()
                    .WithId(1)
                    .WithShopperId(shopperId)
                    .WithItems(new List<ShoppingListItem> {
                        new ShoppingListItem { Id = 1, ShoppingListId = 1, ItemId = 1 },
                        new ShoppingListItem { Id = 2, ShoppingListId = 1, ItemId = 2 }
                    })
                    .Build(),
            };

            var shoppingListsDTOs = new List<ShoppingListDTO>();

            foreach (var shoppingList in shoppingLists)
            {
                var shoppingListDTO = await ShoppingListMapperDomainToDTO.MapToDTO(shoppingList, _shopperService.Object, _itemService.Object);
                shoppingListsDTOs.Add(shoppingListDTO);
            }

            _mediator.Setup(m => m.Send(It.Is<GetShoppingListsByShopperIdQuery>(q => q.ShopperId == shopperId), default)).ReturnsAsync(shoppingLists);

            // When
            var result = await _shoppingListController.GetShoppingListsByShopperId(shopperId);

            // Then
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(shoppingListsDTOs);
        }



        // Testing AddShoppingList:
        [Fact]
        public async Task Given_ShoppingListDTO_When_AddShoppingListIsCalled_Then_CreateShoppingList()
        {
            // Given
            var shoppingListDTO = ShoppingListDTOBuilder.WithDefaults()
                .WithId(1)
                .WithShopper(new ShopperDTO { Id = 1, Name = "John Doe" })
                .WithItems(new List<ShoppingListItemDTO> {
                    new ShoppingListItemDTO { Id = 1, Item = ItemDTOBuilder.WithDefaults().WithId(1).WithName("Apple").WithQuantity(2).Build() }
                })
                .Build();

            var shoppingListDomain = await ShoppingListMapperDTOToDomain.MapToDomain(shoppingListDTO, _shopperService.Object, _itemService.Object);

            var shoppingList = new CreateShoppingListCommand  
            {
                Id = shoppingListDomain.Id,
                ShopperId = shoppingListDomain.ShopperId,
                Items = shoppingListDomain.Items
            };

            // When
            var result = await _shoppingListController.AddShoppingList(shoppingListDTO);

            // Then
            _mediator.Verify(m => m.Send(It.Is<CreateShoppingListCommand>(c =>
                c.Id == shoppingListDomain.Id &&
                c.ShopperId == shoppingListDomain.ShopperId &&
                c.Items.Count == shoppingListDomain.Items.Count &&
                c.Items.All(i => shoppingListDomain.Items.Any(d => d.Id == i.Id && d.ItemId == i.ItemId))
            ), It.IsAny<CancellationToken>()), Times.Once);
            result.Should().NotBeNull();
            result.Should().BeOfType<OkResult>();
        }



        // Testing DeleteShopping:
        [Fact]
        public async Task Given_Id_When_DeleteShoppingIsCalled_Then_DeleteShoppingList()
        {
            // Given
            var id = 1;

            // When
            var result = await _shoppingListController.DeleteShopping(id);

            // Then
            _mediator.Verify(m => m.Send(It.Is<DeleteShoppingListCommand>(c => c.Id == id), default));
            result.Should().NotBeNull();
            result.Should().BeOfType<OkResult>();
        }
    }
}
