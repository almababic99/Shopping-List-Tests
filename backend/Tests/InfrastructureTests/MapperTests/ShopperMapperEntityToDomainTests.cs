using Infrastructure.Mappers;
using Tests.Builders;
using Xunit;
using FluentAssertions;

namespace Tests.InfrastructureTests.MapperTests
{
    public class ShopperMapperEntityToDomainTests
    {
        [Fact]
        public void Given_ShopperEntity_When_MapToDomainIsCalled_Then_ReturnShopper()
        {
            // Given
            var shopperEntity = ShopperEntityBuilder.WithDefaults().WithId(1).WithName("John Doe").Build();
            var expectedShopper = ShopperBuilder.WithDefaults().WithId(1).WithName("John Doe").Build();

            // When 
            var result = ShopperMapperEntityToDomain.MapToDomain(shopperEntity);

            // Then
            result.Should().BeEquivalentTo(expectedShopper);
        }
    }
}
