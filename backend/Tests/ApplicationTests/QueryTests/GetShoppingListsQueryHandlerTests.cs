using Application.Exceptions;
using Application.Interfaces;
using Application.Queries;
using Domain.DomainModels;
using FluentAssertions;
using Moq;
using Tests.Builders;
using Xunit;

namespace Tests.ApplicationTests.QueryTests
{
    public class GetShoppingListsQueryHandlerTests
    {
        private readonly Mock<IShoppingListRepository> _shoppingListRepository;
        private readonly GetShoppingListsQueryHandler _getIShoppingListsQueryHandler;

        public GetShoppingListsQueryHandlerTests()
        {
            _shoppingListRepository = new Mock<IShoppingListRepository>();
            _getIShoppingListsQueryHandler = new GetShoppingListsQueryHandler(_shoppingListRepository.Object);
        }

        [Fact]
        public async Task Given_ShoppingLists_When_HandleIsCalled_Then_ReturnShoppingLists()
        {
            // Given
            var shoppingLists = new List<ShoppingList>
            {
                ShoppingListBuilder.WithDefaults().WithId(1).WithShopperId(1).Build(),
                ShoppingListBuilder.WithDefaults().WithId(2).WithShopperId(2).Build()
            };

            _shoppingListRepository.Setup(repo => repo.GetShoppingLists()).ReturnsAsync(shoppingLists);

            // When
            var result = await _getIShoppingListsQueryHandler.Handle(new GetShoppingListsQuery(), CancellationToken.None);

            // Then
            result.Should().BeEquivalentTo(shoppingLists);
        }

        [Fact]
        public async Task Given_NoShoppingLists_When_HandleIsCalled_Then_ThrowShoppingListsNotFoundException()
        {
            // Given
            var shoppingLists = new List<ShoppingList>();

            _shoppingListRepository.Setup(repo => repo.GetShoppingLists()).ReturnsAsync(shoppingLists);

            // When
            Func<Task> result = async () => await _getIShoppingListsQueryHandler.Handle(new GetShoppingListsQuery(), CancellationToken.None);

            // Then
            await result.Should().ThrowAsync<ShoppingListsNotFoundException>();
        }
    }
}
