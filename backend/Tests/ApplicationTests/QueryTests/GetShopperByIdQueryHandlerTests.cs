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
    public class GetShopperByIdQueryHandlerTests
    {
        private readonly Mock<IShopperRepository> _shopperRepository;
        private readonly GetShopperByIdQueryHandler _getShopperByIdQueryHandler;

        public GetShopperByIdQueryHandlerTests()
        {
            _shopperRepository = new Mock<IShopperRepository>();
            _getShopperByIdQueryHandler = new GetShopperByIdQueryHandler(_shopperRepository.Object);
        }

        [Fact]
        public async Task Given_IdOfExistingShopper_When_HandleIsCalled_Then_ReturnShopper()
        {
            // Given
            var id = 1;
            var shopper = ShopperBuilder.WithDefaults().WithId(id).WithName("John Doe").Build();

            _shopperRepository.Setup(repo => repo.GetShopperById(id)).ReturnsAsync(shopper);

            var query = new GetShopperByIdQuery { Id = id };

            // When
            var result = await _getShopperByIdQueryHandler.Handle(query, CancellationToken.None);

            // Then
            result.Should().BeEquivalentTo(shopper);
        }

        [Fact]
        public async Task Given_IdOfNonExistingShopper_When_HandleIsCalled_Then_ThrowShopperNotFoundException()
        {
            // Given
            var id = 1;

            _shopperRepository.Setup(repo => repo.GetShopperById(id)).ReturnsAsync((Shopper?)null);

            var query = new GetShopperByIdQuery { Id = id };

            // When
            Func<Task> result = async () => await _getShopperByIdQueryHandler.Handle(query, CancellationToken.None);

            // Then
            await result.Should().ThrowAsync<ShopperNotFoundException>();
        }
    }
}
