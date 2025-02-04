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
    public class DeleteShoppingListCommandHandlerTests
    {
        private readonly Mock<IShoppingListRepository> _shoppingListRepository;
        private readonly DeleteShoppingListCommandHandler _deleteShoppingListCommandHandler;

        public DeleteShoppingListCommandHandlerTests()
        {
            _shoppingListRepository = new Mock<IShoppingListRepository>();
            _deleteShoppingListCommandHandler = new DeleteShoppingListCommandHandler(_shoppingListRepository.Object);
        }

        [Fact]
        public async Task Given_ExistingShoppingList_When_HandleIsCalled_Then_DeleteShoppingList()
        {
            // Given
            var id = 1;
            var shoppingLists = new List<ShoppingList>
            {
                ShoppingListBuilder.WithDefaults().WithId(id).WithShopperId(1).Build()
            };

            _shoppingListRepository.Setup(repo => repo.GetShoppingLists()).ReturnsAsync(shoppingLists);
            _shoppingListRepository.Setup(repo => repo.DeleteShoppingList(id)).Returns(Task.CompletedTask);

            // When
            await _deleteShoppingListCommandHandler.Handle(new DeleteShoppingListCommand { Id = id }, CancellationToken.None);

            // Then
            _shoppingListRepository.Verify(repo => repo.DeleteShoppingList(id), Times.Once);
        }

        [Fact]
        public async Task Given_NonExistingShoppingList_When_HandleIsCalled_Then_ThrowShoppingListsNotFoundException()
        {
            // Given
            var id = 1;
            var shoppingLists = new List<ShoppingList>(); 

            _shoppingListRepository.Setup(repo => repo.GetShoppingLists()).ReturnsAsync(shoppingLists);

            // When
            Func<Task> result = async () => await _deleteShoppingListCommandHandler.Handle(new DeleteShoppingListCommand { Id = id }, CancellationToken.None);

            // Then
            await result.Should().ThrowAsync<ShoppingListsNotFoundException>();
        }
    }
}
