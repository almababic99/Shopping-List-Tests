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
    public class DeleteShopperCommandHandlerTests
    {
        private readonly Mock<IShopperRepository> _shopperRepository;
        private readonly DeleteShopperCommandHandler _deleteShopperCommandHandler;

        public DeleteShopperCommandHandlerTests()
        {
            _shopperRepository = new Mock<IShopperRepository>();
            _deleteShopperCommandHandler = new DeleteShopperCommandHandler(_shopperRepository.Object);
        }

        [Fact]
        public async Task Given_ExistingShopper_When_HandleIsCalled_Then_DeleteShopper()
        {
            // Given
            var id = 1;
            var shopper = ShopperBuilder.WithDefaults().WithId(id).WithName("John Doe").Build();

            var command = new DeleteShopperCommand { Id = id };

            _shopperRepository.Setup(repo => repo.GetShopperById(id)).ReturnsAsync(shopper);

            // When
            await _deleteShopperCommandHandler.Handle(command, CancellationToken.None);

            // Then
            _shopperRepository.Verify(repo => repo.DeleteShopper(id), Times.Once());
        }

        [Fact]
        public async Task Given_NonExistingShopper_When_HandleIsCalled_Then_ThrowShopperNotFoundException()
        {
            // Given
            _shopperRepository.Setup(repo => repo.GetShopperById(1)).ReturnsAsync((Shopper?)null);

            var command = new DeleteShopperCommand { Id = 1 };

            // When
            Func<Task> result = async () => await _deleteShopperCommandHandler.Handle(command, CancellationToken.None);

            // Then
            await result.Should().ThrowAsync<ShopperNotFoundException>();
        }
    }
}
