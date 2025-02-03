using FluentAssertions;
using Infrastructure.Mappers;
using Tests.Builders;
using Xunit;

namespace Tests.InfrastructureTests.MapperTests
{
    public class ShopperMapperDomainToEntityTests
    {
        [Fact]
        public void Given_Shopper_When_MapToEntityIsCalled_Then_ReturnShopperEntity()
        {
            // Given
            var shopper = ShopperBuilder.WithDefaults().WithId(1).WithName("John Doe").Build();
            var expectedShopperEntity = ShopperEntityBuilder.WithDefaults().WithId(1).WithName("John Doe").Build();

            // When 
            var result = ShopperMapperDomainToEntity.MapToEntity(shopper);

            // Then
            result.Should().BeEquivalentTo(expectedShopperEntity);
        }
    }
}
