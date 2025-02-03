using API.Mappers;
using Tests.Builders;
using Xunit;
using FluentAssertions;

namespace Tests.APITests
{
    public class ShopperMapperDomainToDTOTests
    {
        [Fact]
        public void Given_Shopper_When_MapToDTOIsCalled_Then_ReturnShopperDTO()
        {
            // Given
            var shopper = ShopperBuilder.WithDefaults().WithId(1).WithName("John Doe").Build();
            var expectedShopperDTO = ShopperDTOBuilder.WithDefaults().WithId(1).WithName("John Doe").Build();

            // When
            var result = ShopperMapperDomainToDTO.MapToDTO(shopper);

            // Then
            result.Should().BeEquivalentTo(expectedShopperDTO);
        }
    }
}
