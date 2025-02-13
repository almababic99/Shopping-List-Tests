﻿using Application.Interfaces;
using Application.Queries;
using Domain.DomainModels;
using Moq;
using Xunit;
using Tests.Builders;
using FluentAssertions;
using Application.Exceptions;

namespace Tests.ApplicationTests.QueryTests
{
    public class GetShoppersQueryHandlerTests
    {
        private readonly Mock<IShopperRepository> _shopperRepository;
        private readonly GetShoppersQueryHandler _getShoppersQueryHandler;

        public GetShoppersQueryHandlerTests()
        {
            _shopperRepository = new Mock<IShopperRepository>();
            _getShoppersQueryHandler = new GetShoppersQueryHandler(_shopperRepository.Object);
        }

        [Fact]
        public async Task Given_Shoppers_When_HandleIsCalled_Then_ReturnShoppers()
        {
            // Given
            var shoppers = new List<Shopper>
            {
                ShopperBuilder.WithDefaults().WithId(1).WithName("John Doe").Build(),
                ShopperBuilder.WithDefaults().WithId(2).WithName("Jane Smith").Build()
            };

            _shopperRepository.Setup(repo => repo.GetShoppers()).ReturnsAsync(shoppers);

            // When
            var result = await _getShoppersQueryHandler.Handle(new GetShoppersQuery(), CancellationToken.None);

            // Then
            result.Should().BeEquivalentTo(shoppers);
        }

        [Fact]
        public async Task Given_NoShoppers_When_HandleIsCalled_Then_ThrowShoppersNotFoundException()
        {
            // Given
            var shoppers = new List<Shopper>();

            _shopperRepository.Setup(repo => repo.GetShoppers()).ReturnsAsync(shoppers);

            // When
            Func<Task> result = async () => await _getShoppersQueryHandler.Handle(new GetShoppersQuery(), CancellationToken.None);

            // Then
            await result.Should().ThrowAsync<ShoppersNotFoundException>();
        }
    }
}
