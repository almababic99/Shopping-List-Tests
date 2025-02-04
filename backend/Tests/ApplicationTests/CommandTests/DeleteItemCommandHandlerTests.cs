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
    public class DeleteItemCommandHandlerTests
    {
        private readonly Mock<IItemRepository> _itemRepository;
        private readonly DeleteItemCommandHandler _deleteItemCommandHandler;

        public DeleteItemCommandHandlerTests()
        {
            _itemRepository = new Mock<IItemRepository>();
            _deleteItemCommandHandler = new DeleteItemCommandHandler(_itemRepository.Object);
        }

        [Fact]
        public async Task Given_ExistingItem_When_HandleIsCalled_Then_DeleteItem()
        {
            // Given
            var id = 1;
            var item = ItemBuilder.WithDefaults().WithId(id).WithName("Milk").Build();

            var command = new DeleteItemCommand { Id = id };

            _itemRepository.Setup(repo => repo.GetItemById(id)).ReturnsAsync(item);

            // When
            await _deleteItemCommandHandler.Handle(command, CancellationToken.None);

            // Then
            _itemRepository.Verify(repo => repo.DeleteItem(id), Times.Once());
        }

        [Fact]
        public async Task Given_NonExistingItem_When_HandleIsCalled_Then_ThrowItemNotFoundException()
        {
            // Given
            _itemRepository.Setup(repo => repo.GetItemById(1)).ReturnsAsync((Item?)null);

            var command = new DeleteItemCommand { Id = 1 };

            // When
            Func<Task> result = async () => await _deleteItemCommandHandler.Handle(command, CancellationToken.None);

            // Then
            await result.Should().ThrowAsync<ItemNotFoundException>();
        }
    }
}
