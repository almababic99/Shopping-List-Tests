using Application.Commands;
using Application.Interfaces;
using Domain.DomainModels;
using Moq;
using Tests.Builders;
using Xunit;

namespace Tests.ApplicationTests
{
    public class UpdateItemCommandHandlerTests
    {
        private readonly Mock<IItemRepository> _itemRepository;
        private readonly UpdateItemCommandHandler _handler;

        public UpdateItemCommandHandlerTests()
        {
            _itemRepository = new Mock<IItemRepository>();
            _handler = new UpdateItemCommandHandler(_itemRepository.Object);
        }

        [Fact]
        public async Task Given_ItemToUpdate_When_HandleIsCalled_Then_EditItem()
        {
            // Given
            var item = ItemBuilder.WithDefaults().WithId(1).WithName("Milk").WithQuantity(1).Build();
            var command = new UpdateItemCommand
            {
                Id = item.Id,
                Name = item.Name,
                Quantity = item.Quantity
            };

            // When
            await _handler.Handle(command, CancellationToken.None);

            // Then
            _itemRepository.Verify(x => x.EditItem(It.Is<Item>(i => i.Id == item.Id && i.Name == item.Name && i.Quantity == item.Quantity)));
        }
    }
}
