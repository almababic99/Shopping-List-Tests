using API.Mappers;
using FluentAssertions;
using Tests.Builders;
using Xunit;

namespace Tests.APITests
{
    public class ItemMapperDTOToDomainTests
    {
        [Fact]
        public void Given_ItemDTO_When_MapToDomainIsCalled_Then_ReturnItem()
        {
            // Given
            var itemDTO = ItemDTOBuilder.WithDefaults().WithId(1).WithName("Milk").WithQuantity(1).Build();
            var expectedItem = ItemBuilder.WithDefaults().WithId(1).WithName("Milk").WithQuantity(1).Build();

            // When
            var result = ItemMapperDTOToDomain.MapToDomain(itemDTO);

            // Then
            result.Should().BeEquivalentTo(expectedItem);
        }
    }
}
