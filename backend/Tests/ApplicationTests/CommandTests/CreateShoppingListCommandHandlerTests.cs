using Application.Commands;
using Application.Exceptions;
using Application.Interfaces;
using Domain.DomainModels;
using FluentAssertions;
using Moq;
using Tests.Builders;
using Xunit;

namespace Tests.ApplicationTests.CommandTests
{
    public class CreateShoppingListCommandHandlerTests
    {
        private readonly Mock<IShoppingListRepository> _shoppingListRepository;
        private readonly CreateShoppingListCommandHandler _createShoppingListCommandHandler;

        public CreateShoppingListCommandHandlerTests()
        {
            _shoppingListRepository = new Mock<IShoppingListRepository>();
            _createShoppingListCommandHandler = new CreateShoppingListCommandHandler(_shoppingListRepository.Object);
        }

        // One item can be found in maximum of 3 shopping lists

        [Fact]
        public async Task Given_ShoppingListWithValidItemsInLessThan3ShoppingLists_When_HandleIsCalled_Then_CreateShoppingList()
        {
            // Given
            var shoppingList = ShoppingListBuilder.WithDefaults()
                .WithId(1)
                .WithShopperId(1)
                .WithItems(new List<ShoppingListItem>{
                    new ShoppingListItem { Id = 1, ShoppingListId = 1, ItemId = 1 },
                    new ShoppingListItem { Id = 2, ShoppingListId = 1, ItemId = 2 }
                })
                .Build();

            var command = new CreateShoppingListCommand { Id = shoppingList.Id, Items = shoppingList.Items, ShopperId = shoppingList.ShopperId };

            _shoppingListRepository.Setup(repo => repo.getCountOfItemInShoppingList(It.IsAny<int>())).ReturnsAsync(2);   // each item from shoppingList is already in 2 shopping lists so they can be added
            _shoppingListRepository.Setup(repo => repo.AddShoppingList(It.IsAny<ShoppingList>())).Returns(Task.CompletedTask);

            // When
            await _createShoppingListCommandHandler.Handle(command, CancellationToken.None);

            // Then
            _shoppingListRepository.Verify(repo => repo.AddShoppingList(It.IsAny<ShoppingList>()), Times.Once);
        }

        [Fact]
        public async Task Given_ShoppingListWithInvalidItemIn3ShoppingLists_When_HandleIsCalled_Then_ThrowShoppingListItemException()
        {
            // Given
            var shoppingList = ShoppingListBuilder.WithDefaults()
                .WithId(1)
                .WithShopperId(1)
                .WithItems(new List<ShoppingListItem>{
                    new ShoppingListItem { Id = 1, ShoppingListId = 1, ItemId = 1 },
                    new ShoppingListItem { Id = 2, ShoppingListId = 1, ItemId = 2 }
                })
                .Build();

            var command = new CreateShoppingListCommand { Id = shoppingList.Id, Items = shoppingList.Items, ShopperId = shoppingList.ShopperId };

            _shoppingListRepository.Setup(repo => repo.getCountOfItemInShoppingList(1)).ReturnsAsync(3);   // item with id 1 is already in 3 shopping lists so it can't be added
            _shoppingListRepository.Setup(repo => repo.getCountOfItemInShoppingList(2)).ReturnsAsync(2);   // item with id 2 is already in 2 shopping lists so it can be added

            // When
            Func<Task> result = async () => await _createShoppingListCommandHandler.Handle(command, CancellationToken.None);

            // Then
            await result.Should().ThrowAsync<ShoppingListItemException>();
        }
    }
}
