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
    public class GetItemByIdQueryHandlerTests
    {
        private readonly Mock<IItemRepository> _itemRepository;
        private readonly GetItemByIdQueryHandler _getItemByIdQueryHandler;

        public GetItemByIdQueryHandlerTests()
        {
            _itemRepository = new Mock<IItemRepository>();
            _getItemByIdQueryHandler = new GetItemByIdQueryHandler(_itemRepository.Object);
        }

        [Fact]
        public async Task Given_IdOfExistingItem_When_HandleIsCalled_Then_ReturnItem()
        {
            // Given
            var id = 1;
            var item = ItemBuilder.WithDefaults().WithId(id).WithName("Milk").Build();

            _itemRepository.Setup(repo => repo.GetItemById(id)).ReturnsAsync(item);

            var query = new GetItemByIdQuery { Id = id };

            // When
            var result = await _getItemByIdQueryHandler.Handle(query, CancellationToken.None);

            // Then
            result.Should().BeEquivalentTo(item);
        }

        [Fact]
        public async Task Given_IdOfNonExistingItem_When_HandleIsCalled_Then_ThrowItemNotFoundException()
        {
            // Given
            var id = 1;

            _itemRepository.Setup(repo => repo.GetItemById(id)).ReturnsAsync((Item?)null);

            var query = new GetItemByIdQuery { Id = id };

            // When
            Func<Task> result = async () => await _getItemByIdQueryHandler.Handle(query, CancellationToken.None);

            // Then
            await result.Should().ThrowAsync<ItemNotFoundException>();
        }
    }
}
