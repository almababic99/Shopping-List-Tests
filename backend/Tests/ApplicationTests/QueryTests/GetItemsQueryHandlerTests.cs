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
    public class GetItemsQueryHandlerTests
    {
        private readonly Mock<IItemRepository> _itemRepository;
        private readonly GetItemsQueryHandler _getItemsQueryHandler;

        public GetItemsQueryHandlerTests()
        {
            _itemRepository = new Mock<IItemRepository>();
            _getItemsQueryHandler = new GetItemsQueryHandler(_itemRepository.Object);
        }

        [Fact]
        public async Task Given_Items_When_HandleIsCalled_Then_ReturnItems()
        {
            // Given
            var items = new List<Item>
            {
                ItemBuilder.WithDefaults().WithId(1).WithName("Milk").Build(),
                ItemBuilder.WithDefaults().WithId(2).WithName("Apple").Build()
            };

            _itemRepository.Setup(repo => repo.GetItems()).ReturnsAsync(items);

            // When
            var result = await _getItemsQueryHandler.Handle(new GetItemsQuery(), CancellationToken.None);

            // Then
            result.Should().BeEquivalentTo(items);
        }

        [Fact]
        public async Task Given_NoItems_When_HandleIsCalled_Then_ThrowItemsNotFoundException()
        {
            // Given
            var items = new List<Item>();

            _itemRepository.Setup(repo => repo.GetItems()).ReturnsAsync(items);

            // When
            Func<Task> result = async () => await _getItemsQueryHandler.Handle(new GetItemsQuery(), CancellationToken.None);

            // Then
            await result.Should().ThrowAsync<ItemsNotFoundException>();
        }
    }
}
