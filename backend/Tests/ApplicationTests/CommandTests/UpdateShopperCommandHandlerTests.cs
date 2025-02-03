using Application.Commands;
using Application.Interfaces;
using Moq;
using Tests.Builders;
using Xunit;
using Domain.DomainModels;

namespace Tests.ApplicationTests.CommandTests
{
    public class UpdateShopperCommandHandlerTests
    {
        private readonly Mock<IShopperRepository> _shopperRepository;
        private readonly UpdateShopperCommandHandler _updateShopperCommandHandler;

        public UpdateShopperCommandHandlerTests()
        {
            _shopperRepository = new Mock<IShopperRepository>();
            _updateShopperCommandHandler = new UpdateShopperCommandHandler(_shopperRepository.Object);
        }

        [Fact]
        public async Task Given_ShopperToUpdate_When_HandleIsCalled_Then_EditShopper()
        {
            // Given
            var shopper = ShopperBuilder.WithDefaults().WithId(1).WithName("John Doe").Build();
            var command = new UpdateShopperCommand
            {
                Id = shopper.Id,
                Name = shopper.Name
            };

            // When
            await _updateShopperCommandHandler.Handle(command, CancellationToken.None);

            // Then
            _shopperRepository.Verify(x => x.EditShopper(It.Is<Shopper>(i => i.Id == shopper.Id && i.Name == shopper.Name)));
        }
    }
}
