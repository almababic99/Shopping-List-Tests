using API.Mappers;
using Tests.Builders;
using Xunit;
using FluentAssertions;

namespace Tests.APITests.MapperTests
{
    public class ItemMapperDomainToDTOTests
    {
        [Fact]
        public void Given_Item_When_MapToDTOIsCalled_Then_ReturnItemDTO()
        {
            // Given
            var item = ItemBuilder.WithDefaults().WithId(1).WithName("Milk").WithQuantity(1).Build();
            var expectedItemDTO = ItemDTOBuilder.WithDefaults().WithId(1).WithName("Milk").WithQuantity(1).Build();

            // When
            var result = ItemMapperDomainToDTO.MapToDTO(item);

            // Then
            result.Should().BeEquivalentTo(expectedItemDTO);
        }
    }
}
