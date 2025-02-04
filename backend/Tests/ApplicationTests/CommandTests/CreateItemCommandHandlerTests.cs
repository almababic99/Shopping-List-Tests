using Application.Commands;
using Application.Interfaces;
using Domain.DomainModels;
using FluentAssertions;
using Moq;
using Tests.Builders;
using Xunit;

namespace Tests.ApplicationTests.CommandTests
{
    public class CreateItemCommandHandlerTests
    {
        private readonly Mock<IItemRepository> _itemRepository;
        private readonly CreateItemCommandHandler _createItemCommandHandler;

        public CreateItemCommandHandlerTests()
        {
            _itemRepository = new Mock<IItemRepository>();
            _createItemCommandHandler = new CreateItemCommandHandler(_itemRepository.Object);
        }

        [Fact]
        public async Task Given_Item_When_HandleIsCalled_Then_CreateItem()
        {
            // Given
            var command = new CreateItemCommand { Id = 1, Name = "Milk" };
            var item = (Item?)null;

            _itemRepository.Setup(repo => repo.GetItem(command.Name)).ReturnsAsync(item);

            // When
            await _createItemCommandHandler.Handle(command, CancellationToken.None);

            // Then
            _itemRepository.Verify(repo => repo.AddItem(It.IsAny<Item>()));
        }

        [Fact]
        public async Task Given_ItemWithSameName_When_HandleIsCalled_Then_ThrowInvalidOperationException()
        {
            // Given
            var command = new CreateItemCommand { Id = 1, Name = "Milk" };
            var item = ItemBuilder.WithDefaults().WithId(1).WithName("Milk").Build();

            _itemRepository.Setup(repo => repo.GetItem(command.Name)).ReturnsAsync(item);

            // When
            Func<Task> result = async () => await _createItemCommandHandler.Handle(command, CancellationToken.None);

            // Then
            await result.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
