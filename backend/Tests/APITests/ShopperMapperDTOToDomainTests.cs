using API.Mappers;
using Tests.Builders;
using Xunit;
using FluentAssertions;

namespace Tests.APITests
{
    public class ShopperMapperDTOToDomainTests
    {
        [Fact]
        public void Given_ShopperDTO_When_MapToDomainIsCalled_Then_ReturnShopper()
        {
            // Given
            var shopperDTO = ShopperDTOBuilder.WithDefaults().WithId(1).WithName("John Doe").Build();
            var expectedShopper = ShopperBuilder.WithDefaults().WithId(1).WithName("John Doe").Build();

            // When
            var result = ShopperMapperDTOToDomain.MapToDomain(shopperDTO);

            // Then
            result.Should().BeEquivalentTo(expectedShopper);
        }
    }
}
