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
    public class GetShoppingListsByShopperIdQueryHandlerTests
    {
        private readonly Mock<IShoppingListRepository> _shoppingListRepository;
        private readonly GetShoppingListsByShopperIdQueryHandler _getShoppingListsByShopperIdQueryHandler;

        public GetShoppingListsByShopperIdQueryHandlerTests()
        {
            _shoppingListRepository = new Mock<IShoppingListRepository>();
            _getShoppingListsByShopperIdQueryHandler = new GetShoppingListsByShopperIdQueryHandler(_shoppingListRepository.Object);
        }

        [Fact]
        public async Task Given_ShoppingLists_When_HandleIsCalled_Then_ReturnShoppingLists()
        {
            // Given
            var shopperId = 1;
            var shoppingLists = new List<ShoppingList>
            {
                ShoppingListBuilder.WithDefaults().WithId(1).WithShopperId(shopperId).Build(),
                ShoppingListBuilder.WithDefaults().WithId(2).WithShopperId(shopperId).Build()
            };

            _shoppingListRepository.Setup(repo => repo.GetShoppingListsByShopperId(shopperId)).ReturnsAsync(shoppingLists);

            // When
            var result = await _getShoppingListsByShopperIdQueryHandler.Handle(new GetShoppingListsByShopperIdQuery { ShopperId = shopperId }, CancellationToken.None);

            // Then
            result.Should().BeEquivalentTo(shoppingLists);
        }

        [Fact]
        public async Task Given_NoShoppingLists_When_HandleIsCalled_Then_ThrowShoppingListsByShopperIdNotFoundException()
        {
            // Given
            var shopperId = 1;
            var shoppingLists = new List<ShoppingList>();

            _shoppingListRepository.Setup(repo => repo.GetShoppingListsByShopperId(shopperId)).ReturnsAsync(shoppingLists);

            // When
            Func<Task> result = async () => await _getShoppingListsByShopperIdQueryHandler.Handle(new GetShoppingListsByShopperIdQuery { ShopperId = shopperId }, CancellationToken.None);

            // Then
            await result.Should().ThrowAsync<ShoppingListsByShopperIdNotFoundException>();
        }
    }
}
