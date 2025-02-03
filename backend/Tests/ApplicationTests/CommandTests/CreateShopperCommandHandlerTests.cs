using Application.Commands;
using Application.Interfaces;
using Domain.DomainModels;
using FluentAssertions;
using Moq;
using Tests.Builders;
using Xunit;

namespace Tests.ApplicationTests.CommandTests
{
    public class CreateShopperCommandHandlerTests
    {
        private readonly Mock<IShopperRepository> _shopperRepository;
        private readonly CreateShopperCommandHandler _createShopperCommandHandler;

        public CreateShopperCommandHandlerTests()
        {
            _shopperRepository = new Mock<IShopperRepository>();
            _createShopperCommandHandler = new CreateShopperCommandHandler(_shopperRepository.Object);
        }

        [Fact]
        public async Task Given_Shopper_When_HandleIsCalled_Then_CreateShopper()
        {
            // Given
            var command = new CreateShopperCommand { Id = 1, Name = "John Doe" };
            var shopper = (Shopper?)null;

            _shopperRepository.Setup(repo => repo.GetShopper(command.Name)).ReturnsAsync(shopper);

            // When
            await _createShopperCommandHandler.Handle(command, CancellationToken.None);

            // Then
            _shopperRepository.Verify(repo => repo.AddShopper(It.IsAny<Shopper>()));
        }

        [Fact]
        public async Task Given_ShopperWithSameName_When_HandleIsCalled_Then_ThrowInvalidOperationException()
        {
            // Given
            var command = new CreateShopperCommand { Id = 1, Name = "John Doe" };
            var shopper = ShopperBuilder.WithDefaults().WithId(1).WithName("John Doe").Build();

            _shopperRepository.Setup(repo => repo.GetShopper(command.Name)).ReturnsAsync(shopper);

            // When
            Func<Task> result = async () => await _createShopperCommandHandler.Handle(command, CancellationToken.None);

            // Then
            await result.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
