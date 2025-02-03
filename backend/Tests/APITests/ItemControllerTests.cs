using API.Controllers;
using API.Mappers;
using Application.Interfaces;
using Application.Queries;
using Domain.DomainModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tests.Builders;
using Xunit;
using FluentAssertions;
using Application.Commands;

namespace Tests.APITests
{
    public class ItemControllerTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<IItemService> _itemService;
        private readonly ItemController _itemController;

        public ItemControllerTests()
        {
            _mediator = new Mock<IMediator>();
            _itemService = new Mock<IItemService>();
            _itemController = new ItemController(_itemService.Object, _mediator.Object);
        }



        // Testing GetItems:
        [Fact]
        public async Task Given_Items_When_GetItemsIsCalled_Then_ReturnItems()
        {
            // Given
            var items = new List<Item>
            {
                ItemBuilder.WithDefaults().WithId(1).WithName("Milk").Build(),
                ItemBuilder.WithDefaults().WithId(2).WithName("Apple").Build()
            };

            var itemDTOs = items.Select(s => ItemMapperDomainToDTO.MapToDTO(s)).ToList();

            _mediator.Setup(m => m.Send(It.IsAny<GetItemsQuery>(), default)).ReturnsAsync(items);

            // When
            var result = await _itemController.GetItems();

            // Then
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(itemDTOs);
        }



        // Testing GetItemById:
        [Fact]
        public async Task Given_Id_When_GetItemByIdIsCalled_Then_ReturnItem()
        {
            // Given
            var item = ItemBuilder.WithDefaults().WithId(1).WithName("Milk").Build();
            var itemDTO = ItemMapperDomainToDTO.MapToDTO(item);

            _mediator.Setup(m => m.Send(It.IsAny<GetItemByIdQuery>(), default)).ReturnsAsync(item);

            // When
            var result = await _itemController.GetItemById(1);

            // Then
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(itemDTO);
        }



        // Testing AddItem:
        [Fact]
        public async Task Given_ItemDTO_When_AddItemIsCalled_Then_CreateItem()
        {
            // Given
            var itemDTO = ItemDTOBuilder.WithDefaults().WithId(1).WithName("Milk").Build();

            // When
            var result = await _itemController.AddItem(itemDTO);

            // Then
            _mediator.Verify(m => m.Send(It.Is<CreateItemCommand>(c => c.Id == itemDTO.Id && c.Name == itemDTO.Name), default));
            result.Should().NotBeNull();
            result.Should().BeOfType<OkResult>();
        }



        // Testing DeleteItem:
        [Fact]
        public async Task Given_Id_When_DeleteItemIsCalled_Then_DeleteItem()
        {
            // Given
            var id = 1;

            // When
            var result = await _itemController.DeleteItem(id);

            // Then
            _mediator.Verify(m => m.Send(It.Is<DeleteItemCommand>(c => c.Id == id), default));
            result.Should().NotBeNull();
            result.Should().BeOfType<OkResult>();
        }



        // Testing EditItem:
        [Fact]
        public async Task Given_IdAndItemDTO_When_EditItemIsCalled_Then_UpdateItem()
        {
            // Given
            var id = 1;
            var itemDTO = ItemDTOBuilder.WithDefaults().WithId(id).WithName("Milk").Build();
            var updateItemDTO = ItemDTOBuilder.WithDefaults().WithId(id).WithName("Milk Update").Build();

            // When
            var result = await _itemController.EditItem(1, updateItemDTO);

            // Then
            _mediator.Verify(m => m.Send(It.Is<UpdateItemCommand>(c => c.Id == id && c.Name == updateItemDTO.Name), default));
            result.Should().NotBeNull();
            result.Should().BeOfType<OkResult>();
        }
    }
}
